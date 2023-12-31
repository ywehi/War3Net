﻿// ------------------------------------------------------------------------------
// <copyright file="GameConfigurationTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Configuration;

namespace War3Net.Build.Core.Tests.Configuration
{
    [TestClass]
    public class GameConfigurationTests
    {
        [DataTestMethod]
        [DynamicData(nameof(TestDataFileProvider.GetGameConfigurationFilePaths), typeof(TestDataFileProvider), DynamicDataSourceType.Method)]
        public void TestBinarySerialization(string filePath)
        {
            SerializationTestHelper<GameConfiguration>.RunBinaryRWTest(filePath);
        }

        [DataTestMethod]
        [DynamicData(nameof(TestDataFileProvider.GetGameConfigurationFilePaths), typeof(TestDataFileProvider), DynamicDataSourceType.Method)]
        public void TestJsonSerialization(string filePath)
        {
            SerializationTestHelper<GameConfiguration>.RunJsonRWTest(filePath, false);
        }

        [DataTestMethod]
        [DynamicData(nameof(TestDataFileProvider.GetGameConfigurationFilePaths), typeof(TestDataFileProvider), DynamicDataSourceType.Method)]
        public void TestJsonSerializationStringEnums(string filePath)
        {
            SerializationTestHelper<GameConfiguration>.RunJsonRWTest(filePath, true);
        }
    }
}