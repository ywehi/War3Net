﻿// ------------------------------------------------------------------------------
// <copyright file="BooleanSyntax.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public sealed class BooleanSyntax : SyntaxNode
    {
        private readonly TokenNode _token;

        public BooleanSyntax(TokenNode tokenNode)
            : base(tokenNode)
        {
            _token = tokenNode ?? throw new ArgumentNullException(nameof(tokenNode));
        }

        internal sealed class Parser : AlternativeParser
        {
            private static Parser _parser;

            internal static Parser Get => _parser ?? (_parser = new Parser()).Init();

            protected override SyntaxNode CreateNode(SyntaxNode node)
            {
                return new BooleanSyntax(node as TokenNode);
            }

            private Parser Init()
            {
                AddParser(TokenParser.Get(SyntaxTokenType.TrueKeyword));
                AddParser(TokenParser.Get(SyntaxTokenType.FalseKeyword));

                return this;
            }
        }
    }
}