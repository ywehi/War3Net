﻿// ------------------------------------------------------------------------------
// <copyright file="MapBuilder.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using War3Net.Build.Providers;
using War3Net.Build.Script;
using War3Net.CodeAnalysis.Jass.Renderer;
using War3Net.IO.Mpq;

namespace War3Net.Build
{
    public class MapBuilder
    {
        private string _outputMapName;
        private ushort _blockSize;
        private bool _generateListfile;

        public MapBuilder()
        {
            _outputMapName = "TestMap.w3x";
            _blockSize = 3;
            _generateListfile = true;
        }

        public MapBuilder(string mapName)
        {
            _outputMapName = mapName;
            _blockSize = 3;
            _generateListfile = true;
        }

        public MapBuilder(string mapName, ushort blockSize)
        {
            _outputMapName = mapName;
            _blockSize = blockSize;
            _generateListfile = true;
        }

        public MapBuilder(string mapName, ushort blockSize, bool generateListfile)
        {
            _outputMapName = mapName;
            _blockSize = blockSize;
            _generateListfile = generateListfile;
        }

        public string OutputMapName
        {
            get => _outputMapName;
            set => _outputMapName = value;
        }

        public ushort BlockSize
        {
            get => _blockSize;
            set => _blockSize = value;
        }

        public bool GenerateListfile
        {
            get => _generateListfile;
            set => _generateListfile = value;
        }

        public bool Build(ScriptCompilerOptions compilerOptions, params string[] assetsDirectories)
        {
            Directory.CreateDirectory(compilerOptions.OutputDirectory);

            var files = new Dictionary<(string fileName, MpqLocale locale), Stream>();

            // Generate mapInfo file
            if (compilerOptions.MapInfo != null)
            {
                var path = Path.Combine(compilerOptions.OutputDirectory, MapInfo.FileName);
                using (var fileStream = File.Create(path))
                {
                    compilerOptions.MapInfo.SerializeTo(fileStream);
                }

                files.Add((new FileInfo(path).Name, MpqLocale.Neutral), File.OpenRead(path));
            }
            else
            {
                // TODO: set MapInfo by parsing war3map.w3i from assetsDirectories, because it's needed for compilation.
                // compilerOptions.MapInfo = ...;
                throw new NotImplementedException();
            }

            // Generate script file
            if (compilerOptions.SourceDirectory != null)
            {
                if (Compile(compilerOptions, out var path))
                {
                    files.Add((new FileInfo(path).Name, MpqLocale.Neutral), File.OpenRead(path));
                }
                else
                {
                    return false;
                }
            }

            // Load assets
            foreach (var assetsDirectory in assetsDirectories)
            {
                if (string.IsNullOrWhiteSpace(assetsDirectory))
                {
                    continue;
                }

                foreach (var (fileName, locale, stream) in FileProvider.EnumerateFiles(assetsDirectory))
                {
                    if (files.ContainsKey((fileName, locale)))
                    {
                        stream.Dispose();
                    }
                    else
                    {
                        files.Add((fileName, locale), stream);
                    }
                }
            }

            // Generate (listfile)
            var generateListfile = compilerOptions.FileFlags.TryGetValue(ListFile.Key, out var listfileFlags)
                ? listfileFlags.HasFlag(MpqFileFlags.Exists)
                : _generateListfile; // compilerOptions.DefaultFileFlags.HasFlag(MpqFileFlags.Exists);

            if (generateListfile)
            {
                var listfilePath = Path.Combine(compilerOptions.OutputDirectory, ListFile.Key);
                using (var listfileStream = File.Create(listfilePath))
                {
                    using (var streamWriter = new StreamWriter(listfileStream, new UTF8Encoding(false)))
                    {
                        foreach (var file in files)
                        {
                            streamWriter.WriteLine(file.Key);
                        }
                    }
                }

                if (files.ContainsKey((ListFile.Key, MpqLocale.Neutral)))
                {
                    files[(ListFile.Key, MpqLocale.Neutral)].Dispose();
                    files.Remove((ListFile.Key, MpqLocale.Neutral));
                }

                files.Add((ListFile.Key, MpqLocale.Neutral), File.OpenRead(listfilePath));
            }

            // Generate mpq files
            var mpqFiles = new List<MpqFile>(files.Count);
            foreach (var file in files)
            {
                var fileflags = compilerOptions.FileFlags.TryGetValue(file.Key.fileName, out var flags) ? flags : compilerOptions.DefaultFileFlags;
                if (fileflags.HasFlag(MpqFileFlags.Exists))
                {
                    mpqFiles.Add(new MpqFile(file.Value, file.Key.fileName, file.Key.locale, fileflags, _blockSize));
                }
                else
                {
                    file.Value.Dispose();
                }
            }

            // Generate .mpq archive file
            var outputMap = Path.Combine(compilerOptions.OutputDirectory, _outputMapName);
            MpqArchive.Create(File.Create(outputMap), mpqFiles, blockSize: _blockSize).Dispose();

            return true;
        }

        /*public bool Compile(ScriptCompilerOptions options, out string scriptFilePath)
        {
            var compiler = ScriptCompiler.GetUnknownLanguageCompiler(options);
            if (compiler is null)
            {
                scriptFilePath = null;
                return false;
            }

            var scriptBuilder = compiler.GetScriptBuilder();
            scriptFilePath = Path.Combine(options.OutputDirectory, $"war3map{scriptBuilder.Extension}");

            var mainFunctionFile = Path.Combine(options.OutputDirectory, $"main{scriptBuilder.Extension}");
            scriptBuilder.BuildMainFunction(mainFunctionFile, options.BuilderOptions);

            var configFunctionFile = Path.Combine(options.OutputDirectory, $"config{scriptBuilder.Extension}");
            scriptBuilder.BuildConfigFunction(configFunctionFile, options.BuilderOptions);

            return compiler.Compile(mainFunctionFile, configFunctionFile);
        }*/

        public bool Compile(ScriptCompilerOptions options, out string scriptFilePath)
        {
            var compiler = GetCompiler(options);
            compiler.BuildMainAndConfig(out var mainFunctionFilePath, out var configFunctionFilePath);
            return compiler.Compile(out scriptFilePath, mainFunctionFilePath, configFunctionFilePath);
        }

        private ScriptCompiler GetCompiler(ScriptCompilerOptions options)
        {
            switch (options.MapInfo.ScriptLanguage)
            {
                case ScriptLanguage.Jass:
                    return new JassScriptCompiler(options, JassRendererOptions.Default);

                case ScriptLanguage.Lua:
                    // TODO: distinguish C# and lua
                    var rendererOptions = new CSharpLua.LuaSyntaxGenerator.SettingInfo();
                    rendererOptions.Indent = 4;

                    return new CSharpScriptCompiler(options, rendererOptions);

                default:
                    throw new Exception();
            }
        }
    }
}