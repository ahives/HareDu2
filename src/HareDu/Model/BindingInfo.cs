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
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public interface BindingInfo
    {
        /// <summary>
        /// Name of the source exchange.
        /// </summary>
        [JsonProperty("source")]
        string Source { get; }
        
        /// <summary>
        /// Name of the RabbitMQ virtual host object.
        /// </summary>
        [JsonProperty("vhost")]
        string VirtualHost { get; }
        
        /// <summary>
        /// Name of the destination exchange/queue object.
        /// </summary>
        [JsonProperty("destination")]
        string Destination { get; }
        
        /// <summary>
        /// Qualifies the destination object by defining the type of object (e.g., queue, exchange, etc.).
        /// </summary>
        [JsonProperty("destination_type")]
        string DestinationType { get; }
        
        [JsonProperty("routing_key")]
        string RoutingKey { get; }
        
        [JsonProperty("arguments")]
        IDictionary<string, object> Arguments { get; }
        
        [JsonProperty("properties_key")]
        string PropertiesKey { get; }
    }
}