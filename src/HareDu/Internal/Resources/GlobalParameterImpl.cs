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
    using System.Diagnostics;
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

        public async Task<Result<IEnumerable<GlobalParameterInfo>>> GetAllAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);

            string url = $"api/global-parameters";

            HttpResponseMessage response = await HttpGet(url, cancellationToken);
            Result<IEnumerable<GlobalParameterInfo>> result = await response.GetResponse<IEnumerable<GlobalParameterInfo>>();

            LogInfo($"Sent request to return all global parameter information on current RabbitMQ server.");

            return result;
        }

        public async Task<Result> CreateAsync(Action<GlobalParameterCreateAction> action, CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);
            
            var impl = new GlobalParameterCreateActionImpl();
            action(impl);

            DefinedGlobalParameter definition = impl.Definition.Value;

            Debug.Assert(definition != null);

            if (string.IsNullOrWhiteSpace(definition.Name))
                throw new ParameterMissingException("The name of the parameter is missing.");

            string url = $"api/global-parameters/{definition.Name}";

            HttpResponseMessage response = await HttpPut(url, definition, cancellationToken);
            Result result = await response.GetResponse();

            LogInfo($"Sent request to RabbitMQ server to create a global parameter '{definition.Name}'.");

            return result;
        }

        public async Task<Result> DeleteAsync(Action<GlobalParameterDeleteAction> action, CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);

            var impl = new GlobalParameterDeleteActionImpl();
            action(impl);
            
            if (string.IsNullOrWhiteSpace(impl.ParameterName))
                throw new ParameterMissingException("The name of the parameter is missing.");

            string url = $"api/global-parameters/{impl.ParameterName}";

            HttpResponseMessage response = await HttpDelete(url, cancellationToken);
            Result result = await response.GetResponse();

            LogInfo($"Sent request to RabbitMQ server to delete a global parameter '{impl.ParameterName}'.");

            return result;
        }

        
        class GlobalParameterDeleteActionImpl :
            GlobalParameterDeleteAction
        {
            public string ParameterName { get; private set; }
            
            public void Parameter(string name) => ParameterName = name;
        }


        class GlobalParameterCreateActionImpl :
            GlobalParameterCreateAction
        {
            static IDictionary<string, object> _arguments;
            static string _name;

            public Lazy<DefinedGlobalParameter> Definition { get; }

            public GlobalParameterCreateActionImpl() => Definition = new Lazy<DefinedGlobalParameter>(
                () => new DefinedGlobalParameterImpl(_name, _arguments), LazyThreadSafetyMode.PublicationOnly);

            public void Parameter(string name) => _name = name;
            
            public void Configure(Action<GlobalParameterConfiguration> configuration)
            {
                var impl = new GlobalParameterConfigurationImpl();
                configuration(impl);
                
                _arguments = impl.Arguments;
            }

            class GlobalParameterConfigurationImpl :
                GlobalParameterConfiguration
            {
                public IDictionary<string, object> Arguments { get; private set; }
                
                public void WithArguments(Action<GlobalParameterArguments> arguments)
                {
                    var impl = new GlobalParameterArgumentsImpl();
                    arguments(impl);

                    Arguments = impl.Arguments;
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
            }


            class DefinedGlobalParameterImpl :
                DefinedGlobalParameter
            {
                public DefinedGlobalParameterImpl(string name, IDictionary<string, object> arguments)
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