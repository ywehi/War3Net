﻿// ------------------------------------------------------------------------------
// <copyright file="MpqArchiveBuilder.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace War3Net.IO.Mpq
{
    public sealed class MpqArchiveBuilder : IEnumerable<MpqFile>
    {
        private readonly ushort _originalHashTableSize;
        private readonly List<MpqFile> _originalFiles;
        private readonly List<MpqFile> _modifiedFiles;
        private readonly List<ulong> _removedFiles;

        public MpqArchiveBuilder()
        {
            _originalHashTableSize = 0;
            _originalFiles = new List<MpqFile>();
            _modifiedFiles = new List<MpqFile>();
            _removedFiles = new List<ulong>();
        }

        public MpqArchiveBuilder(MpqArchive originalMpqArchive)
        {
            if (originalMpqArchive is null)
            {
                throw new ArgumentNullException(nameof(originalMpqArchive));
            }

            _originalHashTableSize = (ushort)originalMpqArchive.HashTableSize;
            _originalFiles = new List<MpqFile>(originalMpqArchive.GetMpqFiles());
            _modifiedFiles = new List<MpqFile>();
            _removedFiles = new List<ulong>();
        }

        public void AddFile(MpqFile file)
        {
            _modifiedFiles.Add(file);
        }

        public void RemoveFile(ulong hashedFileName)
        {
            _removedFiles.Add(hashedFileName);
        }

        public void RemoveFile(string fileName)
        {
            RemoveFile(MpqHash.GetHashedFileName(fileName));
        }

        public void SaveTo(string fileName)
        {
            using (var stream = File.Create(fileName))
            {
                SaveTo(stream);
            }
        }

        public void SaveTo(Stream stream, bool leaveOpen = false)
        {
            var options = new MpqArchiveCreateOptions
            {
                HashTableSize = _originalHashTableSize,
            };

            MpqArchive.Create(stream, GetMpqFiles().ToArray(), options, leaveOpen).Dispose();
        }

        /// <inheritdoc/>
        public IEnumerator<MpqFile> GetEnumerator() => GetMpqFiles().GetEnumerator();

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => GetMpqFiles().GetEnumerator();

        private IEnumerable<MpqFile> GetMpqFiles()
        {
            return _modifiedFiles.Concat(_originalFiles.Where(originalFile =>
                !_removedFiles.Contains(originalFile.Name) &&
                !_modifiedFiles.Where(modifiedFile => modifiedFile.Equals(originalFile)).Any()));
        }
    }
}