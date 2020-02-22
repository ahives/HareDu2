// Copyright 2013-2020 Albert L. Hives
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
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public interface ServerInfo
    {
        [JsonProperty("rabbit_version")]
        string RabbitMqVersion { get; }
        
        [JsonProperty("users")]
        IList<UserInfo> Users { get; }
        
        [JsonProperty("vhosts")]
        IList<VirtualHostInfo> VirtualHosts { get; }
        
        [JsonProperty("permissions")]
        IList<UserPermissionsInfo> Permissions { get; }
        
        [JsonProperty("policies")]
        IList<PolicyInfo> Policies { get; }
        
        [JsonProperty("parameters")]
        IList<ScopedParameterInfo> Parameters { get; }
        
        [JsonProperty("global_parameters")]
        IList<GlobalParameterInfo> GlobalParameters { get; }
        
        [JsonProperty("queues")]
        IList<QueueInfo> Queues { get; }
        
        [JsonProperty("exchanges")]
        IList<ExchangeInfo> Exchanges { get; }
        
        [JsonProperty("bindings")]
        IList<BindingInfo> Bindings { get; }
        
        [JsonProperty("topic_permissions")]
        IList<TopicPermissionsInfo> TopicPermissions { get; }
    }
}