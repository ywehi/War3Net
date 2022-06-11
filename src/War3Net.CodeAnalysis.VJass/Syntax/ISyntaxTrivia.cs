﻿// ------------------------------------------------------------------------------
// <copyright file="ISyntaxTrivia.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public interface ISyntaxTrivia : IEquatable<ISyntaxTrivia>
    {
        void WriteTo(TextWriter writer);
    }
}