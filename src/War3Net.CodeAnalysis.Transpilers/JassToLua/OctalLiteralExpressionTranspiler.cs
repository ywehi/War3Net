﻿// ------------------------------------------------------------------------------
// <copyright file="OctalLiteralExpressionTranspiler.cs" company="Drake53">
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
        public LuaExpressionSyntax Transpile(JassOctalLiteralExpressionSyntax octalLiteralExpression, out JassTypeSyntax type)
        {
            type = JassTypeSyntax.Integer;

            return octalLiteralExpression.Value;
        }
    }
}