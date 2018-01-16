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
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Configuration;
    using Exceptions;
    using Model;

    internal class BindingImpl :
        ResourceBase,
        Binding
    {
        public BindingImpl(HttpClient client, HareDuClientSettings settings)
            : base(client, settings)
        {
        }

        public async Task<Result<IEnumerable<BindingInfo>>> GetAll(CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled(LogInfo);

            string url = $"api/bindings";
            var result = await Get<IEnumerable<BindingInfo>>(url, cancellationToken);

            return result;
        }

        public async Task<Result<BindingInfo>> Create(Action<BindingCreateAction> action, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled(LogInfo);

            var impl = new BindingCreateActionImpl();
            action(impl);
            
            if (impl.Errors.Value.Any())
                return Result.None<BindingInfo>(errors: impl.Errors.Value);

            DefineBinding definition = impl.Definition.Value;

            Debug.Assert(definition != null);

            string source = impl.Source.Value;
            string destination = impl.Destination.Value;
            BindingType bindingType = impl.BindingType.Value;
            string vhost = impl.VirtualHost.Value;
            
            if (string.IsNullOrWhiteSpace(source))
                return Result.None<BindingInfo>(errors: new List<Error>{ new ErrorImpl("The name of the binding (queue/exchange) source is missing.") });

            if (string.IsNullOrWhiteSpace(destination))
                return Result.None<BindingInfo>(errors: new List<Error>{ new ErrorImpl("The name of the destination binding (queue/exchange) is missing.") });
            
            if (string.IsNullOrWhiteSpace(vhost))
                return Result.None<BindingInfo>(errors: new List<Error>{ new ErrorImpl("The name of the virtual host is missing.") });
            
            string sanitizedVHost = vhost.SanitizeVirtualHostName();

            string url = bindingType == BindingType.Exchange
                ? $"api/bindings/{sanitizedVHost}/e/{source}/e/{destination}"
                : $"api/bindings/{sanitizedVHost}/e/{source}/q/{destination}";

            var result = await PerformHttpPost<DefineBinding, BindingInfo>(url, definition, cancellationToken);

            return result;
        }

        public async Task<Result<BindingInfo>> Delete(Action<BindingDeleteAction> action, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled(LogInfo);

            var impl = new BindingDeleteActionImpl();
            action(impl);

            string destination = impl.BindingDestination.Value;
            string vhost = impl.VirtualHost.Value;
            string bindingName = impl.BindingName.Value;
            string source = impl.BindingSource.Value;
            BindingType bindingType = impl.BindingType.Value;
            
            if (string.IsNullOrWhiteSpace(destination))
                return Result.None<BindingInfo>(errors: new List<Error>{ new ErrorImpl("The name of the destination binding (queue/exchange) is missing.") });

            if (string.IsNullOrWhiteSpace(vhost))
                return Result.None<BindingInfo>(errors: new List<Error>{ new ErrorImpl("The name of the virtual host is missing.") });

            if (string.IsNullOrWhiteSpace(bindingName))
                return Result.None<BindingInfo>(errors: new List<Error>{ new ErrorImpl("The name of the binding is missing.") });
            
            string sanitizedVHost = vhost.SanitizeVirtualHostName();

            string url = bindingType == BindingType.Queue
                ? $"api/bindings/{sanitizedVHost}/e/{source}/q/{destination}/{bindingName}"
                : $"api/bindings/{sanitizedVHost}/e/{source}/e/{destination}/{bindingName}";

            var result = await Delete<BindingInfo>(url, cancellationToken);

            return result;
        }

        
        class BindingDeleteActionImpl :
            BindingDeleteAction
        {
            static BindingType _bindingType;
            static string _bindingName;
            static string _bindingSource;
            static string _bindingDestination;
            static string _vhost;
            
            public Lazy<string> VirtualHost { get; }
            public Lazy<BindingType> BindingType { get; }
            public Lazy<string> BindingName { get; }
            public Lazy<string> BindingSource { get; }
            public Lazy<string> BindingDestination { get; }

            public BindingDeleteActionImpl()
            {
                BindingType = new Lazy<BindingType>(() => _bindingType, LazyThreadSafetyMode.PublicationOnly);
                BindingName = new Lazy<string>(() => _bindingName, LazyThreadSafetyMode.PublicationOnly);
                BindingSource = new Lazy<string>(() => _bindingSource, LazyThreadSafetyMode.PublicationOnly);
                BindingDestination = new Lazy<string>(() => _bindingDestination, LazyThreadSafetyMode.PublicationOnly);
                VirtualHost = new Lazy<string>(() => _vhost, LazyThreadSafetyMode.PublicationOnly);
            }

            public void Binding(Action<BindingDeleteDefinition> definition)
            {
                var impl = new BindingDeleteDefinitionImpl();
                definition(impl);

                _bindingType = impl.BindingType;
                _bindingName = impl.BindingName;
                _bindingSource = impl.BindingSource;
                _bindingDestination = impl.DestinationSource;
            }

            public void Targeting(Action<BindingTarget> target)
            {
                var impl = new BindingTargetImpl();
                target(impl);

                _vhost = impl.VirtualHostName;
            }

            
            class BindingTargetImpl :
                BindingTarget
            {
                public string VirtualHostName { get; private set; }

                public void VirtualHost(string name) => VirtualHostName = name;
            }


            class BindingDeleteDefinitionImpl :
                BindingDeleteDefinition
            {
                public string BindingName { get; private set; }
                public string BindingSource { get; private set; }
                public string DestinationSource { get; private set; }
                public BindingType BindingType { get; private set; }

                public void Name(string name) => BindingName = name;

                public void Source(string binding) => BindingSource = binding;

                public void Destination(string binding) => DestinationSource = binding;

                public void Type(BindingType bindingType) => BindingType = bindingType;
            }
        }


        class BindingCreateActionImpl :
            BindingCreateAction
        {
            static string _routingKey;
            static IDictionary<string, ArgumentValue<object>> _arguments;
            static string _vhost;
            static string _source;
            static string _destination;
            static BindingType _bindingType;

            public Lazy<DefineBinding> Definition { get; }
            public Lazy<string> Source { get; }
            public Lazy<string> Destination { get; }
            public Lazy<string> VirtualHost { get; }
            public Lazy<BindingType> BindingType { get; }
            public Lazy<List<Error>> Errors { get; }

            public BindingCreateActionImpl()
            {
                Errors = new Lazy<List<Error>>(() => _arguments.Select(x => x.Value.Error).ToList(), LazyThreadSafetyMode.PublicationOnly);
                Definition = new Lazy<DefineBinding>(() => new DefineBindingImpl(_routingKey, _arguments), LazyThreadSafetyMode.PublicationOnly);
                Source = new Lazy<string>(() => _source, LazyThreadSafetyMode.PublicationOnly);
                Destination = new Lazy<string>(() => _destination, LazyThreadSafetyMode.PublicationOnly);
                VirtualHost = new Lazy<string>(() => _vhost, LazyThreadSafetyMode.PublicationOnly);
                BindingType = new Lazy<BindingType>(() => _bindingType, LazyThreadSafetyMode.PublicationOnly);
            }

            public void Binding(Action<BindingCreateDefinition> definition)
            {
                var impl = new BindingCreateDefinitionImpl();
                definition(impl);
                
                _source = impl.SourceBinding;
                _destination = impl.DestinationBinding;
                _bindingType = impl.BindingType;
            }

            public void Configure(Action<BindingConfiguration> configuration)
            {
                var impl = new BindingConfigurationImpl();
                configuration(impl);

                _arguments = impl.Arguments;
                _routingKey = impl.RoutingKey;
            }

            public void Targeting(Action<BindingTarget> target)
            {
                var impl = new BindingTargetImpl();
                target(impl);

                _vhost = impl.VirtualHostName;
            }

            
            class BindingTargetImpl :
                BindingTarget
            {
                public string VirtualHostName { get; private set; }

                public void VirtualHost(string name) => VirtualHostName = name;
            }


            class BindingCreateDefinitionImpl :
                BindingCreateDefinition
            {
                public string SourceBinding { get; private set; }
                public string DestinationBinding { get; private set; }
                public BindingType BindingType { get; private set; }

                public void Source(string binding) => SourceBinding = binding;

                public void Destination(string binding) => DestinationBinding = binding;

                public void Type(BindingType bindingType) => BindingType = bindingType;
            }


            class BindingConfigurationImpl :
                BindingConfiguration
            {
                public string RoutingKey { get; private set; }
                public IDictionary<string, ArgumentValue<object>> Arguments { get; private set; }

                public void HasRoutingKey(string routingKey) => RoutingKey = routingKey;

                public void HasArguments(Action<BindingArguments> arguments)
                {
                    var impl = new BindingArgumentsImpl();
                    arguments(impl);

                    Arguments = impl.Arguments;
                }
            }

            
            class BindingArgumentsImpl :
                BindingArguments
            {
                public IDictionary<string, ArgumentValue<object>> Arguments { get; } = new Dictionary<string, ArgumentValue<object>>();

                public void Set<T>(string arg, T value)
                {
                    Arguments.Add(arg.Trim(),
                        Arguments.ContainsKey(arg)
                            ? new ArgumentValue<object>(value, $"Argument '{arg}' has already been set")
                            : new ArgumentValue<object>(value));
                }
            }


            class DefineBindingImpl :
                DefineBinding
            {
                public DefineBindingImpl(string routingKey, IDictionary<string, ArgumentValue<object>> arguments)
                {
                    RoutingKey = routingKey;
                    Arguments = arguments.ToDictionary(x => x.Key, x => x.Value.Value);
                }

                public string RoutingKey { get; }
                public IDictionary<string, object> Arguments { get; }
            }
        }
    }
}