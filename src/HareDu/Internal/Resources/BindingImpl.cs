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

            LogInfo($"Sent request to return all binding information corresponding on current RabbitMQ server.");

            HttpResponseMessage response = await HttpGet(url, cancellationToken);
            Result<IEnumerable<BindingInfo>> result = await response.GetResponse<IEnumerable<BindingInfo>>();

            return result;
        }

        public async Task<Result> Create(string vhost, string source, string destination, Action<BindingBehavior> behavior,
            CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);

            if (string.IsNullOrWhiteSpace(source))
                throw new ExchangeMissingException("The name of the source exchange is missing.");

            if (string.IsNullOrWhiteSpace(destination))
                throw new ExchangeMissingException("The name of the destination exchange is missing.");

            if (string.IsNullOrWhiteSpace(vhost))
                throw new VirtualHostMissingException("The name of the virtual host is missing.");
            
            var impl = new BindingBehaviorImpl();
            behavior(impl);

            BindingSetting settings = impl.Settings.Value;

            string sanitizedVHost = vhost.SanitizeVirtualHostName();

            string url = settings.BindingType == BindingType.Exchange
                ? $"api/bindings/{sanitizedVHost}/e/{source}/e/{destination}"
                : $"api/bindings/{sanitizedVHost}/e/{source}/q/{destination}";

            LogInfo($"Sent request to RabbitMQ server to create a binding between exchanges '{source}' and '{destination}' on virtual host '{sanitizedVHost}'.");

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

        
        class BindingBehaviorImpl :
            BindingBehavior
        {
            static string _routingKey;
            static IDictionary<string, object> _arguments;
            static BindingType _bindingType;
            
            public Lazy<BindingSetting> Settings { get; }

            public BindingBehaviorImpl() => Settings = new Lazy<BindingSetting>(Init, LazyThreadSafetyMode.PublicationOnly);

            BindingSetting Init() => new BindingSettingImpl(_routingKey, _arguments, _bindingType);

            public void RoutingKey(string routingKey) => _routingKey = routingKey;

            public void WithArguments(IDictionary<string, object> arguments)
            {
                if (arguments == null)
                    return;

                _arguments = arguments;
            }
            
            public void BindingType(BindingType bindingType) => _bindingType = bindingType;


            class BindingSettingImpl :
                BindingSetting
            {
                public BindingSettingImpl(string routingKey, IDictionary<string, object> arguments, BindingType bindingType)
                {
                    RoutingKey = routingKey;
                    Arguments = arguments;
                    BindingType = bindingType;
                }

                public string RoutingKey { get; }
                public IDictionary<string, object> Arguments { get; }
                public BindingType BindingType { get; }
            }
        }
    }
}