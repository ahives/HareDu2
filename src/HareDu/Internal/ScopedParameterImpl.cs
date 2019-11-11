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
namespace HareDu.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Core.Extensions;
    using Model;

    class ScopedParameterImpl :
        RmqBrokerClient,
        ScopedParameter
    {
        public ScopedParameterImpl(HttpClient client)
            : base(client)
        {
        }

        public async Task<ResultList<ScopedParameterInfo>> GetAll(CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            string url = $"api/parameters";
            
            ResultList<ScopedParameterInfo> result = await GetAll<ScopedParameterInfo>(url, cancellationToken);

            return result;
        }

        public async Task<Result> Create(Action<ScopedParameterCreateAction> action, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();
            
            var impl = new ScopedParameterCreateActionImpl();
            action(impl);

            ScopedParameterDefinition definition = impl.Definition.Value;

            Debug.Assert(definition != null);
                    
            string url = $"api/parameters/{definition.Component}/{definition.VirtualHost.SanitizeVirtualHostName()}/{definition.ParameterName}";

            if (impl.Errors.Value.Any())
                return new FaultedResult(impl.Errors.Value, new DebugInfoImpl(url, definition.ToJsonString()));

            Result result = await Put(url, definition, cancellationToken);

            return result;
        }

        public async Task<Result> Delete(Action<ScopedParameterDeleteAction> action, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            var impl = new ScopedParameterDeleteActionImpl();
            action(impl);

            string url = $"api/parameters/{impl.Component.Value}/{impl.VirtualHost.Value}/{impl.ScopedParameter.Value}";
            
            if (impl.Errors.Value.Any())
                return new FaultedResult(impl.Errors.Value, new DebugInfoImpl(url, null));

            Result result = await Delete(url, cancellationToken);

            return result;
        }

        
        class ScopedParameterDeleteActionImpl :
            ScopedParameterDeleteAction
        {
            string _vhost;
            string _component;
            string _scopedParameter;
            readonly List<Error> _errors;

            public Lazy<string> ScopedParameter { get; }
            public Lazy<string> Component { get; }
            public Lazy<string> VirtualHost { get; }
            public Lazy<List<Error>> Errors { get; }

            public ScopedParameterDeleteActionImpl()
            {
                _errors = new List<Error>();
                
                Errors = new Lazy<List<Error>>(() => _errors, LazyThreadSafetyMode.PublicationOnly);
                ScopedParameter = new Lazy<string>(() => _scopedParameter, LazyThreadSafetyMode.PublicationOnly);
                Component = new Lazy<string>(() => _component, LazyThreadSafetyMode.PublicationOnly);
                VirtualHost = new Lazy<string>(() => _vhost, LazyThreadSafetyMode.PublicationOnly);
            }

            public void Parameter(string name)
            {
                _scopedParameter = name;
            
                if (string.IsNullOrWhiteSpace(_scopedParameter))
                    _errors.Add(new ErrorImpl("The name of the parameter is missing."));
            }
            
            public void Targeting(Action<ScopedParameterTarget> target)
            {
                var impl = new ScopedParameterTargetImpl();
                target(impl);

                _vhost = impl.VirtualHostName;
                _component = impl.ComponentName;
            }

            public void Verify()
            {
                
            }

            
            class ScopedParameterTargetImpl :
                ScopedParameterTarget
            {
                public string ComponentName { get; private set; }
                public string VirtualHostName { get; private set; }

                public void Component(string component) => ComponentName = component;

                public void VirtualHost(string name) => VirtualHostName = name;
            }
        }

        
        class ScopedParameterCreateActionImpl :
            ScopedParameterCreateAction
        {
            string _component;
            string _vhost;
            string _value;
            string _name;
            readonly List<Error> _errors;

            public Lazy<ScopedParameterDefinition> Definition { get; }
            public Lazy<List<Error>> Errors { get; }

            public ScopedParameterCreateActionImpl()
            {
                _errors = new List<Error>();
                
                Errors = new Lazy<List<Error>>(() => _errors, LazyThreadSafetyMode.PublicationOnly);
                Definition = new Lazy<ScopedParameterDefinition>(
                    () => new ScopedParameterDefinitionImpl(_vhost, _component, _name, _value), LazyThreadSafetyMode.PublicationOnly);
            }

            public void Parameter(string name, string value)
            {
                _name = name;
                _value = value;

                if (string.IsNullOrWhiteSpace(_name))
                    _errors.Add(new ErrorImpl("The name of the parameter is missing."));
            }
            
            public void Targeting(Action<ScopedParameterTarget> target)
            {
                var impl = new ScopedParameterTargetImpl();
                target(impl);

                _vhost = impl.VirtualHostName;
                _component = impl.ComponentName;

                if (string.IsNullOrWhiteSpace(_vhost))
                    _errors.Add(new ErrorImpl("The name of the virtual host is missing."));

                if (string.IsNullOrWhiteSpace(_component))
                    _errors.Add(new ErrorImpl("The component name is missing."));
            }

            public void Verify()
            {
                
            }

            
            class ScopedParameterTargetImpl :
                ScopedParameterTarget
            {
                public string ComponentName { get; private set; }
                public string VirtualHostName { get; private set; }

                public void Component(string component) => ComponentName = component;

                public void VirtualHost(string name) => VirtualHostName = name;
            }

            
            class ScopedParameterDefinitionImpl :
                ScopedParameterDefinition
            {
                public ScopedParameterDefinitionImpl(string virtualHost, string component, string name, string value)
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