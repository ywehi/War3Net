﻿// ------------------------------------------------------------------------------
// <copyright file="JassFunctionCustomScriptAction.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassFunctionCustomScriptAction : IDeclarationLineSyntax
    {
        public JassFunctionCustomScriptAction(JassFunctionDeclaratorSyntax functionDeclarator)
        {
            FunctionDeclarator = functionDeclarator;
        }

        public JassFunctionDeclaratorSyntax FunctionDeclarator { get; init; }

        public bool Equals(IDeclarationLineSyntax? other)
        {
            return other is JassFunctionCustomScriptAction function
                && FunctionDeclarator.Equals(function.FunctionDeclarator);
        }

        public override string ToString() => $"{JassKeyword.Function} {FunctionDeclarator}";
    }
}