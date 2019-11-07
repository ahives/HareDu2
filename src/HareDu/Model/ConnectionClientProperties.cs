// Copyright 2013-2019 Albert L. Hives
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
namespace HareDu.Model
{
    using System;
    using Newtonsoft.Json;

    public interface ConnectionClientProperties
    {
        [JsonProperty("assembly")]
        string Assembly { get; }

        [JsonProperty("assembly_version")]
        string AssemblyVersion { get; }

        [JsonProperty("capabilities")]
        ConnectionCapabilities Capabilities { get; }

        [JsonProperty("client_api")]
        string ClientApi { get; }

        [JsonProperty("connected")]
        DateTimeOffset Connected { get; }

        [JsonProperty("connection_name")]
        string ConnectionName { get; }

        [JsonProperty("copyright")]
        string Copyright { get; }

        [JsonProperty("hostname")]
        string Host { get; }

        [JsonProperty("information")]
        string Information { get; }

        [JsonProperty("platform")]
        string Platform { get; }

        [JsonProperty("process_id")]
        string ProcessId { get; }

        [JsonProperty("process_name")]
        string ProcessName { get; }

        [JsonProperty("product")]
        string Product { get; }

        [JsonProperty("version")]
        string Version { get; }
    }
}