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

    internal class GlobalParameterImpl :
        ResourceBase,
        GlobalParameter
    {
        public GlobalParameterImpl(HttpClient client, HareDuClientSettings settings)
            : base(client, settings)
        {
        }

        public async Task<Result<IEnumerable<GlobalParameterInfo>>> GetAll(CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);

            string url = $"api/global-parameters";

            LogInfo($"Sent request to return all global parameter information on current RabbitMQ server.");

            HttpResponseMessage response = await HttpGet(url, cancellationToken);
            Result<IEnumerable<GlobalParameterInfo>> result = await response.GetResponse<IEnumerable<GlobalParameterInfo>>();

            return result;
        }

        public async Task<Result> Create(string name, Action<GlobalParameterDefinition> definition, CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);

            if (string.IsNullOrWhiteSpace(name))
                throw new ParameterMissingException("The name of the parameter is missing.");
            
            var impl = new GlobalParameterDefinitionImpl();
            definition(impl);

            GlobalParameterDescription parameterDescription = impl.ParameterDescription.Value;

            string url = $"api/global-parameters/{name}";

            LogInfo($"Sent request to RabbitMQ server to create a global parameter '{name}'.");

            HttpResponseMessage response = await HttpPut(url, parameterDescription, cancellationToken);
            Result result = response.GetResponse();

            return result;
        }

        public async Task<Result> Delete(string name, CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);

            if (string.IsNullOrWhiteSpace(name))
                throw new ParameterMissingException("The name of the parameter is missing.");

            string url = $"api/global-parameters/{name}";

            LogInfo($"Sent request to RabbitMQ server to delete a global parameter '{name}'.");

            HttpResponseMessage response = await HttpDelete(url, cancellationToken);
            Result result = response.GetResponse();

            return result;
        }

        
        class GlobalParameterDefinitionImpl :
            GlobalParameterDefinition
        {
            static IDictionary<string, object> _arguments;
            static string _name;

            public Lazy<GlobalParameterDescription> ParameterDescription { get; }

            public GlobalParameterDefinitionImpl() => ParameterDescription = new Lazy<GlobalParameterDescription>(Init, LazyThreadSafetyMode.PublicationOnly);

            GlobalParameterDescription Init() => new GlobalParameterDescriptionImpl(_name, _arguments);

            public void Name(string name) => _name = name;

            public void WithArguments(Action<GlobalParameterArguments> arguments)
            {
                var impl = new GlobalParameterArgumentsImpl();
                arguments(impl);

                _arguments = impl.Arguments;
            }

            
            class GlobalParameterArgumentsImpl :
                GlobalParameterArguments
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
                        throw new ParameterDefinitionException($"Argument '{arg}' has already been set");
                }
            }


            class GlobalParameterDescriptionImpl :
                GlobalParameterDescription
            {
                public GlobalParameterDescriptionImpl(string name, IDictionary<string, object> arguments)
                {
                    Name = name;
                    Value = arguments;
                }

                public string Name { get; }
                public IDictionary<string, object> Value { get; }
            }
        }
    }
}