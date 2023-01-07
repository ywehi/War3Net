﻿// ------------------------------------------------------------------------------
// <copyright file="AbilityObjectDataTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Object;

namespace War3Net.Build.Core.Tests.Object
{
    [TestClass]
    public class AbilityObjectDataTests
    {
        [DataTestMethod]
        [DynamicData(nameof(TestDataFileProvider.GetAbilityObjectDataFilePaths), typeof(TestDataFileProvider), DynamicDataSourceType.Method)]
        public void TestParseAbilityObjectData(string abilityObjectDataFilePath)
        {
            ParseTestHelper.RunBinaryRWTest(
                abilityObjectDataFilePath,
                typeof(AbilityObjectData));
        }
    }
}