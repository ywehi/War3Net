﻿// ------------------------------------------------------------------------------
// <copyright file="CampaignAbilityObjectData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

namespace War3Net.Build.Object
{
    public sealed partial class CampaignAbilityObjectData : AbilityObjectData
    {
        internal CampaignAbilityObjectData(BinaryReader reader)
            : base(reader)
        {
        }
    }
}