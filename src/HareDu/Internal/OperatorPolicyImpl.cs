namespace HareDu.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Net.Http;
    using System.Text.Json.Serialization;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Core.Extensions;
    using Extensions;
    using HareDu.Model;
    using Serialization;

    public class OperatorPolicyImpl :
        BaseBrokerObject,
        OperatorPolicy
    {
        public OperatorPolicyImpl(HttpClient client) :
            base(client)
        {
        }
        
        public async Task<ResultList<OperatorPolicyInfo>> GetAll(CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            string url = "api/operator-policies";

            var result = await GetAllRequest<OperatorPolicyInfoImpl>(url, cancellationToken).ConfigureAwait(false);

            ResultList<OperatorPolicyInfo> MapResult(ResultList<OperatorPolicyInfoImpl> result) => new ResultListCopy(result);

            return MapResult(result);
        }

        class ResultListCopy :
            ResultList<OperatorPolicyInfo>
        {
            public ResultListCopy(ResultList<OperatorPolicyInfoImpl> result)
            {
                Timestamp = result.Timestamp;
                DebugInfo = result.DebugInfo;
                Errors = result.Errors;
                HasFaulted = result.HasFaulted;
                HasData = result.HasData;

                var data = new List<OperatorPolicyInfo>();
                foreach (OperatorPolicyInfoImpl item in result.Data)
                    data.Add(new InternalOperatorPolicyInfo(item));

                Data = data;
            }

            public DateTimeOffset Timestamp { get; }
            public DebugInfo DebugInfo { get; }
            public IReadOnlyList<Error> Errors { get; }
            public bool HasFaulted { get; }
            public IReadOnlyList<OperatorPolicyInfo> Data { get; }
            public bool HasData { get; }
        }

        public async Task<Result> Create(string policy, string pattern, string vhost, Action<OperatorPolicyConfigurator> configurator,
            OperatorPolicyAppliedTo appliedTo = default, int priority = default, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();
        
            var impl = new OperatorPolicyConfiguratorImpl();
            configurator?.Invoke(impl);

            impl.Validate();

            OperatorPolicyRequest request = new OperatorPolicyRequest(pattern, impl.Arguments.Value, priority, appliedTo);
        
            Debug.Assert(request != null);
            
            var errors = new List<Error>();
            
            errors.AddRange(impl.Errors.Value);
            
            if (string.IsNullOrWhiteSpace(policy))
                errors.Add(new ErrorImpl("The name of the operator policy is missing."));
        
            if (string.IsNullOrWhiteSpace(vhost))
                errors.Add(new ErrorImpl("The name of the virtual host is missing."));
                
            string url = $"api/operator-policies/{vhost.ToSanitizedName()}/{policy}";
            
            if (errors.Any())
                return new FaultedResult(new DebugInfoImpl(url, request.ToJsonString(Deserializer.Options), errors));
        
            return await PutRequest(url, request, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Result> Delete(string policy, string vhost, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            var errors = new List<Error>();
            
            if (string.IsNullOrWhiteSpace(policy))
                errors.Add(new ErrorImpl("The name of the operator policy is missing."));

            if (string.IsNullOrWhiteSpace(vhost))
                errors.Add(new ErrorImpl("The name of the virtual host is missing."));
            
            string url = $"api/operator-policies/{vhost.ToSanitizedName()}/{policy}";

            if (errors.Any())
                return new FaultedResult(new DebugInfoImpl(url, errors));

            return await DeleteRequest(url, cancellationToken).ConfigureAwait(false);
        }


        class OperatorPolicyConfiguratorImpl :
            OperatorPolicyConfigurator
        {
            readonly IDictionary<string, ArgumentValue<ulong>> _arguments;
            readonly List<Error> _errors;

            public Lazy<IDictionary<string, ulong>> Arguments { get; }
            public Lazy<List<Error>> Errors { get; }

            public OperatorPolicyConfiguratorImpl()
            {
                _errors = new List<Error>();
                _arguments = new Dictionary<string, ArgumentValue<ulong>>();
                
                Errors = new Lazy<List<Error>>(() => _errors, LazyThreadSafetyMode.PublicationOnly);
                Arguments = new Lazy<IDictionary<string, ulong>>(() => _arguments.GetArgumentsOrNull(), LazyThreadSafetyMode.PublicationOnly);
            }

            public void SetExpiry(ulong milliseconds) => SetArg("expires", milliseconds);
                
            public void SetMaxInMemoryBytes(ulong bytes) => SetArg("max-in-memory-bytes", bytes);

            public void SetMaxInMemoryLength(ulong messages) => SetArg("max-in-memory-length", messages);

            public void SetDeliveryLimit(ulong limit) => SetArg("delivery-limit", limit);

            public void SetMessageTimeToLive(ulong milliseconds) => SetArg("message-ttl", milliseconds);

            public void SetMessageMaxSizeInBytes(ulong value) => SetArg("max-length-bytes", value);

            public void SetMessageMaxSize(ulong value) => SetArg("max-length", value);

            public void Validate()
            {
                if (_arguments.IsNull() || !_arguments.Any())
                    _errors.Add(new ErrorImpl("No arguments have been set."));
            }
            
            void SetArg(string arg, ulong value) =>
                _arguments.Add(arg.Trim(),
                    _arguments.ContainsKey(arg)
                        ? new ArgumentValue<ulong>(value, $"Argument '{arg}' has already been set")
                        : new ArgumentValue<ulong>(value));
        }
    }

    class InternalOperatorPolicyInfo :
        OperatorPolicyInfo
    {
        public InternalOperatorPolicyInfo(OperatorPolicyInfoImpl obj)
        {
            VirtualHost = obj.VirtualHost;
            Name = obj.Name;
            Pattern = obj.Pattern;
            AppliedTo = obj.AppliedTo;
            Definition = obj.Definition;
            Priority = obj.Priority;
        }

        public string VirtualHost { get; }
        public string Name { get; }
        public string Pattern { get; }
        public string AppliedTo { get; }
        public IDictionary<string, ulong> Definition { get; }
        public int Priority { get; }
    }

    class OperatorPolicyInfoImpl
    {
        [JsonPropertyName("vhost")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string VirtualHost { get; set; }
        
        [JsonPropertyName("name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Name { get; set; }
        
        [JsonPropertyName("pattern")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Pattern { get; set; }
        
        [JsonPropertyName("apply-to")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string AppliedTo { get; set; }
        
        [JsonPropertyName("definition")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public IDictionary<string, ulong> Definition { get; set; }
        
        [JsonPropertyName("priority")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int Priority { get; set; }
    }
}