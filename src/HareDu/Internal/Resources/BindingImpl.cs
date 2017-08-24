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

    internal class BindingImpl :
        ResourceBase,
        Binding
    {
        public BindingImpl(HttpClient client, HareDuClientSettings settings)
            : base(client, settings)
        {
        }

        public async Task<Result<IEnumerable<BindingInfo>>> GetAll(CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);

            string url = $"api/bindings";

            HttpResponseMessage response = await HttpGet(url, cancellationToken);
            Result<IEnumerable<BindingInfo>> result = await response.GetResponse<IEnumerable<BindingInfo>>();

            LogInfo($"Sent request to return all binding information corresponding on current RabbitMQ server.");

            return result;
        }

        public async Task<Result> Create(Action<BindingCreateAction> action, CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);

            var impl = new BindingCreateActionImpl();
            action(impl);

            BindingCreateSettings settings = impl.Settings.Value;

            if (string.IsNullOrWhiteSpace(settings.Source))
                throw new BindingException("The name of the binding source is missing.");

            if (string.IsNullOrWhiteSpace(settings.Destination))
                throw new BindingException("The name of the binding destination is missing.");
            
            if (string.IsNullOrWhiteSpace(settings.VirtualHost))
                throw new VirtualHostMissingException("The name of the virtual host is missing.");
            
            string sanitizedVHost = settings.VirtualHost.SanitizeVirtualHostName();

            string url = settings.BindingType == BindingType.Exchange
                ? $"api/bindings/{sanitizedVHost}/e/{settings.Source}/e/{settings.Destination}"
                : $"api/bindings/{sanitizedVHost}/e/{settings.Source}/q/{settings.Destination}";

            HttpResponseMessage response = await HttpPut(url, settings, cancellationToken);
            Result result = response.GetResponse();

            LogInfo($"Sent request to RabbitMQ server to create a binding between exchanges '{settings.Source}' and '{settings.Destination}' on virtual host '{sanitizedVHost}'.");

            return result;
        }

        public async Task<Result> Delete(Action<BindingDeleteAction> action, CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);

            var impl = new BindingDeleteActionImpl();
            action(impl);
            
            if (string.IsNullOrWhiteSpace(impl.BindingDestination))
                throw new QueueMissingException("The name of the destination binding (queue/exchange) is missing.");

            if (string.IsNullOrWhiteSpace(impl.VirtualHost))
                throw new VirtualHostMissingException("The name of the virtual host is missing.");

            if (string.IsNullOrWhiteSpace(impl.BindingName))
                throw new BindingException("The name of the binding is missing.");
            
            string sanitizedVHost = impl.VirtualHost.SanitizeVirtualHostName();

            string url = impl.BindingType == BindingType.Queue
                ? $"api/bindings/{sanitizedVHost}/e/{impl.BindingSource}/q/{impl.BindingDestination}/{impl.BindingName}"
                : $"api/bindings/{sanitizedVHost}/e/{impl.BindingSource}/e/{impl.BindingDestination}/{impl.BindingName}";

            HttpResponseMessage response = await HttpDelete(url, cancellationToken);
            Result result = response.GetResponse();

            LogInfo($"Sent request to RabbitMQ server for binding '{impl.BindingName}'.");

            return result;
        }

        
        class BindingDeleteActionImpl :
            BindingDeleteAction
        {
            public string VirtualHost { get; private set; }
            public BindingType BindingType { get; private set; }
            public string BindingName { get; private set; }
            public string BindingSource { get; private set; }
            public string BindingDestination { get; private set; }
            
            public void Binding(Action<BindingDeleteDefinition> definition)
            {
                var impl = new BindingDeleteDefinitionImpl();
                definition(impl);

                BindingType = impl.BindingType;
                BindingName = impl.BindingName;
                BindingSource = impl.BindingSource;
                BindingDestination = impl.DestinationSource;
            }

            public void On(Action<BindingOn> location)
            {
                var impl = new BindingOnImpl();
                location(impl);

                VirtualHost = impl.VirtualHostName;
            }

            
            class BindingOnImpl :
                BindingOn
            {
                public string VirtualHostName { get; private set; }

                public void VirtualHost(string vhost) => VirtualHostName = vhost;
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
            static IDictionary<string, object> _arguments;
            static string _vhost;
            static string _source;
            static string _destination;
            static BindingType _bindingType;

            public Lazy<BindingCreateSettings> Settings { get; }

            public BindingCreateActionImpl() => Settings = new Lazy<BindingCreateSettings>(Init, LazyThreadSafetyMode.PublicationOnly);

            BindingCreateSettings Init() => new BindingCreateSettingsImpl(_routingKey, _arguments, _vhost, _source, _destination, _bindingType);

            public void Bind(Action<BindingCreateDefinition> definition)
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

            public void On(Action<BindingOn> location)
            {
                var impl = new BindingOnImpl();
                location(impl);

                _vhost = impl.VirtualHostName;
            }

            
            class BindingOnImpl :
                BindingOn
            {
                public string VirtualHostName { get; private set; }

                public void VirtualHost(string vhost) => VirtualHostName = vhost;
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
                public string Source { get; private set; }
                public string Destination { get; private set; }
                public string RoutingKey { get; private set; }
                public IDictionary<string, object> Arguments { get; private set; }
                
                public void Bind(string source) => Source = source;

                public void To(string destination) => Destination = destination;

                public void WithRoutingKey(string routingKey) => RoutingKey = routingKey;

                public void WithArguments(Action<BindingArguments> arguments)
                {
                    var impl = new BindingArgumentsImpl();
                    arguments(impl);

                    Arguments = impl.Arguments;
                }
            }

            
            class BindingArgumentsImpl :
                BindingArguments
            {
                public IDictionary<string, object> Arguments { get; } = new Dictionary<string, object>();

                public void Set<T>(string arg, T value)
                {
                    Validate(arg);
                    
                    Arguments.Add(arg.Trim(), value);
                }

                void Validate(string arg)
                {
                    if (Arguments.ContainsKey(arg))
                        throw new PolicyDefinitionException($"Argument '{arg}' has already been set");
                }
            }


            class BindingCreateSettingsImpl :
                BindingCreateSettings
            {
                public BindingCreateSettingsImpl(string routingKey, IDictionary<string, object> arguments,
                    string vhost, string source, string destination, BindingType bindingType)
                {
                    RoutingKey = routingKey;
                    Arguments = arguments;
                    Source = source;
                    Destination = destination;
                    VirtualHost = vhost;
                    BindingType = bindingType;
                }

                public string RoutingKey { get; }
                public IDictionary<string, object> Arguments { get; }
                public BindingType BindingType { get; }
                public string Source { get; }
                public string Destination { get; }
                public string VirtualHost { get; }
            }
        }
    }
}