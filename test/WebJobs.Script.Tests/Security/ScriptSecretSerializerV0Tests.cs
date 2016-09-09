﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Script.WebHost;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Microsoft.Azure.WebJobs.Script.Tests.Security
{
    public class ScriptSecretSerializerV0Tests
    {
        [Fact]
        public void SerializeFunctionSecrets_ReturnsExpectedResult()
        {
            var serializer = new ScriptSecretSerializerV0();

            var secrets = new List<Key>
            {
                new Key
                {
                    Name = string.Empty,
                    Value = "Value1",
                    IsEncrypted = false,
                    EncryptionKeyId = "KeyId1"
                },
                new Key
                {
                    Name = "Key2",
                    Value = "Value2",
                    IsEncrypted = true,
                    EncryptionKeyId = "KeyId2"
                }
            };

            string serializedSecret = serializer.SerializeFunctionSecrets(secrets);

            Assert.NotNull(serializedSecret);

            var jsonObject = JObject.Parse(serializedSecret);
            var serializedSecretValue = jsonObject.Value<string>("key");

            Assert.Equal("Value1", serializedSecretValue);
        }

        [Fact]
        public void DeserializeFunctionSecrets_ReturnsExpectedResult()
        {
            var serializer = new ScriptSecretSerializerV0();
            var serializedSecret = "{ 'key': 'TestValue' }";

            var expected = new List<Key>
            {
                new Key
                {
                    Name = string.Empty,
                    Value = "TestValue",
                    IsEncrypted = false
                }
            };

            IList<Key> actual = serializer.DeserializeFunctionSecrets(JObject.Parse(serializedSecret));
            AssertKeyCollectionsEquality(expected, actual);
        }

        [Fact]
        public void DeserializeHostSecrets_ReturnsExpectedResult()
        {
            var serializer = new ScriptSecretSerializerV0();
            var serializedSecret = "{'masterKey': 'master', 'functionKey': 'master'}";
            var expected = new HostSecrets
            {
                MasterKey = new Key { Name = string.Empty, Value = "master" },
                FunctionKeys = new List<Key>
                {
                    new Key
                    {
                        Name = string.Empty,
                        Value = "master",
                        IsEncrypted = false
                    }
                }
            };

            HostSecrets actual = serializer.DeserializeHostSecrets(JObject.Parse(serializedSecret));

            Assert.NotNull(actual);
            Assert.Equal(expected.MasterKey, actual.MasterKey);
            AssertKeyCollectionsEquality(expected.FunctionKeys, actual.FunctionKeys);
        }

        [Fact]
        public void SerializeHostSecrets_ReturnsExpectedResult()
        {
            var serializer = new ScriptSecretSerializerV0();

            var secrets = new HostSecrets
            {
                MasterKey = new Key { Name = "master", Value = "mastervalue" },
                FunctionKeys = new List<Key>
                {
                    new Key
                    {
                        Name = string.Empty,
                        Value = "functionKeyValue",
                        IsEncrypted = false,
                        EncryptionKeyId = "KeyId1"
                    }
                }
            };

            string serializedSecret = serializer.SerializeHostSecrets(secrets);

            Assert.NotNull(serializedSecret);

            var jsonObject = JObject.Parse(serializedSecret);
            var functionKey = jsonObject.Value<string>("functionKey");
            var masterKey = jsonObject.Value<string>("masterKey");

            Assert.Equal("mastervalue", masterKey);
            Assert.Equal("functionKeyValue", functionKey);
        }

        private void AssertKeyCollectionsEquality(IList<Key> expected, IList<Key> actual)
        {
            Assert.NotNull(actual);
            Assert.Equal(expected.Count, actual.Count);
            Assert.True(expected.Zip(actual, (k1, k2) => k1.Equals(k2)).All(r => r));
        }
    }
}
