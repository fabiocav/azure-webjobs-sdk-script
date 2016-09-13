﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Microsoft.Azure.WebJobs.Script.WebHost
{
    public interface IScriptSecretSerializer
    {
        int SupportedFormatVersion { get; }

        IList<Key> DeserializeFunctionSecrets(JObject secrets);

        string SerializeFunctionSecrets(IList<Key> secrets);

        HostSecrets DeserializeHostSecrets(JObject secrets);

        string SerializeHostSecrets(HostSecrets secrets);
    }
}
