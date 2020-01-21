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
namespace HareDu.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Core.Extensions;
    using Model;

    class VirtualHostLimitsImpl :
        BaseBrokerObject,
        VirtualHostLimits
    {
        public VirtualHostLimitsImpl(HttpClient client)
            : base(client)
        {
        }

        public async Task<ResultList<VirtualHostLimitsInfo>> GetAll(CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            string url = "api/vhost-limits";
            
            ResultList<VirtualHostLimitsInfo> result = await GetAll<VirtualHostLimitsInfo>(url, cancellationToken);

            return result;
        }

        public async Task<Result> Define(Action<VirtualHostConfigureLimitsAction> action, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            var impl = new VirtualHostConfigureLimitsActionImpl();
            action(impl);

            impl.Validate();

            VirtualHostLimitsDefinition definition = impl.Definition.Value;

            string url = $"api/vhost-limits/vhost/{impl.VirtualHostName.Value.SanitizeVirtualHostName()}";

            if (impl.Errors.Value.Any())
                return new FaultedResult(impl.Errors.Value, new DebugInfoImpl(url, definition.ToJsonString()));

            Result result = await Put(url, definition, cancellationToken);

            return result;
        }

        public async Task<Result> Delete(Action<VirtualHostDeleteLimitsAction> action, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            var impl = new VirtualHostDeleteLimitsActionImpl();
            action(impl);

            impl.Validate();

            string url = $"api/vhost-limits/vhost/{impl.VirtualHostName.Value.SanitizeVirtualHostName()}";

            if (impl.Errors.Value.Any())
                return new FaultedResult(impl.Errors.Value, new DebugInfoImpl(url, null));

            Result result = await Delete(url, cancellationToken);

            return result;
        }


        class VirtualHostConfigureLimitsActionImpl :
            VirtualHostConfigureLimitsAction
        {
            string _vhost;
            ulong _maxConnectionLimits;
            ulong _maxQueueLimits;
            readonly List<Error> _errors;

            public Lazy<string> VirtualHostName { get; }
            public Lazy<List<Error>> Errors { get; }
            public Lazy<VirtualHostLimitsDefinition> Definition { get; }

            public VirtualHostConfigureLimitsActionImpl()
            {
                _errors = new List<Error>();
                
                Errors = new Lazy<List<Error>>(() => _errors, LazyThreadSafetyMode.PublicationOnly);
                VirtualHostName = new Lazy<string>(() => _vhost, LazyThreadSafetyMode.PublicationOnly);
                Definition = new Lazy<VirtualHostLimitsDefinition>(
                    () => new VirtualHostLimitsDefinitionImpl(_maxConnectionLimits, _maxQueueLimits), LazyThreadSafetyMode.PublicationOnly);
            }

            public void VirtualHost(string name) => _vhost = name;

            public void Configure(Action<VirtualHostLimitsConfiguration> configuration)
            {
                var impl = new VirtualHostLimitsConfigurationImpl();
                configuration(impl);

                _maxConnectionLimits = impl.MaxConnectionLimit.Value;
                _maxQueueLimits = impl.MaxQueueLimit.Value;
            }

            public void Validate()
            {
                if (string.IsNullOrWhiteSpace(_vhost))
                    _errors.Add(new ErrorImpl("The name of the virtual host is missing."));
                
                if (_maxConnectionLimits < 1)
                    _errors.Add(new ErrorImpl("Max connection limit value is missing."));
                
                if (_maxQueueLimits < 1)
                    _errors.Add(new ErrorImpl("Max queue limit value is missing."));
            }

            
            class VirtualHostLimitsDefinitionImpl :
                VirtualHostLimitsDefinition
            {
                public VirtualHostLimitsDefinitionImpl(ulong maxConnectionLimit, ulong maxQueueLimit)
                {
                    MaxConnectionLimit = maxConnectionLimit;
                    MaxQueueLimit = maxQueueLimit;
                }

                public ulong MaxConnectionLimit { get; }
                public ulong MaxQueueLimit { get; }
            }

            
            class VirtualHostLimitsConfigurationImpl :
                VirtualHostLimitsConfiguration
            {
                ulong _maxQueueLimits;
                ulong _maxConnectionLimits;
                
                public Lazy<ulong> MaxConnectionLimit { get; }
                public Lazy<ulong> MaxQueueLimit { get; }
                
                public VirtualHostLimitsConfigurationImpl()
                {
                    MaxConnectionLimit = new Lazy<ulong>(() => _maxConnectionLimits, LazyThreadSafetyMode.PublicationOnly);
                    MaxQueueLimit = new Lazy<ulong>(() => _maxQueueLimits, LazyThreadSafetyMode.PublicationOnly);
                }

                public void SetMaxConnectionLimit(ulong value) => _maxConnectionLimits = value;

                public void SetMaxQueueLimit(ulong value) => _maxQueueLimits = value;
            }
        }


        class VirtualHostDeleteLimitsActionImpl :
            VirtualHostDeleteLimitsAction
        {
            string _vhost;
            readonly List<Error> _errors;

            public Lazy<string> VirtualHostName { get; }
            public Lazy<List<Error>> Errors { get; }

            public VirtualHostDeleteLimitsActionImpl()
            {
                _errors = new List<Error>();
                
                Errors = new Lazy<List<Error>>(() => _errors, LazyThreadSafetyMode.PublicationOnly);
                VirtualHostName = new Lazy<string>(() => _vhost, LazyThreadSafetyMode.PublicationOnly);
            }

            public void For(string vhost) => _vhost = vhost;

            public void Validate()
            {
                if (string.IsNullOrWhiteSpace(_vhost))
                    _errors.Add(new ErrorImpl("The name of the virtual host is missing."));
            }
        }
    }
}