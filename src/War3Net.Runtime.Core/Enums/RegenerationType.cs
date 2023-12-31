﻿// ------------------------------------------------------------------------------
// <copyright file="RegenerationType.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using War3Net.Runtime.Core;

namespace War3Net.Runtime.Enums
{
    public sealed class RegenerationType : Handle
    {
        private static readonly Dictionary<int, RegenerationType> _types = GetTypes().ToDictionary(t => (int)t, t => new RegenerationType(t));

        private readonly Type _type;

        private RegenerationType(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            None = 0,
            Always = 1,
            Blight = 2,
            Day = 3,
            Night = 4,
        }

        public static implicit operator Type(RegenerationType regenerationType) => regenerationType._type;

        public static explicit operator int(RegenerationType regenerationType) => (int)regenerationType._type;

        public static RegenerationType GetRegenerationType(int i)
        {
            if (!_types.TryGetValue(i, out var regenerationType))
            {
                regenerationType = new RegenerationType((Type)i);
                _types.Add(i, regenerationType);
            }

            return regenerationType;
        }

        private static IEnumerable<Type> GetTypes()
        {
            foreach (Type type in Enum.GetValues(typeof(Type)))
            {
                yield return type;
            }
        }
    }
}