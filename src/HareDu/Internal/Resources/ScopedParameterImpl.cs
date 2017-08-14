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
namespace HareDu.Internal.Resources
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Configuration;
    using Exceptions;
    using Model;

    internal class ScopedParameterImpl :
        ResourceBase,
        ScopedParameter
    {
        public ScopedParameterImpl(HttpClient client, HareDuClientSettings settings)
            : base(client, settings)
        {
        }

        public async Task<Result<IEnumerable<ScopedParameterInfo>>> GetAll(CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);

            string url = $"api/parameters";

            LogInfo($"Sent request to return all parameter information on current RabbitMQ server.");

            HttpResponseMessage response = await HttpGet(url, cancellationToken);
            Result<IEnumerable<ScopedParameterInfo>> result = await response.GetResponse<IEnumerable<ScopedParameterInfo>>();

            return result;
        }

        public async Task<Result> Create(Action<ScopedParameterDefinition> definition, CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);
            
            var impl = new ScopedParameterDefinitionImpl();
            definition(impl);

            ScopedParameterDescription desc = impl.ParameterDescription.Value;

            string url = $"api/parameters/{desc.Component}/{desc.VirtualHost.SanitizeVirtualHostName()}/{desc.Name}";

            LogInfo($"Sent request to RabbitMQ server to create a scoped parameter '{desc.Name}'.");

            HttpResponseMessage response = await HttpPut(url, desc, cancellationToken);
            Result result = response.GetResponse();

            return result;
        }

        public async Task<Result> Delete(string component, string name, string vhost, CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);

            if (string.IsNullOrWhiteSpace(name))
                throw new ParameterMissingException("The name of the parameter is missing.");

            string url = $"api/parameters/{component}/{vhost}/{name}";

            LogInfo($"Sent request to RabbitMQ server to delete a global parameter '{name}'.");

            HttpResponseMessage response = await HttpDelete(url, cancellationToken);
            Result result = response.GetResponse();

            return result;
        }

        
        class ScopedParameterDefinitionImpl :
            ScopedParameterDefinition
        {
            static string _component;
            static string _vhost;
            static string _value;
            static string _name;
            
            public Lazy<ScopedParameterDescription> ParameterDescription { get; }

            public ScopedParameterDefinitionImpl() => ParameterDescription = new Lazy<ScopedParameterDescription>(Init, LazyThreadSafetyMode.PublicationOnly);

            ScopedParameterDescription Init() => new ScopedParameterDescriptionImpl(_vhost, _component, _name, _value);

            public void OnComponent(string component) => _component = component;

            public void OnVirtualHost(string vhost) => _vhost = vhost;
            
            public void SetParameter(string name, string value)
            {
                _name = name;
                _value = value;
            }

            
            class ScopedParameterDescriptionImpl :
                ScopedParameterDescription
            {
                public ScopedParameterDescriptionImpl(string virtualHost, string component, string name, string value)
                {
                    if (string.IsNullOrWhiteSpace(name))
                        throw new ParameterMissingException("The name of the parameter is missing.");
                    
                    VirtualHost = virtualHost;
                    Component = component;
                    Name = name;
                    Value = value;
                }

                public string VirtualHost { get; }
                public string Component { get; }
                public string Name { get; }
                public string Value { get; }
            }
        }
    }
}