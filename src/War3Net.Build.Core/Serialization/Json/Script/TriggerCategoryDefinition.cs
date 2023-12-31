﻿// ------------------------------------------------------------------------------
// <copyright file="TriggerCategoryDefinition.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Text.Json;

using War3Net.Build.Extensions;
using War3Net.Common.Extensions;

namespace War3Net.Build.Script
{
    public sealed partial class TriggerCategoryDefinition : TriggerItem
    {
        internal TriggerCategoryDefinition(JsonElement jsonElement, TriggerItemType triggerItemType, MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion)
            : base(triggerItemType)
        {
            GetFrom(jsonElement, formatVersion, subVersion);
        }

        internal TriggerCategoryDefinition(ref Utf8JsonReader reader, TriggerItemType triggerItemType,MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion)
            : base(triggerItemType)
        {
            ReadFrom(ref reader, formatVersion, subVersion);
        }

        internal void GetFrom(JsonElement jsonElement, MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion)
        {
            Id = jsonElement.GetInt32(nameof(Id));
            Name = jsonElement.GetString(nameof(Name));
            if (formatVersion >= MapTriggersFormatVersion.v7)
            {
                IsComment = jsonElement.GetBoolean(nameof(IsComment));
            }

            if (subVersion is not null)
            {
                IsExpanded = jsonElement.GetBoolean(nameof(IsExpanded));
                ParentId = jsonElement.GetInt32(nameof(ParentId));
            }
        }

        internal void ReadFrom(ref Utf8JsonReader reader, MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion)
        {
            GetFrom(JsonDocument.ParseValue(ref reader).RootElement, formatVersion, subVersion);
        }

        internal override void WriteTo(Utf8JsonWriter writer, JsonSerializerOptions options, MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion)
        {
            writer.WriteStartObject();

            writer.WriteObject(nameof(Type), Type, options);

            writer.WriteNumber(nameof(Id), Id);
            writer.WriteString(nameof(Name), Name);
            if (formatVersion >= MapTriggersFormatVersion.v7)
            {
                writer.WriteBoolean(nameof(IsComment), IsComment);
            }

            if (subVersion is not null)
            {
                writer.WriteBoolean(nameof(IsExpanded), IsExpanded);
                writer.WriteNumber(nameof(ParentId), ParentId);
            }

            writer.WriteEndObject();
        }
    }
}