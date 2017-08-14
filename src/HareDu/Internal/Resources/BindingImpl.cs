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
    using BindingDefinition = HareDu.BindingDefinition;

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

            LogInfo($"Sent request to return all binding information corresponding on current RabbitMQ server.");

            HttpResponseMessage response = await HttpGet(url, cancellationToken);
            Result<IEnumerable<BindingInfo>> result = await response.GetResponse<IEnumerable<BindingInfo>>();

            return result;
        }

        public async Task<Result> Create(Action<BindingDefinition> definition, BindingType bindingType,
            CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);

            var impl = new BindingDefinitionImpl();
            definition(impl);

            BindingCreationSettings settings = impl.Settings.Value;

            string sanitizedVHost = settings.VirtualHost.SanitizeVirtualHostName();

            string url = bindingType == BindingType.Exchange
                ? $"api/bindings/{sanitizedVHost}/e/{settings.SourceBinding}/e/{settings.DestinationBinding}"
                : $"api/bindings/{sanitizedVHost}/e/{settings.SourceBinding}/q/{settings.DestinationBinding}";

            LogInfo($"Sent request to RabbitMQ server to create a binding between exchanges '{settings.SourceBinding}' and '{settings.DestinationBinding}' on virtual host '{sanitizedVHost}'.");

            HttpResponseMessage response = await HttpPut(url, settings, cancellationToken);
            Result result = response.GetResponse();

            return result;
        }

        public async Task<Result> Delete(string vhost, string exchange, string queue, CancellationToken cancellationToken = new CancellationToken())
        {
//            cancellationToken.RequestCanceled(LogInfo);
//
//            if (string.IsNullOrWhiteSpace(queue))
//                throw new QueueMissingException("The name of the queue is missing.");
//
//            if (string.IsNullOrWhiteSpace(vhost))
//                throw new VirtualHostMissingException("The name of the virtual host is missing.");
//            
//            string sanitizedVHost = vhost.SanitizeVirtualHostName();
//
//            string url = $"api/queues/{sanitizedVHost}/{queue}";
//            string query = string.Empty;
//
//            LogInfo($"Sent request to RabbitMQ server to delete queue '{queue}' from virtual host '{sanitizedVHost}'.");
//
//            HttpResponseMessage response = await HttpDelete(url, cancellationToken);
//            Result result = response.GetResponse();
//
//            return result;

            throw new NotImplementedException();
        }

        
        class BindingDefinitionImpl :
            BindingDefinition
        {
            static string _routingKey;
            static IDictionary<string, object> _arguments;
            static string _vhost;
            static string _source;
            static string _destination;
            
            public Lazy<BindingCreationSettings> Settings { get; }

            public BindingDefinitionImpl() => Settings = new Lazy<BindingCreationSettings>(Init, LazyThreadSafetyMode.PublicationOnly);

            BindingCreationSettings Init() => new BindingCreationSettingsImpl(_routingKey, _arguments, _vhost, _source, _destination);

            public void Binding(Action<BindingDescription> description)
            {
                var impl = new BindingDescriptionImpl();
                description(impl);

                _source = impl.Source;
                _destination = impl.Destination;
                _arguments = impl.Arguments;
                _routingKey = impl.RoutingKey;
            }

            public void On(string vhost)
            {
                if (string.IsNullOrWhiteSpace(vhost))
                    throw new VirtualHostMissingException("The name of the virtual host is missing.");
            
                _vhost = vhost;
            }


            class BindingDescriptionImpl :
                BindingDescription
            {
                public string Source { get; private set; }
                public string Destination { get; private set; }
                public string RoutingKey { get; private set; }
                public IDictionary<string, object> Arguments { get; private set; }
                
                public void Bind(string source)
                {
                    if (string.IsNullOrWhiteSpace(source))
                        throw new BindingException("The name of the binding source is missing.");

                    Source = source;
                }

                public void To(string destination)
                {
                    if (string.IsNullOrWhiteSpace(destination))
                        throw new BindingException("The name of the binding destination is missing.");
            
                    Destination = destination;
                }

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


            class BindingCreationSettingsImpl :
                BindingCreationSettings
            {
                public BindingCreationSettingsImpl(string routingKey, IDictionary<string, object> arguments,
                    string vhost, string source, string destination)
                {
                    RoutingKey = routingKey;
                    Arguments = arguments;
                    SourceBinding = source;
                    DestinationBinding = destination;
                    VirtualHost = vhost;
                }

                public string RoutingKey { get; }
                public IDictionary<string, object> Arguments { get; }
                public BindingType BindingType { get; }
                public string SourceBinding { get; }
                public string DestinationBinding { get; }
                public string VirtualHost { get; }
            }
        }
    }
}