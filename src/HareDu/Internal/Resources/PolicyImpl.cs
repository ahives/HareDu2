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

        public async Task<Result<IEnumerable<PolicyInfo>>> GetAllAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);

            string url = $"api/policies";

            HttpResponseMessage response = await HttpGet(url, cancellationToken);
            Result<IEnumerable<PolicyInfo>> result = await response.GetResponse<IEnumerable<PolicyInfo>>();

            LogInfo($"Sent request to return all policy information on current RabbitMQ server.");

            return result;
        }

        public async Task<Result> CreateAsync(Action<PolicyCreateAction> action, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.RequestCanceled(LogInfo);

            var impl = new PolicyCreateActionImpl();
            action(impl);

            DefinedPolicy definition = impl.Definition.Value;

            string policy = impl.PolicyName.Value;
            string vhost = impl.VirtualHost.Value;
            
            if (string.IsNullOrWhiteSpace(policy))
                throw new PolicyNameMissingException("The name of the policy is missing.");

            if (string.IsNullOrWhiteSpace(vhost))
                throw new VirtualHostMissingException("The name of the virtual host is missing.");
            
            string sanitizedVHost = vhost.SanitizeVirtualHostName();

            string url = $"api/policies/{sanitizedVHost}/{policy}";

            HttpResponseMessage response = await HttpPut(url, definition, cancellationToken);
            Result result = await response.GetResponse();

            LogInfo($"Sent request to RabbitMQ server to create a policy '{policy}' in virtual host '{sanitizedVHost}'.");

            return result;
        }

        public async Task<Result> DeleteAsync(Action<PolicyDeleteAction> action, CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);

            var impl = new PolicyDeleteActionImpl();
            action(impl);
            
            string policy = impl.PolicyName.Value;
            string vhost = impl.VirtualHost.Value;
            
            if (string.IsNullOrWhiteSpace(policy))
                throw new PolicyNameMissingException("The name of the policy is missing.");

            if (string.IsNullOrWhiteSpace(vhost))
                throw new VirtualHostMissingException("The name of the virtual host is missing.");

            string sanitizedVHost = vhost.SanitizeVirtualHostName();

            string url = $"api/policies/{sanitizedVHost}/{policy}";

            HttpResponseMessage response = await HttpDelete(url, cancellationToken);
            Result result = await response.GetResponse();

            LogInfo($"Sent request to RabbitMQ server to create a policy '{policy}' in virtual host '{sanitizedVHost}'.");

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

                public void VirtualHost(string vhost) => VirtualHostName = vhost;
            }
        }

        
        class PolicyCreateActionImpl :
            PolicyCreateAction
        {
            static string _pattern;
            static IDictionary<string, object> _arguments;
            static int _priority;
            static string _applyTo;
            static string _policy;
            static string _vhost;
            
            public Lazy<DefinedPolicy> Definition { get; }
            public Lazy<string> VirtualHost { get; }
            public Lazy<string> PolicyName { get; }

            public PolicyCreateActionImpl()
            {
                Definition = new Lazy<DefinedPolicy>(
                    () => new DefinedPolicyImpl(_pattern, _arguments, _priority, _applyTo), LazyThreadSafetyMode.PublicationOnly);
                VirtualHost = new Lazy<string>(() => _vhost, LazyThreadSafetyMode.PublicationOnly);
                PolicyName = new Lazy<string>(() => _policy, LazyThreadSafetyMode.PublicationOnly);
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

                if (_arguments.ContainsKey("ha-mode"))
                {
                    object value;
                    if (!_arguments.TryGetValue("ha-mode", out value))
                        throw new PolicyDefinitionException($"Argument 'ha-mode' was set without a corresponding value.");

                    string mode = value.ToString().Trim();
                    if (((mode.ConvertTo() == HighAvailabilityModes.Exactly) || (mode.ConvertTo() == HighAvailabilityModes.Nodes)) &&
                        !_arguments.ContainsKey("ha-params"))
                        throw new PolicyDefinitionException($"Argument 'ha-mode' has been set to {mode}, which means that argument 'ha-params' has to also be set");
                }
            }
            
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

                public void VirtualHost(string vhost) => VirtualHostName = vhost;
            }


            class PolicyConfigurationImpl :
                PolicyConfiguration
            {
                public int Priority { get; private set; }
                public string Pattern { get; private set; }
                public string AppllyTo { get; private set; }
                public IDictionary<string, object> Arguments { get; private set; }

                public void UsingPattern(string pattern) => Pattern = pattern;

                public void WithArguments(Action<PolicyDefinitionArguments> arguments)
                {
                    var impl = new PolicyDefinitionArgumentsImpl();
                    arguments(impl);

                    Arguments = impl.Arguments;
                }

                public void WithPriority(int priority) => Priority = priority;

                public void AppliedTo(string applyTo) => AppllyTo = applyTo;
            }
            
            
            class PolicyDefinitionArgumentsImpl :
                PolicyDefinitionArguments
            {
                public IDictionary<string, object> Arguments { get; } = new Dictionary<string, object>();
                
                public void Set<T>(string arg, T value)
                {
                    ValidateConflictingArgs(arg, "federation-upstream", "federation-upstream-set");
                    ValidateConflictingArgs(arg, "ha-mode");
                    ValidateConflictingArgs(arg, "ha-sync-mode");
                    ValidateConflictingArgs(arg, "ha-params");
                    ValidateConflictingArgs(arg, "expires");
                    ValidateConflictingArgs(arg, "message-ttl");
                    ValidateConflictingArgs(arg, "max-length-bytes");
                    ValidateConflictingArgs(arg, "max-length");
                    ValidateConflictingArgs(arg, "dead-letter-exchange");
                    ValidateConflictingArgs(arg, "dead-letter-routing-key");
                    ValidateConflictingArgs(arg, "queue-mode");
                    ValidateConflictingArgs(arg, "alternate-exchange");
                    
                    Arguments.Add(arg, value);
                }

                public void SetExpiry(long milliseconds)
                {
                    Validate("expires");
                    
                    Arguments.Add("expires", milliseconds);
                }

                public void SetFederationUpstreamSet(string value)
                {
                    ValidateConflictingArgs("federation-upstream-set", "federation-upstream");
                    
                    Arguments.Add("federation-upstream-set", value.Trim());
                }

                public void SetFederationUpstream(string value)
                {
                    ValidateConflictingArgs("federation-upstream", "federation-upstream-set");
                    
                    Arguments.Add("federation-upstream", value.Trim());
                }

                public void SetHighAvailabilityMode(HighAvailabilityModes mode)
                {
                    Validate("ha-mode");
                    
                    Arguments.Add("ha-mode", mode.ConvertTo());
                }

                public void SetHighAvailabilityParams(string value)
                {
                    Validate("ha-params");
                    
                    Arguments.Add("ha-params", value.Trim());
                }

                public void SetHighAvailabilitySyncMode(HighAvailabilitySyncModes mode)
                {
                    Validate("ha-sync-mode");
                    
                    Arguments.Add("ha-sync-mode", mode.ConvertTo());
                }

                public void SetMessageTimeToLive(long milliseconds)
                {
                    Validate("message-ttl");
                    
                    Arguments.Add("message-ttl", milliseconds);
                }

                public void SetMessageMaxSizeInBytes(long value)
                {
                    Validate("max-length-bytes");
                    
                    Arguments.Add("max-length-bytes", value.ToString());
                }

                public void SetMessageMaxSize(long value)
                {
                    Validate("max-length");
                    
                    Arguments.Add("max-length", value);
                }

                public void SetDeadLetterExchange(string value)
                {
                    Validate("dead-letter-exchange");
                    
                    Arguments.Add("dead-letter-exchange", value.Trim());
                }

                public void SetDeadLetterRoutingKey(string value)
                {
                    Validate("dead-letter-routing-key");
                    
                    Arguments.Add("dead-letter-routing-key", value.Trim());
                }

                public void SetQueueMode()
                {
                    Validate("queue-mode");
                    
                    Arguments.Add("queue-mode", "lazy");
                }

                public void SetAlternateExchange(string value)
                {
                    Validate("alternate-exchange");
                    
                    Arguments.Add("alternate-exchange", value.Trim());
                }

                void Validate(string arg)
                {
                    if (Arguments.ContainsKey(arg))
                        throw new PolicyDefinitionException($"Argument '{arg}' has already been set");
                }

                void ValidateConflictingArgs(string arg, string targetArg)
                {
                    if (Arguments.ContainsKey(arg) || (arg == targetArg && Arguments.ContainsKey(targetArg)))
                        throw new PolicyDefinitionException($"Argument '{arg}' has already been set or would conflict with argument '{targetArg}'");
                }

                void ValidateConflictingArgs(string arg, string targetArg, string conflictingArg)
                {
                    if (Arguments.ContainsKey(arg) ||
                        (arg == conflictingArg && Arguments.ContainsKey(targetArg)) ||
                        (arg == targetArg && Arguments.ContainsKey(conflictingArg)))
                        throw new PolicyDefinitionException($"Argument '{conflictingArg}' has already been set or would conflict with argument '{arg}'");
                }
            }

            
            class DefinedPolicyImpl :
                DefinedPolicy
            {
                public DefinedPolicyImpl(string pattern, IDictionary<string, object> arguments, int priority, string applyTo)
                {
                    Pattern = pattern;
                    Arguments = arguments;
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