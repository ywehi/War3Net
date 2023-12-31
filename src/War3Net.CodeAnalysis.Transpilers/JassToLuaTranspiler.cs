﻿// ------------------------------------------------------------------------------
// <copyright file="JassToLuaTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Reflection;

using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        private readonly Dictionary<string, JassTypeSyntax> _functionReturnTypes;
        private readonly Dictionary<string, JassTypeSyntax> _globalTypes;
        private readonly Dictionary<string, JassTypeSyntax> _localTypes;

        public JassToLuaTranspiler()
        {
            _functionReturnTypes = new(StringComparer.Ordinal);
            _globalTypes = new(StringComparer.Ordinal);
            _localTypes = new(StringComparer.Ordinal);
        }

        public bool IgnoreComments { get; set; }

        public bool IgnoreEmptyDeclarations { get; set; }

        public bool IgnoreEmptyStatements { get; set; }

        public bool KeepFunctionsSeparated { get; set; }

        public void RegisterJassFile(JassCompilationUnitSyntax compilationUnit)
        {
            foreach (var declaration in compilationUnit.Declarations)
            {
                if (declaration is JassGlobalDeclarationListSyntax globalDeclarationList)
                {
                    foreach (var global in globalDeclarationList.Globals)
                    {
                        if (global is JassGlobalDeclarationSyntax globalDeclaration)
                        {
                            RegisterVariableType(globalDeclaration.Declarator, false);
                        }
                    }
                }
                else if (declaration is JassNativeFunctionDeclarationSyntax nativeFunctionDeclaration)
                {
                    RegisterFunctionReturnType(nativeFunctionDeclaration.FunctionDeclarator);
                }
                else if (declaration is JassFunctionDeclarationSyntax functionDeclaration)
                {
                    RegisterFunctionReturnType(functionDeclaration.FunctionDeclarator);
                }
            }
        }

        public void RegisterFunctionReturnType(MethodInfo method)
        {
            if (method is null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            _functionReturnTypes.Add(method.Name, method.ReturnType.ToJassType());
        }

        public void RegisterGlobalVariableType(FieldInfo field)
        {
            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            _globalTypes.Add(field.Name, field.FieldType.ToJassType());
        }

        internal void ClearLocalTypes()
        {
            _localTypes.Clear();
        }

        internal void RegisterFunctionReturnType(JassFunctionDeclaratorSyntax functionDeclarator)
        {
            _functionReturnTypes.Add(functionDeclarator.IdentifierName.Name, functionDeclarator.ReturnType);
        }

        private void RegisterVariableType(IVariableDeclaratorSyntax declarator, bool isLocalDeclaration)
        {
            switch (declarator)
            {
                case JassArrayDeclaratorSyntax arrayDeclarator: (isLocalDeclaration ? _localTypes : _globalTypes).Add(arrayDeclarator.IdentifierName.Name, arrayDeclarator.Type); break;
                case JassVariableDeclaratorSyntax variableDeclarator: (isLocalDeclaration ? _localTypes : _globalTypes).Add(variableDeclarator.IdentifierName.Name, variableDeclarator.Type); break;
            }
        }

        private void RegisterLocalVariableType(JassParameterSyntax parameter)
        {
            _localTypes.Add(parameter.IdentifierName.Name, parameter.Type);
        }

        private JassTypeSyntax GetFunctionReturnType(JassIdentifierNameSyntax functionName)
        {
            return _functionReturnTypes.TryGetValue(functionName.Name, out var type)
                ? type
                : throw new KeyNotFoundException($"Function '{functionName}' could not be found.");
        }

        private JassTypeSyntax GetVariableType(JassIdentifierNameSyntax variableName)
        {
            return (_localTypes.TryGetValue(variableName.Name, out var type) || _globalTypes.TryGetValue(variableName.Name, out type))
                ? type
                : throw new KeyNotFoundException($"Variable '{variableName}' could not be found.");
        }
    }
}