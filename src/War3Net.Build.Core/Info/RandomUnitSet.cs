﻿// ------------------------------------------------------------------------------
// <copyright file="RandomUnitSet.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Linq;

using War3Net.Common.Extensions;

namespace War3Net.Build.Info
{
    public sealed partial class RandomUnitSet
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RandomUnitSet"/> class.
        /// </summary>
        public RandomUnitSet()
        {
        }

        public int Chance { get; set; }

        public int[] UnitIds { get; set; }

        public override string ToString() => $"{string.Join(", ", UnitIds.Select(id => id.ToRawcode()))} ({Chance}%)";
    }
}