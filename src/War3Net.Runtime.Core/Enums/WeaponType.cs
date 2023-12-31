﻿// ------------------------------------------------------------------------------
// <copyright file="WeaponType.cs" company="Drake53">
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
    public sealed class WeaponType : Handle
    {
        private static readonly Dictionary<int, WeaponType> _types = GetTypes().ToDictionary(t => (int)t, t => new WeaponType(t));

        private readonly Type _type;

        private WeaponType(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            Undefined = 0,

            MetalLightChop = 1,
            MetalMediumChop = 2,
            MetalHeavyChop = 3,
            MetalLightSlice = 4,
            MetalMediumSlice = 5,
            MetalHeavySlice = 6,
            MetalMediumBash = 7,
            MetalHeavyBash = 8,
            MetalMediumStab = 9,
            MetalHeavyStab = 10,

            WoodLightSlice = 11,
            WoodMediumSlice = 12,
            WoodHeavySlice = 13,
            WoodLightBash = 14,
            WoodMediumBash = 15,
            WoodHeavyBash = 16,
            WoodLightStab = 17,
            WoodMediumStab = 18,

            ClawLightSlice = 19,
            ClawMediumSlice = 20,
            ClawHeavySlice = 21,

            AxeMediumChop = 22,

            RockHeavyBash = 23,
        }

        public static implicit operator Type(WeaponType weaponType) => weaponType._type;

        public static explicit operator int(WeaponType weaponType) => (int)weaponType._type;

        public static WeaponType GetWeaponType(int i)
        {
            if (!_types.TryGetValue(i, out var weaponType))
            {
                weaponType = new WeaponType((Type)i);
                _types.Add(i, weaponType);
            }

            return weaponType;
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