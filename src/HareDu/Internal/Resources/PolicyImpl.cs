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
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Configuration;
    using Exceptions;
    using Extensions;
    using Model;

    internal class PolicyImpl :
        ResourceBase,
        Policy
    {
        public PolicyImpl(HttpClient client, HareDuClientSettings settings)
            : base(client, settings)
        {
        }

        public async Task<Result<IEnumerable<PolicyInfo>>> GetAll(CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled(LogInfo);

            string url = $"api/policies";
            var result = await Get<IEnumerable<PolicyInfo>>(url, cancellationToken);

            return result;
        }

        public async Task<Result<PolicyInfo>> Create(Action<PolicyCreateAction> action, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled(LogInfo);

            var impl = new PolicyCreateActionImpl();
            action(impl);

            if (impl.Errors.Value.Any())
                return Result.None<PolicyInfo>(errors: impl.Errors.Value);

            DefinedPolicy definition = impl.Definition.Value;

            Debug.Assert(definition != null);

            string policy = impl.PolicyName.Value;
            string vhost = impl.VirtualHost.Value;
            
            if (string.IsNullOrWhiteSpace(policy))
                return Result.None<PolicyInfo>(errors: new List<Error>{ new ErrorImpl("The name of the policy is missing.") });

            if (string.IsNullOrWhiteSpace(vhost))
                return Result.None<PolicyInfo>(errors: new List<Error>{ new ErrorImpl("The name of the virtual host is missing.") });
            
            string sanitizedVHost = vhost.SanitizeVirtualHostName();

            string url = $"api/policies/{sanitizedVHost}/{policy}";

            var result = await Put<DefinedPolicy, PolicyInfo>(url, definition, cancellationToken);

            return result;
        }

        public async Task<Result<PolicyInfo>> Delete(Action<PolicyDeleteAction> action, CancellationToken cancellationToken = default)
        {
            cancellationToken.RequestCanceled(LogInfo);

            var impl = new PolicyDeleteActionImpl();
            action(impl);
            
            string policy = impl.PolicyName.Value;
            string vhost = impl.VirtualHost.Value;
            
            if (string.IsNullOrWhiteSpace(policy))
                return Result.None<PolicyInfo>(errors: new List<Error>{ new ErrorImpl("The name of the policy is missing.") });

            if (string.IsNullOrWhiteSpace(vhost))
                return Result.None<PolicyInfo>(errors: new List<Error>{ new ErrorImpl("The name of the virtual host is missing.") });

            string sanitizedVHost = vhost.SanitizeVirtualHostName();

            string url = $"api/policies/{sanitizedVHost}/{policy}";

            var result = await Delete<PolicyInfo>(url, cancellationToken);

            return result;
        }

        
        class PolicyDeleteActionImpl :
            PolicyDeleteAction
        {
            static string _vhost;
            static string _policy;
            
            public Lazy<string> PolicyName { get; }
            public Lazy<string> VirtualHost { get; }

            public PolicyDeleteActionImpl()
            {
                VirtualHost = new Lazy<string>(() => _vhost, LazyThreadSafetyMode.PublicationOnly);
                PolicyName = new Lazy<string>(() => _policy, LazyThreadSafetyMode.PublicationOnly);
            }

            public void Policy(string name) => _policy = name;
            
            public void Target(Action<PolicyTarget> target)
            {
                var impl = new PolicyTargetImpl();
                target(impl);

                _vhost = impl.VirtualHostName;
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
            static string _pattern;
            static IDictionary<string, ArgumentValue<object>> _arguments;
            static int _priority;
            static string _applyTo;
            static string _policy;
            static string _vhost;
            static List<Error> _errors;
            
            public Lazy<DefinedPolicy> Definition { get; }
            public Lazy<string> VirtualHost { get; }
            public Lazy<string> PolicyName { get; }
            public Lazy<List<Error>> Errors { get; }

            public PolicyCreateActionImpl()
            {
                _errors = _arguments.Select(x => x.Value.Error).ToList();
                
                Definition = new Lazy<DefinedPolicy>(
                    () => new DefinedPolicyImpl(_pattern, _arguments, _priority, _applyTo), LazyThreadSafetyMode.PublicationOnly);
                VirtualHost = new Lazy<string>(() => _vhost, LazyThreadSafetyMode.PublicationOnly);
                PolicyName = new Lazy<string>(() => _policy, LazyThreadSafetyMode.PublicationOnly);
                Errors = new Lazy<List<Error>>(() => _errors, LazyThreadSafetyMode.PublicationOnly);
            }

            public void Policy(string name) => _policy = name;

            public void Configure(Action<PolicyConfiguration> configuration)
            {
                var impl = new PolicyConfigurationImpl();
                configuration(impl);

                _applyTo = impl.AppllyTo;
                _pattern = impl.Pattern;
                _priority = impl.Priority;
                _arguments = impl.Arguments;

                if (!_arguments.ContainsKey("ha-mode"))
                    return;
                
                if (!_arguments.TryGetValue("ha-mode", out var value))
                    _errors.Add(new ErrorImpl($"Argument 'ha-mode' was set without a corresponding value."));

                string mode = value.ToString().Trim();
                if ((mode.ConvertTo() == HighAvailabilityModes.Exactly || mode.ConvertTo() == HighAvailabilityModes.Nodes) &&
                    !_arguments.ContainsKey("ha-params"))
                    _errors.Add(new ErrorImpl($"Argument 'ha-mode' has been set to {mode}, which means that argument 'ha-params' has to also be set"));
            }
            
            public void Targeting(Action<PolicyTarget> target)
            {
                var impl = new PolicyTargetImpl();
                target(impl);

                _vhost = impl.VirtualHostName;
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
                public string AppllyTo { get; private set; }
                public IDictionary<string, ArgumentValue<object>> Arguments { get; private set; }

                public void UsingPattern(string pattern) => Pattern = pattern;

                public void HasArguments(Action<PolicyDefinitionArguments> arguments)
                {
                    var impl = new PolicyDefinitionArgumentsImpl();
                    arguments(impl);

                    Arguments = impl.Arguments;
                }

                public void HasPriority(int priority) => Priority = priority;

                public void AppliedTo(string applyTo) => AppllyTo = applyTo;
            }
            
            
            class PolicyDefinitionArgumentsImpl :
                PolicyDefinitionArguments
            {
                public IDictionary<string, ArgumentValue<object>> Arguments { get; } = new Dictionary<string, ArgumentValue<object>>();
                
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
                    SetArg("ha-mode", mode.ConvertTo());
                }

                public void SetHighAvailabilityParams(string value)
                {
                    SetArg("ha-params", value);
                }

                public void SetHighAvailabilitySyncMode(HighAvailabilitySyncModes mode)
                {
                    SetArg("ha-sync-mode", mode.ConvertTo());
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
                        Arguments.ContainsKey(arg) || (arg == targetArg && Arguments.ContainsKey(targetArg))
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

            
            class DefinedPolicyImpl :
                DefinedPolicy
            {
                public DefinedPolicyImpl(string pattern, IDictionary<string, ArgumentValue<object>> arguments, int priority, string applyTo)
                {
                    Pattern = pattern;
                    Arguments = arguments.ToDictionary(x => x.Key, x => x.Value.Value);
                    Priority = priority;
                    ApplyTo = applyTo;
                }

                public string Pattern { get; }
                public IDictionary<string, object> Arguments { get; }
                public int Priority { get; }
                public string ApplyTo { get; }
            }
        }
    }
}