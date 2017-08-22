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

            HttpResponseMessage response = await HttpGet(url, cancellationToken);
            Result<IEnumerable<ScopedParameterInfo>> result = await response.GetResponse<IEnumerable<ScopedParameterInfo>>();

            LogInfo($"Sent request to return all parameter information on current RabbitMQ server.");

            return result;
        }

        public async Task<Result> Create(Action<ScopedParameterCreateAction> action, CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);
            
            var impl = new ScopedParameterCreateActionImpl();
            action(impl);

            ScopedParameterSettings settings = impl.Settings.Value;

            if (string.IsNullOrWhiteSpace(settings.ParameterName))
                throw new ParameterMissingException("The name of the parameter is missing.");
                    
            string url = $"api/parameters/{settings.Component}/{settings.VirtualHost.SanitizeVirtualHostName()}/{settings.ParameterName}";

            HttpResponseMessage response = await HttpPut(url, settings, cancellationToken);
            Result result = response.GetResponse();

            LogInfo($"Sent request to RabbitMQ server to create a scoped parameter '{settings.ParameterName}'.");

            return result;
        }

        public async Task<Result> Delete(Action<ScopedParameterDeleteAction> action, CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);

            var impl = new ScopedParameterDeleteActionImpl();
            action(impl);
            
            if (string.IsNullOrWhiteSpace(impl.ScopedParameter))
                throw new ParameterMissingException("The name of the parameter is missing.");

            string url = $"api/parameters/{impl.Component}/{impl.VirtualHost}/{impl.ScopedParameter}";

            HttpResponseMessage response = await HttpDelete(url, cancellationToken);
            Result result = response.GetResponse();

            LogInfo($"Sent request to RabbitMQ server to delete a global parameter '{impl.ScopedParameter}'.");

            return result;
        }

        
        class ScopedParameterDeleteActionImpl :
            ScopedParameterDeleteAction
        {
            public string ScopedParameter { get; private set; }
            public string Component { get; private set; }
            public string VirtualHost { get; private set; }

            public void Parameter(string name) => ScopedParameter = name;

            public void OnComponent(string component) => Component = component;

            public void OnVirtualHost(string vhost) => VirtualHost = vhost;
        }

        
        class ScopedParameterCreateActionImpl :
            ScopedParameterCreateAction
        {
            static string _component;
            static string _vhost;
            static string _value;
            static string _name;
            
            public Lazy<ScopedParameterSettings> Settings { get; }

            public ScopedParameterCreateActionImpl() => Settings = new Lazy<ScopedParameterSettings>(Init, LazyThreadSafetyMode.PublicationOnly);

            ScopedParameterSettings Init() => new ScopedParameterSettingsImpl(_vhost, _component, _name, _value);

            public void OnComponent(string component) => _component = component;

            public void OnVirtualHost(string vhost) => _vhost = vhost;
            
            public void Parameter(string name, string value)
            {
                _name = name;
                _value = value;
            }

            
            class ScopedParameterSettingsImpl :
                ScopedParameterSettings
            {
                public ScopedParameterSettingsImpl(string virtualHost, string component, string name, string value)
                {
                    VirtualHost = virtualHost;
                    Component = component;
                    ParameterName = name;
                    ParameterValue = value;
                }

                public string VirtualHost { get; }
                public string Component { get; }
                public string ParameterName { get; }
                public string ParameterValue { get; }
            }
        }
    }
}