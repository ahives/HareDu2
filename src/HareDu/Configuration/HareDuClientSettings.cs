// Copyright 2013-2017 Albert L. Hives
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
namespace HareDu.Configuration
{
    using System;
    using Common.Logging;
    using Newtonsoft.Json;

    public interface HareDuClientSettings
    {
        [JsonProperty("rmqServerUrl")]
        string RabbitMqServerUrl { get; }
        
        [JsonProperty("timeout")]
        TimeSpan Timeout { get; }
        
        [JsonProperty("logger")]
        HareDuLoggerSettings LoggerSettings { get; }
        
        [JsonProperty("credentials")]
        HareDuCredentials Credentials { get; }
        
        [JsonProperty("transientRetry")]
        HareDuTransientRetrySettings TransientRetrySettings { get; }
    }

    public interface HareDuLoggerSettings
    {
        [JsonProperty("enable")]
        bool Enable { get; }
        
        [JsonProperty("name")]
        string Name { get; }
        
        [JsonIgnore]
        ILog Logger { get; }
    }

    public interface HareDuTransientRetrySettings
    {
        [JsonProperty("enable")]
        bool Enable { get; }
        
        [JsonProperty("limit")]
        int RetryLimit { get; }
    }
}