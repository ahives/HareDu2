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
    using Exceptions;
    using Extensions;

    internal class GlobalParameterImpl :
        ResourceBase,
        GlobalParameter
    {
        public GlobalParameterImpl(HttpClient client)
            : base(client)
        {
        }

        public async Task<Result<IReadOnlyList<GlobalParameterInfo>>> GetAll(CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            string url = $"api/global-parameters";
            var result = await GetAll<GlobalParameterInfo>(url, cancellationToken);

            return result;
        }

        public async Task<Result> Create(Action<GlobalParameterCreateAction> action, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();
            
            var impl = new GlobalParameterCreateActionImpl();
            action(impl);
            
            var errors = new List<Error>();
            
            errors.AddRange(impl.Errors.Value);

            DefinedGlobalParameter definition = impl.Definition.Value;

            Debug.Assert(definition != null);

            if (string.IsNullOrWhiteSpace(definition.Name))
                errors.Add(new ErrorImpl("The name of the parameter is missing."));
            
            if (errors.Any())
                return new FaultedResult(errors);

            string url = $"api/global-parameters/{definition.Name}";

            var result = await Put(url, definition, cancellationToken);

            return result;
        }

        public async Task<Result> Delete(Action<GlobalParameterDeleteAction> action, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            var impl = new GlobalParameterDeleteActionImpl();
            action(impl);
            
            if (string.IsNullOrWhiteSpace(impl.ParameterName))
                return new FaultedResult(new List<Error> {new ErrorImpl("The name of the parameter is missing.")});

            string url = $"api/global-parameters/{impl.ParameterName}";

            var result = await Delete(url, cancellationToken);

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
            static IDictionary<string, ArgumentValue<object>> _arguments;
            static string _name;

            public Lazy<DefinedGlobalParameter> Definition { get; }
            public Lazy<List<Error>> Errors { get; }

            public GlobalParameterCreateActionImpl()
            {
                Errors = new Lazy<List<Error>>(() => _arguments.Select(x => x.Value.Error).Where(x => !x.IsNull()).ToList(), LazyThreadSafetyMode.PublicationOnly);
                Definition = new Lazy<DefinedGlobalParameter>(
                    () => new DefinedGlobalParameterImpl(_name, _arguments), LazyThreadSafetyMode.PublicationOnly);
            }

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
                public IDictionary<string, ArgumentValue<object>> Arguments { get; private set; }
                
                public void HasArguments(Action<GlobalParameterArguments> arguments)
                {
                    var impl = new GlobalParameterArgumentsImpl();
                    arguments(impl);

                    Arguments = impl.Arguments;
                }


                class GlobalParameterArgumentsImpl :
                    GlobalParameterArguments
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
            }


            class DefinedGlobalParameterImpl :
                DefinedGlobalParameter
            {
                public DefinedGlobalParameterImpl(string name, IDictionary<string, ArgumentValue<object>> arguments)
                {
                    Name = name;
                    Value = arguments.ToDictionary(x => x.Key, x => x.Value.Value);
                }

                public string Name { get; }
                public IDictionary<string, object> Value { get; }
            }
        }
    }
}