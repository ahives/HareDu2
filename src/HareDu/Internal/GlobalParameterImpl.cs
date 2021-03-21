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
    using Extensions;
    using HareDu.Model;
    using Serialization;

    class GlobalParameterImpl :
        BaseBrokerObject,
        GlobalParameter
    {
        public GlobalParameterImpl(HttpClient client)
            : base(client)
        {
        }

        public Task<ResultList<GlobalParameterInfo>> GetAll(CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            string url = $"api/global-parameters";
            
            return GetAll<GlobalParameterInfo>(url, cancellationToken);
        }

        public Task<Result> Create(Action<GlobalParameterCreateAction> action, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();
            
            var impl = new GlobalParameterCreateActionImpl();
            action(impl);
            
            impl.Validate();
            
            GlobalParameterDefinition definition = impl.Definition.Value;

            Debug.Assert(definition != null);

            string url = $"api/global-parameters/{definition.Name}";
            
            if (impl.Errors.Value.Any())
                return Task.FromResult<Result>(new FaultedResult(impl.Errors.Value, new DebugInfoImpl(url, definition.ToJsonString(Deserializer.Options))));

            return Put(url, definition, cancellationToken);
        }

        public Task<Result> Delete(Action<GlobalParameterDeleteAction> action, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            var impl = new GlobalParameterDeleteActionImpl();
            action(impl);
            
            if (string.IsNullOrWhiteSpace(impl.ParameterName))
                return Task.FromResult<Result>(new FaultedResult(new List<Error> {new ErrorImpl("The name of the parameter is missing.")},
                    new DebugInfoImpl(@"api/global-parameters/")));

            string url = $"api/global-parameters/{impl.ParameterName}";

            return Delete(url, cancellationToken);
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
            IDictionary<string, ArgumentValue<object>> _arguments;
            string _name;
            object _argument;
            readonly List<Error> _errors;

            public Lazy<GlobalParameterDefinition> Definition { get; }
            public Lazy<List<Error>> Errors { get; }

            public GlobalParameterCreateActionImpl()
            {
                _errors = new List<Error>();
                
                Errors = new Lazy<List<Error>>(() => _errors, LazyThreadSafetyMode.PublicationOnly);
                Definition = new Lazy<GlobalParameterDefinition>(
                    () => new GlobalParameterDefinitionImpl(_name, _arguments, _argument), LazyThreadSafetyMode.PublicationOnly);
            }

            public void Parameter(string name) => _name = name;

            public void Value(Action<GlobalParameterArguments> arguments)
            {
                var impl = new GlobalParameterArgumentsImpl();
                arguments(impl);

                _arguments = impl.Arguments;
            }

            public void Value<T>(T argument) => _argument = argument;

            public void Validate()
            {
                if (string.IsNullOrWhiteSpace(_name))
                    _errors.Add(new ErrorImpl("The name of the parameter is missing."));

                if (_argument != null && _argument.GetType() == typeof(string))
                {
                    if (string.IsNullOrWhiteSpace(_argument.ToString()))
                        _errors.Add(new ErrorImpl("Parameter value is missing."));

                    return;
                }
                
                if (_argument == null && _arguments == null)
                    _errors.Add(new ErrorImpl("Parameter value is missing."));
                
                if (_arguments != null)
                    _errors.AddRange(_arguments
                        .Select(x => x.Value?.Error)
                        .Where(error => !error.IsNull())
                        .ToList());
            }


            class GlobalParameterArgumentsImpl :
                GlobalParameterArguments
            {
                public IDictionary<string, ArgumentValue<object>> Arguments { get; } =
                    new Dictionary<string, ArgumentValue<object>>();

                public void Set<T>(string arg, T value)
                {
                    Arguments.Add(arg.Trim(),
                        Arguments.ContainsKey(arg)
                            ? new ArgumentValue<object>(value, $"Argument '{arg}' has already been set")
                            : new ArgumentValue<object>(value));
                }
            }


            class GlobalParameterDefinitionImpl :
                GlobalParameterDefinition
            {
                public GlobalParameterDefinitionImpl(string name, IDictionary<string, ArgumentValue<object>> arguments,
                    object argument)
                {
                    Name = name;

                    if (!argument.IsNull())
                    {
                        Value = argument;
                        return;
                    }
                    
                    if (arguments.IsNull())
                        return;
                    
                    Value = arguments.ToDictionary(x => x.Key, x => x.Value.Value);
                }

                public string Name { get; }
                public object Value { get; }
            }
        }
    }
}