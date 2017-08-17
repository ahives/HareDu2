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

    internal class PolicyImpl :
        ResourceBase,
        Policy
    {
        public PolicyImpl(HttpClient client, HareDuClientSettings settings)
            : base(client, settings)
        {
        }

        public async Task<Result<IEnumerable<PolicyInfo>>> GetAll(CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);

            string url = $"api/policies";

            LogInfo($"Sent request to return all policy information on current RabbitMQ server.");

            HttpResponseMessage response = await HttpGet(url, cancellationToken);
            Result<IEnumerable<PolicyInfo>> result = await response.GetResponse<IEnumerable<PolicyInfo>>();

            return result;
        }

        public async Task<Result> Create(Action<PolicyDefinition> definition, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.RequestCanceled(LogInfo);

            var impl = new PolicyDefinitionImpl();
            definition(impl);

            DefinedPolicy policy = impl.DefinedPolicy.Value;

            string sanitizedVHost = impl.VirtualHost.SanitizeVirtualHostName();

            string url = $"api/policies/{sanitizedVHost}/{impl.PolicyName}";

            LogInfo($"Sent request to RabbitMQ server to create a policy '{impl.PolicyName}' in virtual host '{sanitizedVHost}'.");

            HttpResponseMessage response = await HttpPut(url, policy, cancellationToken);
            Result result = response.GetResponse();

            return result;
        }

        public async Task<Result> Delete(string policy, string vhost, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.RequestCanceled(LogInfo);

            if (string.IsNullOrWhiteSpace(policy))
                throw new PolicyNameMissingException("The name of the policy is missing.");

            if (string.IsNullOrWhiteSpace(vhost))
                throw new VirtualHostMissingException("The name of the virtual host is missing.");

            string sanitizedVHost = vhost.SanitizeVirtualHostName();

            string url = $"api/policies/{sanitizedVHost}/{policy}";

            LogInfo($"Sent request to RabbitMQ server to create a policy '{policy}' in virtual host '{sanitizedVHost}'.");

            HttpResponseMessage response = await HttpDelete(url, cancellationToken);
            Result result = response.GetResponse();

            return result;
        }

        
        class PolicyDefinitionImpl :
            PolicyDefinition
        {
            static string _pattern;
            static IDictionary<string, object> _arguments;
            static int _priority;
            static string _applyTo;
            
            public Lazy<DefinedPolicy> DefinedPolicy { get; }
            public string VirtualHost { get; private set; }
            public string PolicyName { get; private set; }

            public PolicyDefinitionImpl() => DefinedPolicy = new Lazy<DefinedPolicy>(Init, LazyThreadSafetyMode.PublicationOnly);

            DefinedPolicy Init() => new DefinedPolicyImpl(_pattern, _arguments, _priority, _applyTo);

            public void Policy(Action<PolicyConfiguration> definition)
            {
                var impl = new PolicyConfigurationImpl();
                definition(impl);

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
                    if (((mode.ConvertTo() == HighAvailabilityModes.Exactly) || (mode.ConvertTo() == HighAvailabilityModes.Nodes)) && !_arguments.ContainsKey("ha-params"))
                        throw new PolicyDefinitionException(
                            $"Argument 'ha-mode' has been set to {mode}, which means that argument 'ha-params' has to also be set");
                }

                if (string.IsNullOrWhiteSpace(impl.PolicyName))
                    throw new PolicyNameMissingException("The name of the policy is missing.");

                PolicyName = impl.PolicyName;
            }

            public void On(string vhost)
            {
                if (string.IsNullOrWhiteSpace(vhost))
                    throw new VirtualHostMissingException("The name of the virtual host is missing.");
            
                VirtualHost = vhost;
            }


            class PolicyConfigurationImpl :
                PolicyConfiguration
            {
                public string PolicyName { get; private set; }
                public int Priority { get; private set; }
                public string Pattern { get; private set; }
                public string AppllyTo { get; private set; }
                public IDictionary<string, object> Arguments { get; private set; }
                
                public void Name(string name) => PolicyName = name;

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
                
                public void Define<T>(string arg, T value)
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

                public void DefineExpiry(long milliseconds)
                {
                    Validate("expires");
                    
                    Arguments.Add("expires", milliseconds);
                }

                public void DefineFederationUpstreamSet(string value)
                {
                    ValidateConflictingArgs("federation-upstream-set", "federation-upstream");
                    
                    Arguments.Add("federation-upstream-set", value.Trim());
                }

                public void DefineFederationUpstream(string value)
                {
                    ValidateConflictingArgs("federation-upstream", "federation-upstream-set");
                    
                    Arguments.Add("federation-upstream", value.Trim());
                }

                public void DefineHighAvailabilityMode(HighAvailabilityModes mode)
                {
                    Validate("ha-mode");
                    
                    Arguments.Add("ha-mode", mode.ConvertTo());
                }

                public void DefineHighAvailabilityParams(string value)
                {
                    Validate("ha-params");
                    
                    Arguments.Add("ha-params", value.Trim());
                }

                public void DefineHighAvailabilitySyncMode(HighAvailabilitySyncModes mode)
                {
                    Validate("ha-sync-mode");
                    
                    Arguments.Add("ha-sync-mode", mode.ConvertTo());
                }

                public void DefineMessageTimeToLive(long milliseconds)
                {
                    Validate("message-ttl");
                    
                    Arguments.Add("message-ttl", milliseconds);
                }

                public void DefineMessageMaxSizeInBytes(long value)
                {
                    Validate("max-length-bytes");
                    
                    Arguments.Add("max-length-bytes", value.ToString());
                }

                public void DefineMessageMaxSize(long value)
                {
                    Validate("max-length");
                    
                    Arguments.Add("max-length", value);
                }

                public void DefineDeadLetterExchange(string value)
                {
                    Validate("dead-letter-exchange");
                    
                    Arguments.Add("dead-letter-exchange", value.Trim());
                }

                public void DefineDeadLetterRoutingKey(string value)
                {
                    Validate("dead-letter-routing-key");
                    
                    Arguments.Add("dead-letter-routing-key", value.Trim());
                }

                public void DefineQueueMode()
                {
                    Validate("queue-mode");
                    
                    Arguments.Add("queue-mode", "lazy");
                }

                public void DefineAlternateExchange(string value)
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