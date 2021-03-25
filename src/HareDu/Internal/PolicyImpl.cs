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

    class PolicyImpl :
        BaseBrokerObject,
        Policy
    {
        public PolicyImpl(HttpClient client)
            : base(client)
        {
        }

        public async Task<ResultList<PolicyInfo>> GetAll(CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            string url = "api/policies";

            ResultList<PolicyInfoImpl> result = await GetAll<PolicyInfoImpl>(url, cancellationToken).ConfigureAwait(false);

            ResultList<PolicyInfo> MapResult(ResultList<PolicyInfoImpl> result) => new ResultListCopy(result);

            return MapResult(result);
        }

        public async Task<Result> Create(Action<PolicyCreateAction> action, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            var impl = new PolicyCreateActionImpl();
            action(impl);

            impl.Validate();

            PolicyDefinition definition = impl.Definition.Value;

            Debug.Assert(definition != null);
            
            string url = $"api/policies/{impl.VirtualHost.Value.ToSanitizedName()}/{impl.PolicyName.Value}";
            
            if (impl.Errors.Value.Any())
                return new FaultedResult(impl.Errors.Value, new DebugInfoImpl(url, definition.ToJsonString(Deserializer.Options)));

            return await Put(url, definition, cancellationToken).ConfigureAwait(false);
        }

        public Task<Result> Delete(Action<PolicyDeleteAction> action, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            var impl = new PolicyDeleteActionImpl();
            action(impl);

            impl.Verify();
            
            string url = $"api/policies/{impl.VirtualHost.Value.ToSanitizedName()}/{impl.PolicyName.Value}";
            
            if (impl.Errors.Value.Any())
                return Task.FromResult<Result>(new FaultedResult(impl.Errors.Value, new DebugInfoImpl(url)));

            return Delete(url, cancellationToken);
       }

        public async Task<Result> Create(string policy, string pattern, string vhost, Action<PolicyConfigurator> configurator,
            PolicyAppliedTo appliedTo = PolicyAppliedTo.All, int priority = 0, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            var impl = new PolicyConfiguratorImpl();
            configurator?.Invoke(impl);

            impl.Validate();

            PolicyRequest request = new PolicyRequest(pattern, impl.Arguments.Value, priority, appliedTo);

            Debug.Assert(request != null);
            
            var errors = new List<Error>();
            
            errors.AddRange(impl.Errors.Value);
            
            if (string.IsNullOrWhiteSpace(policy))
                errors.Add(new ErrorImpl("The name of the policy is missing."));

            if (string.IsNullOrWhiteSpace(vhost))
                errors.Add(new ErrorImpl("The name of the virtual host is missing."));

            if (string.IsNullOrWhiteSpace(pattern))
                errors.Add(new ErrorImpl("Pattern was not set."));

            string url = $"api/policies/{vhost.ToSanitizedName()}/{policy}";
            
            if (errors.Any())
                return new FaultedResult(new DebugInfoImpl(url, request.ToJsonString(Deserializer.Options), errors));

            return await PutRequest(url, request, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Result> Delete(string policy, string vhost, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled();

            var errors = new List<Error>();
            
            if (string.IsNullOrWhiteSpace(policy))
                errors.Add(new ErrorImpl("The name of the policy is missing."));

            if (string.IsNullOrWhiteSpace(vhost))
                errors.Add(new ErrorImpl("The name of the virtual host is missing."));
            
            string url = $"api/policies/{vhost.ToSanitizedName()}/{policy}";

            if (errors.Any())
                return new FaultedResult(new DebugInfoImpl(url, errors));

            return await DeleteRequest(url, cancellationToken).ConfigureAwait(false);
       }

        
        class ResultListCopy :
            ResultList<PolicyInfo>
        {
            public ResultListCopy(ResultList<PolicyInfoImpl> result)
            {
                Timestamp = result.Timestamp;
                DebugInfo = result.DebugInfo;
                Errors = result.Errors;
                HasFaulted = result.HasFaulted;
                HasData = result.HasData;

                var data = new List<PolicyInfo>();
                foreach (PolicyInfoImpl item in result.Data)
                    data.Add(new InternalPolicyInfo(item));

                Data = data;
            }

            public DateTimeOffset Timestamp { get; }
            public DebugInfo DebugInfo { get; }
            public IReadOnlyList<Error> Errors { get; }
            public bool HasFaulted { get; }
            public IReadOnlyList<PolicyInfo> Data { get; }
            public bool HasData { get; }
        }


        class PolicyConfiguratorImpl :
            PolicyConfigurator
        {
            readonly IDictionary<string, ArgumentValue<object>> _arguments;
            readonly List<Error> _errors;

            public Lazy<IDictionary<string, string>> Arguments { get; }
            public Lazy<List<Error>> Errors { get; }

            public PolicyConfiguratorImpl()
            {
                _errors = new List<Error>();
                _arguments = new Dictionary<string, ArgumentValue<object>>();

                Errors = new Lazy<List<Error>>(() => _errors, LazyThreadSafetyMode.PublicationOnly);
                Arguments = new Lazy<IDictionary<string, string>>(() => _arguments.GetStringArguments(), LazyThreadSafetyMode.PublicationOnly);
            }

            public void Validate()
            {
                foreach (var argument in _arguments?.Where(x => x.Value.IsNull()).Select(x => x.Key))
                    _errors.Add(new ErrorImpl($"Argument '{argument}' has been set without a corresponding value."));

                if (!_arguments.TryGetValue("ha-mode", out var haMode))
                    return;

                string mode = haMode.Value.ToString().Trim();
                if ((mode.Convert() == HighAvailabilityModes.Exactly ||
                    mode.Convert() == HighAvailabilityModes.Nodes) && !_arguments.ContainsKey("ha-params"))
                    _errors.Add(new ErrorImpl($"Argument 'ha-mode' has been set to {mode}, which means that argument 'ha-params' has to also be set"));
            }

            public void Set<T>(string arg, T value)
            {
                SetArgWithConflictingCheck(arg, "federation-upstream", "federation-upstream-set", value);
                SetArgWithConflictingCheck(arg, "ha-mode", value);
                SetArgWithConflictingCheck(arg, "ha-sync-mode", value);
                SetArgWithConflictingCheck(arg, "ha-params", value);
                SetArgWithConflictingCheck(arg, "expires", value);
                SetArgWithConflictingCheck(arg, "message-ttl", value);
                SetArgWithConflictingCheck(arg, "max-length-bytes", value);
                SetArgWithConflictingCheck(arg, "max-length", value);
                SetArgWithConflictingCheck(arg, "dead-letter-exchange", value);
                SetArgWithConflictingCheck(arg, "dead-letter-routing-key", value);
                SetArgWithConflictingCheck(arg, "queue-mode", value);
                SetArgWithConflictingCheck(arg, "alternate-exchange", value);
                SetArgWithConflictingCheck(arg, "queue-master-locator", value);
                SetArgWithConflictingCheck(arg, "ha-promote-on-shutdown", value);
                SetArgWithConflictingCheck(arg, "ha-promote-on-failure", value);
                SetArgWithConflictingCheck(arg, "delivery-limit", value);
            }

            public void SetExpiry(ulong milliseconds) => SetArg("expires", milliseconds);

            public void SetFederationUpstreamSet(string value) => SetArgWithConflictingCheck("federation-upstream-set", "federation-upstream", value.Trim());

            public void SetFederationUpstream(string value) => SetArgWithConflictingCheck("federation-upstream", "federation-upstream-set", value.Trim());

            public void SetHighAvailabilityMode(HighAvailabilityModes mode) => SetArg("ha-mode", mode.Convert());

            public void SetHighAvailabilityParams(uint value) => SetArg("ha-params", value);

            public void SetHighAvailabilitySyncMode(HighAvailabilitySyncMode mode) => SetArg("ha-sync-mode", mode.Convert());

            public void SetMessageTimeToLive(ulong milliseconds) => SetArg("message-ttl", milliseconds);

            public void SetMessageMaxSizeInBytes(ulong value) => SetArg("max-length-bytes", value.ToString());

            public void SetMessageMaxSize(ulong value) => SetArg("max-length", value);

            public void SetDeadLetterExchange(string value) => SetArg("dead-letter-exchange", value.Trim());

            public void SetDeadLetterRoutingKey(string value) => SetArg("dead-letter-routing-key", value.Trim());
            
            public void SetQueueMode(QueueMode mode) => SetArg("queue-mode", mode.Convert());

            public void SetAlternateExchange(string value) => SetArg("alternate-exchange", value.Trim());
            
            public void SetQueueMasterLocator(string key) => SetArg("queue-master-locator", key.Trim());
            
            public void SetQueuePromotionOnShutdown(QueuePromotionShutdownMode mode) => SetArg("ha-promote-on-shutdown", mode.Convert());

            public void SetQueuePromotionOnFailure(QueuePromotionFailureMode mode) => SetArg("ha-promote-on-failure", mode.Convert());

            public void SetDeliveryLimit(ulong limit) => SetArg("delivery-limit", limit);

            void SetArg(string arg, object value) =>
                _arguments.Add(arg.Trim(),
                    _arguments.ContainsKey(arg)
                        ? new ArgumentValue<object>(value, $"Argument '{arg}' has already been set")
                        : new ArgumentValue<object>(value));

            void SetArgWithConflictingCheck(string arg, string targetArg, object value) =>
                _arguments.Add(arg.Trim(),
                    _arguments.ContainsKey(arg) || arg == targetArg && _arguments.ContainsKey(targetArg)
                        ? new ArgumentValue<object>(value, $"Argument '{arg}' has already been set or would conflict with argument '{targetArg}'")
                        : new ArgumentValue<object>(value));

            void SetArgWithConflictingCheck(string arg, string targetArg, string conflictingArg, object value) =>
                _arguments.Add(arg.Trim(),
                    _arguments.ContainsKey(arg)
                    || arg == conflictingArg && _arguments.ContainsKey(targetArg)
                    || arg == targetArg && _arguments.ContainsKey(conflictingArg)
                        ? new ArgumentValue<object>(value, $"Argument '{conflictingArg}' has already been set or would conflict with argument '{arg}'")
                        : new ArgumentValue<object>(value));
        }


        class PolicyDeleteActionImpl :
            PolicyDeleteAction
        {
            string _vhost;
            string _policy;
            readonly List<Error> _errors;
            bool _policyCalled;
            bool _targetCalled;

            public Lazy<string> PolicyName { get; }
            public Lazy<string> VirtualHost { get; }
            public Lazy<List<Error>> Errors { get; }

            public PolicyDeleteActionImpl()
            {
                _errors = new List<Error>();
                
                Errors = new Lazy<List<Error>>(() => _errors, LazyThreadSafetyMode.PublicationOnly);
                VirtualHost = new Lazy<string>(() => _vhost, LazyThreadSafetyMode.PublicationOnly);
                PolicyName = new Lazy<string>(() => _policy, LazyThreadSafetyMode.PublicationOnly);
            }

            public void Policy(string name)
            {
                _policyCalled = true;
                _policy = name;
            
                if (string.IsNullOrWhiteSpace(_policy))
                    _errors.Add(new ErrorImpl("The name of the policy is missing."));
            }
            
            public void Targeting(Action<PolicyTarget> target)
            {
                _targetCalled = true;
                var impl = new PolicyTargetImpl();
                target(impl);

                _vhost = impl.VirtualHostName;

                if (string.IsNullOrWhiteSpace(_vhost))
                    _errors.Add(new ErrorImpl("The name of the virtual host is missing."));
            }

            public void Verify()
            {
                if (!_policyCalled)
                    _errors.Add(new ErrorImpl("The name of the policy is missing."));
                
                if (!_targetCalled)
                    _errors.Add(new ErrorImpl("The name of the virtual host is missing."));
            }

            
            class PolicyTargetImpl :
                PolicyTarget
            {
                public string VirtualHostName { get; private set; }

                public void VirtualHost(string name) => VirtualHostName = name;
            }
        }


        class PolicyCreateActionImpl :
            PolicyCreateAction
        {
            string _pattern;
            IDictionary<string, ArgumentValue<object>> _arguments;
            int _priority;
            string _applyTo;
            string _policy;
            string _vhost;
            readonly List<Error> _errors;
            bool _targetCalled;
            bool _policyCalled;

            public Lazy<PolicyDefinition> Definition { get; }
            public Lazy<string> VirtualHost { get; }
            public Lazy<string> PolicyName { get; }
            public Lazy<List<Error>> Errors { get; }

            public PolicyCreateActionImpl()
            {
                _errors = new List<Error>();
                
                Errors = new Lazy<List<Error>>(() => _errors, LazyThreadSafetyMode.PublicationOnly);
                Definition = new Lazy<PolicyDefinition>(
                    () => new PolicyDefinition(_pattern, _arguments.ToDictionary(x => x.Key, x => x.Value.Value.ToString()), _priority, _applyTo), LazyThreadSafetyMode.PublicationOnly);
                VirtualHost = new Lazy<string>(() => _vhost, LazyThreadSafetyMode.PublicationOnly);
                PolicyName = new Lazy<string>(() => _policy, LazyThreadSafetyMode.PublicationOnly);
            }

            public void Policy(string name)
            {
                _policyCalled = true;
                _policy = name;
                
                if (string.IsNullOrWhiteSpace(_policy))
                    _errors.Add(new ErrorImpl("The name of the policy is missing."));
            }

            public void Configure(Action<PolicyConfiguration> configuration)
            {
                var impl = new PolicyConfigurationImpl();
                configuration(impl);

                _applyTo = impl.WhereToApply;
                _pattern = impl.Pattern;
                _priority = impl.Priority;
                _arguments = impl.Arguments;

                foreach (var argument in _arguments?.Where(x => x.Value.IsNull()).Select(x => x.Key))
                {
                    _errors.Add(new ErrorImpl($"Argument '{argument}' has been set without a corresponding value."));
                }

                if (!_arguments.TryGetValue("ha-mode", out var haMode))
                    return;
                
                string mode = haMode.Value.ToString().Trim();
                if ((mode.Convert() == HighAvailabilityModes.Exactly ||
                     mode.Convert() == HighAvailabilityModes.Nodes) && !_arguments.ContainsKey("ha-params"))
                    _errors.Add(new ErrorImpl($"Argument 'ha-mode' has been set to {mode}, which means that argument 'ha-params' has to also be set"));
            }

            public void Targeting(Action<PolicyTarget> target)
            {
                _targetCalled = true;
                var impl = new PolicyTargetImpl();
                target(impl);

                _vhost = impl.VirtualHostName;

                if (string.IsNullOrWhiteSpace(_vhost))
                    _errors.Add(new ErrorImpl("The name of the virtual host is missing."));
            }

            public void Validate()
            {
                if (!_policyCalled)
                    _errors.Add(new ErrorImpl("The name of the policy is missing."));
                
                if (!_targetCalled)
                    _errors.Add(new ErrorImpl("The name of the virtual host is missing."));
            }

            
            class PolicyTargetImpl :
                PolicyTarget
            {
                public string VirtualHostName { get; private set; }

                public void VirtualHost(string name) => VirtualHostName = name;
            }


            class PolicyConfigurationImpl :
                PolicyConfiguration
            {
                public int Priority { get; private set; }
                public string Pattern { get; private set; }
                public string WhereToApply { get; private set; }
                public IDictionary<string, ArgumentValue<object>> Arguments { get; private set; }

                public void UsingPattern(string pattern) => Pattern = pattern;

                public void HasArguments(Action<PolicyDefinitionArguments> arguments)
                {
                    var impl = new PolicyDefinitionArgumentsImpl();
                    arguments(impl);

                    Arguments = impl.Arguments;
                }

                public void HasPriority(int priority) => Priority = priority;

                public void ApplyTo(string applyTo) => WhereToApply = applyTo;
            }
            
            
            class PolicyDefinitionArgumentsImpl :
                PolicyDefinitionArguments
            {
                public IDictionary<string, ArgumentValue<object>> Arguments { get; }

                public PolicyDefinitionArgumentsImpl()
                {
                    Arguments = new Dictionary<string, ArgumentValue<object>>();
                }

                public void Set<T>(string arg, T value)
                {
                    SetArgWithConflictingCheck(arg, "federation-upstream", "federation-upstream-set", value);
                    SetArgWithConflictingCheck(arg, "ha-mode", value);
                    SetArgWithConflictingCheck(arg, "ha-sync-mode", value);
                    SetArgWithConflictingCheck(arg, "ha-params", value);
                    SetArgWithConflictingCheck(arg, "expires", value);
                    SetArgWithConflictingCheck(arg, "message-ttl", value);
                    SetArgWithConflictingCheck(arg, "max-length-bytes", value);
                    SetArgWithConflictingCheck(arg, "max-length", value);
                    SetArgWithConflictingCheck(arg, "dead-letter-exchange", value);
                    SetArgWithConflictingCheck(arg, "dead-letter-routing-key", value);
                    SetArgWithConflictingCheck(arg, "queue-mode", value);
                    SetArgWithConflictingCheck(arg, "alternate-exchange", value);
                }

                public void SetExpiry(long milliseconds)
                {
                    SetArg("expires", milliseconds);
                }

                public void SetFederationUpstreamSet(string value)
                {
                    SetArgWithConflictingCheck("federation-upstream-set", "federation-upstream", value.Trim());
                }

                public void SetFederationUpstream(string value)
                {
                    SetArgWithConflictingCheck("federation-upstream", "federation-upstream-set", value.Trim());
                }

                public void SetHighAvailabilityMode(HighAvailabilityModes mode)
                {
                    SetArg("ha-mode", mode.Convert());
                }

                public void SetHighAvailabilityParams(string value)
                {
                    SetArg("ha-params", value);
                }

                public void SetHighAvailabilitySyncMode(HighAvailabilitySyncMode mode)
                {
                    SetArg("ha-sync-mode", mode.Convert());
                }

                public void SetMessageTimeToLive(long milliseconds)
                {
                    SetArg("message-ttl", milliseconds);
                }

                public void SetMessageMaxSizeInBytes(long value)
                {
                    SetArg("max-length-bytes", value.ToString());
                }

                public void SetMessageMaxSize(long value)
                {
                    SetArg("max-length", value);
                }

                public void SetDeadLetterExchange(string value)
                {
                    SetArg("dead-letter-exchange", value.Trim());
                }

                public void SetDeadLetterRoutingKey(string value)
                {
                    SetArg("dead-letter-routing-key", value.Trim());
                }

                public void SetQueueMode()
                {
                    SetArg("queue-mode", "lazy");
                }

                public void SetAlternateExchange(string value)
                {
                    SetArg("alternate-exchange", value.Trim());
                }

                void SetArg(string arg, object value)
                {
                    Arguments.Add(arg.Trim(),
                        Arguments.ContainsKey(arg)
                            ? new ArgumentValue<object>(value, $"Argument '{arg}' has already been set")
                            : new ArgumentValue<object>(value));
                }

                void SetArgWithConflictingCheck(string arg, string targetArg, object value)
                {
                    Arguments.Add(arg.Trim(),
                        Arguments.ContainsKey(arg) || arg == targetArg && Arguments.ContainsKey(targetArg)
                            ? new ArgumentValue<object>(value, $"Argument '{arg}' has already been set or would conflict with argument '{targetArg}'")
                            : new ArgumentValue<object>(value));
                }

                void SetArgWithConflictingCheck(string arg, string targetArg, string conflictingArg, object value)
                {
                    Arguments.Add(arg.Trim(),
                        Arguments.ContainsKey(arg)
                        || arg == conflictingArg && Arguments.ContainsKey(targetArg)
                        || arg == targetArg && Arguments.ContainsKey(conflictingArg)
                            ? new ArgumentValue<object>(value, $"Argument '{conflictingArg}' has already been set or would conflict with argument '{arg}'")
                            : new ArgumentValue<object>(value));
                }
            }
        }
    }
}