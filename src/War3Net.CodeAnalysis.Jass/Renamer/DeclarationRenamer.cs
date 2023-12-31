﻿// ------------------------------------------------------------------------------
// <copyright file="DeclarationRenamer.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenamer
    {
        private bool TryRenameDeclaration(ITopLevelDeclarationSyntax declaration, [NotNullWhen(true)] out ITopLevelDeclarationSyntax? renamedDeclaration)
        {
            return declaration switch
            {
                JassGlobalDeclarationListSyntax globalDeclarationList => TryRenameGlobalDeclarationList(globalDeclarationList, out renamedDeclaration),
                JassNativeFunctionDeclarationSyntax nativeFunctionDeclaration => TryRenameNativeFunctionDeclaration(nativeFunctionDeclaration, out renamedDeclaration),
                JassFunctionDeclarationSyntax functionDeclaration => TryRenameFunctionDeclaration(functionDeclaration, out renamedDeclaration),

                _ => TryRenameDummy(declaration, out renamedDeclaration),
            };
        }
    }
}