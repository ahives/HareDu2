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
    using Model;
    using Serialization;

    class GlobalParameterImpl :
        BaseBrokerObject,
        GlobalParameter
    {
        public GlobalParameterImpl(HttpClient client)
            : base(client)
        {
        }

        public async Task<ResultList<GlobalParameterInfo>> GetAll(CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            string url = "api/global-parameters";

            var result = await GetAll<GlobalParameterInfoImpl>(url, cancellationToken).ConfigureAwait(false);

            ResultList<GlobalParameterInfo> MapResult(ResultList<GlobalParameterInfoImpl> result) => new ResultListCopy(result);

            return MapResult(result);
        }

        public async Task<Result> Create(Action<GlobalParameterCreateAction> action, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();
            
            var impl = new GlobalParameterCreateActionImpl();
            action(impl);
            
            impl.Validate();
            
            GlobalParameterDefinition definition = impl.Definition.Value;

            Debug.Assert(definition != null);

            string url = $"api/global-parameters/{definition.Name}";
            
            if (impl.Errors.Value.Any())
                return new FaultedResult(impl.Errors.Value, new DebugInfoImpl(url, definition.ToJsonString(Deserializer.Options)));

            return await Put(url, definition, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Result> Delete(Action<GlobalParameterDeleteAction> action, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            var impl = new GlobalParameterDeleteActionImpl();
            action(impl);
            
            if (string.IsNullOrWhiteSpace(impl.ParameterName))
                return new FaultedResult(new List<Error> {new ErrorImpl("The name of the parameter is missing.")}, new DebugInfoImpl(@"api/global-parameters/"));

            string url = $"api/global-parameters/{impl.ParameterName}";

            return await Delete(url, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Result> Create(string parameter, Action<GlobalParameterConfigurator> configurator, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();
            
            var impl = new GlobalParameterConfiguratorImpl(parameter);
            configurator?.Invoke(impl);
            
            impl.Validate();
            
            GlobalParameterRequest request = impl.Request.Value;

            Debug.Assert(request != null);

            var errors = new List<Error>();
            
            errors.AddRange(impl.Errors.Value);
            
            if (string.IsNullOrWhiteSpace(parameter))
                errors.Add(new ErrorImpl("The name of the parameter is missing."));

            string url = $"api/global-parameters/{parameter}";
            
            if (errors.Any())
                return new FaultedResult(new DebugInfoImpl(url, request.ToJsonString(Deserializer.Options), errors));

            return await PutRequest(url, request, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Result> Delete(string parameter, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();
            
            var errors = new List<Error>();
            
            if (string.IsNullOrWhiteSpace(parameter))
                errors.Add(new ErrorImpl("The name of the parameter is missing."));

            string url = $"api/global-parameters/{parameter}";
            
            if (errors.Any())
                return new FaultedResult(new DebugInfoImpl(url, errors));

            return await DeleteRequest(url, cancellationToken).ConfigureAwait(false);
        }

        
        class ResultListCopy :
            ResultList<GlobalParameterInfo>
        {
            public ResultListCopy(ResultList<GlobalParameterInfoImpl> result)
            {
                Timestamp = result.Timestamp;
                DebugInfo = result.DebugInfo;
                Errors = result.Errors;
                HasFaulted = result.HasFaulted;
                HasData = result.HasData;

                var data = new List<GlobalParameterInfo>();
                foreach (GlobalParameterInfoImpl item in result.Data)
                    data.Add(new InternalGlobalParameterInfo(item));

                Data = data;
            }

            public DateTimeOffset Timestamp { get; }
            public DebugInfo DebugInfo { get; }
            public IReadOnlyList<Error> Errors { get; }
            public bool HasFaulted { get; }
            public IReadOnlyList<GlobalParameterInfo> Data { get; }
            public bool HasData { get; }
        }


        class GlobalParameterConfiguratorImpl :
            GlobalParameterConfigurator
        {
            IDictionary<string, ArgumentValue<object>> _arguments;
            object _argument;
            
            readonly List<Error> _errors;

            public Lazy<GlobalParameterRequest> Request { get; }
            public Lazy<List<Error>> Errors { get; }

            public GlobalParameterConfiguratorImpl(string name)
            {
                _errors = new List<Error>();
                
                Errors = new Lazy<List<Error>>(() => _errors, LazyThreadSafetyMode.PublicationOnly);
                Request = new Lazy<GlobalParameterRequest>(
                    () => new GlobalParameterRequest(name, _argument.IsNotNull() ? _argument : _arguments.GetArgumentsOrNull()), LazyThreadSafetyMode.PublicationOnly);
            }

            public void Value(Action<GlobalParameterArgumentConfigurator> configurator)
            {
                var impl = new GlobalParameterArgumentConfiguratorImpl();
                configurator?.Invoke(impl);

                _arguments = impl.Arguments;
            }

            public void Value<T>(T argument)
            {
                _argument = argument;
            }

            public void Validate()
            {
                if (_argument != null && _argument.GetType() == typeof(string))
                {
                    if (string.IsNullOrWhiteSpace(_argument.ToString()))
                        _errors.Add(new ErrorImpl("Parameter value is missing."));
                
                    return;
                }
                
                if (_argument == null && _arguments == null)
                    _errors.Add(new ErrorImpl("Parameter value is missing."));
                
                if (_arguments != null)
                {
                    _errors.AddRange(_arguments
                        .Select(x => x.Value?.Error)
                        .Where(error => error.IsNotNull())
                        .ToList());
                }
            }


            class GlobalParameterArgumentConfiguratorImpl :
                GlobalParameterArgumentConfigurator
            {
                public IDictionary<string, ArgumentValue<object>> Arguments { get; } =
                    new Dictionary<string, ArgumentValue<object>>();

                public void Add<T>(string arg, T value) =>
                    Arguments.Add(arg.Trim(),
                        Arguments.ContainsKey(arg)
                            ? new ArgumentValue<object>(value, $"Argument '{arg}' has already been set")
                            : new ArgumentValue<object>(value));
            }
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
                    () => new GlobalParameterDefinition(_name, _arguments, _argument), LazyThreadSafetyMode.PublicationOnly);
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
        }
    }
}