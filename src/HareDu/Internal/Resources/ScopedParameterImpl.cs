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

            string scopedParameter = impl.ScopedParameter.Value;
            string virtualHost = impl.VirtualHost.Value;
            string component = impl.Component.Value;
            
            if (string.IsNullOrWhiteSpace(scopedParameter))
                throw new ParameterMissingException("The name of the parameter is missing.");

            string url = $"api/parameters/{component}/{virtualHost}/{scopedParameter}";

            HttpResponseMessage response = await HttpDelete(url, cancellationToken);
            Result result = response.GetResponse();

            LogInfo($"Sent request to RabbitMQ server to delete a global parameter '{scopedParameter}'.");

            return result;
        }

        
        class ScopedParameterDeleteActionImpl :
            ScopedParameterDeleteAction
        {
            static string _vhost;
            static string _component;
            static string _scopedParameter;
            
            public Lazy<string> ScopedParameter { get; }
            public Lazy<string> Component { get; }
            public Lazy<string> VirtualHost { get; }

            public ScopedParameterDeleteActionImpl()
            {
                ScopedParameter = new Lazy<string>(() => _scopedParameter, LazyThreadSafetyMode.PublicationOnly);
                Component = new Lazy<string>(() => _component, LazyThreadSafetyMode.PublicationOnly);
                VirtualHost = new Lazy<string>(() => _vhost, LazyThreadSafetyMode.PublicationOnly);
            }

            public void Parameter(string name) => _scopedParameter = name;
            
            public void Target(Action<ScopedParameterTarget> target)
            {
                var impl = new ScopedParameterTargetImpl();
                target(impl);

                _vhost = impl.VirtualHostName;
                _component = impl.ComponentName;
            }

            
            class ScopedParameterTargetImpl :
                ScopedParameterTarget
            {
                public string ComponentName { get; private set; }
                public string VirtualHostName { get; private set; }

                public void Component(string component) => ComponentName = component;

                public void VirtualHost(string vhost) => VirtualHostName = vhost;
            }
        }

        
        class ScopedParameterCreateActionImpl :
            ScopedParameterCreateAction
        {
            static string _component;
            static string _vhost;
            static string _value;
            static string _name;
            
            public Lazy<ScopedParameterSettings> Settings { get; }

            public ScopedParameterCreateActionImpl() => Settings = new Lazy<ScopedParameterSettings>(
                () => new ScopedParameterSettingsImpl(_vhost, _component, _name, _value), LazyThreadSafetyMode.PublicationOnly);

            public void Parameter(string name, string value)
            {
                _name = name;
                _value = value;
            }
            
            public void Target(Action<ScopedParameterTarget> target)
            {
                var impl = new ScopedParameterTargetImpl();
                target(impl);

                _vhost = impl.VirtualHostName;
                _component = impl.ComponentName;
            }

            
            class ScopedParameterTargetImpl :
                ScopedParameterTarget
            {
                public string ComponentName { get; private set; }
                public string VirtualHostName { get; private set; }

                public void Component(string component) => ComponentName = component;

                public void VirtualHost(string vhost) => VirtualHostName = vhost;
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