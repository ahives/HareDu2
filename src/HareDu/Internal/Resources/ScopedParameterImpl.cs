// Copyright 2013-2018 Albert L. Hives
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
    using System.Diagnostics;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Exceptions;
    using Model;

    internal class ScopedParameterImpl :
        ResourceBase,
        ScopedParameter
    {
        public ScopedParameterImpl(HttpClient client)
            : base(client)
        {
        }

        public async Task<Result<IEnumerable<ScopedParameterInfo>>> GetAll(CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            string url = $"api/parameters";

            var result = await Get<IEnumerable<ScopedParameterInfo>>(url, cancellationToken);

            return result;
        }

        public async Task<Result<ScopedParameterInfo>> Create(Action<ScopedParameterCreateAction> action, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();
            
            var impl = new ScopedParameterCreateActionImpl();
            action(impl);

            DefinedScopedParameter definition = impl.Definition.Value;

            Debug.Assert(definition != null);

            if (string.IsNullOrWhiteSpace(definition.ParameterName))
                return Result.None<ScopedParameterInfo>(errors: new List<Error>{ new ErrorImpl("The name of the parameter is missing.") });
                    
            string url = $"api/parameters/{definition.Component}/{definition.VirtualHost.SanitizeVirtualHostName()}/{definition.ParameterName}";

            var result = await Put<DefinedScopedParameter, ScopedParameterInfo>(url, definition, cancellationToken);

            return result;
        }

        public async Task<Result<ScopedParameterInfo>> Delete(Action<ScopedParameterDeleteAction> action, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            var impl = new ScopedParameterDeleteActionImpl();
            action(impl);

            string scopedParameter = impl.ScopedParameter.Value;
            string virtualHost = impl.VirtualHost.Value;
            string component = impl.Component.Value;
            
            if (string.IsNullOrWhiteSpace(scopedParameter))
                return Result.None<ScopedParameterInfo>(errors: new List<Error>{ new ErrorImpl("The name of the parameter is missing.") });

            string url = $"api/parameters/{component}/{virtualHost}/{scopedParameter}";

            var result = await Delete<ScopedParameterInfo>(url, cancellationToken);

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
            
            public void Targeting(Action<ScopedParameterTarget> target)
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

                public void VirtualHost(string name) => VirtualHostName = name;
            }
        }

        
        class ScopedParameterCreateActionImpl :
            ScopedParameterCreateAction
        {
            static string _component;
            static string _vhost;
            static string _value;
            static string _name;
            
            public Lazy<DefinedScopedParameter> Definition { get; }

            public ScopedParameterCreateActionImpl()
            {
                Definition = new Lazy<DefinedScopedParameter>(
                    () => new DefinedScopedParameterImpl(_vhost, _component, _name, _value), LazyThreadSafetyMode.PublicationOnly);
            }

            public void Parameter(string name, string value)
            {
                _name = name;
                _value = value;
            }
            
            public void Targeting(Action<ScopedParameterTarget> target)
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

                public void VirtualHost(string name) => VirtualHostName = name;
            }

            
            class DefinedScopedParameterImpl :
                DefinedScopedParameter
            {
                public DefinedScopedParameterImpl(string virtualHost, string component, string name, string value)
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