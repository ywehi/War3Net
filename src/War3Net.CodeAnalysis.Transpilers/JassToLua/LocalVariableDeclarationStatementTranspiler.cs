﻿// ------------------------------------------------------------------------------
// <copyright file="LocalVariableDeclarationStatementTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using CSharpLua.LuaAst;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        public LuaStatementSyntax Transpile(JassLocalVariableDeclarationStatementSyntax localVariableDeclarationStatement)
        {
            return Transpile(localVariableDeclarationStatement.Declarator, true);
        }
    }
}