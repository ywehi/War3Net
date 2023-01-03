﻿// ------------------------------------------------------------------------------
// <copyright file="MapAbilityObjectData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Text.Json;
using System.Text.Json.Serialization;

using War3Net.Build.Serialization.Json;

namespace War3Net.Build.Object
{
    [JsonConverter(typeof(JsonMapAbilityObjectDataConverter))]
    public sealed partial class MapAbilityObjectData : AbilityObjectData
    {
        internal MapAbilityObjectData(JsonElement jsonElement)
            : base(jsonElement)
        {
        }

        internal MapAbilityObjectData(ref Utf8JsonReader reader)
            : base(ref reader)
        {
        }
    }
}