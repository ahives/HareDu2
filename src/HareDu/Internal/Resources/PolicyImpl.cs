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

        public async Task<Result> Create(string policy, string vhost, Action<PolicyDefinition> definition, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.RequestCanceled(LogInfo);

            if (string.IsNullOrWhiteSpace(policy))
                throw new PolicyMissingException("The name of the policy is missing.");

            if (string.IsNullOrWhiteSpace(vhost))
                throw new VirtualHostMissingException("The name of the virtual host is missing.");
            
            var impl = new PolicyDefinitionImpl();
            definition(impl);

            DefinedPolicy definedPolicy = impl.DefinedPolicy.Value;

            string sanitizedVHost = vhost.SanitizeVirtualHostName();

            string url = $"api/policies/{sanitizedVHost}/{policy}";

            LogInfo($"Sent request to RabbitMQ server to create a policy '{policy}' in virtual host '{sanitizedVHost}'.");

            HttpResponseMessage response = await HttpPut(url, definedPolicy, cancellationToken);
            Result result = response.GetResponse();

            return result;
        }

        public async Task<Result> Delete(string vhost, string policy, CancellationToken cancellationToken = new CancellationToken())
        {
            cancellationToken.RequestCanceled(LogInfo);

            if (string.IsNullOrWhiteSpace(policy))
                throw new PolicyMissingException("The name of the policy is missing.");

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

            public PolicyDefinitionImpl() => DefinedPolicy = new Lazy<DefinedPolicy>(Init, LazyThreadSafetyMode.PublicationOnly);

            DefinedPolicy Init() => new DefinedPolicyImpl(_pattern, _arguments, _priority, _applyTo);

            public void UsingPattern(string pattern) => _pattern = pattern;
            
            public void WithArguments(Action<PolicyDefinitionArguments> arguments)
            {
                var impl = new PolicyDefinitionArgumentsImpl();
                arguments(impl);

                _arguments = impl.Arguments;
            }

            public void Priority(int priority) => _priority = priority;

            public void AppliedTo(string applyTo) => _applyTo = applyTo;

            
            class PolicyDefinitionArgumentsImpl :
                PolicyDefinitionArguments
            {
                public IDictionary<string, object> Arguments { get; } = new Dictionary<string, object>();
                
                public void Set<T>(string arg, T value)
                {
                    Validate(arg.Trim(), "federation-upstream", "federation-upstream-set");
                    Validate("ha-mode");
                    
                    Arguments.Add(arg.Trim(), value);
                }

                public void SetExpiry(long milliseconds)
                {
                    Validate("expires");
                    
                    Arguments.Add("expires", milliseconds);
                }

                public void SetFederationUpstreamSet(string upstreamSet)
                {
                    Validate("federation-upstream-set", "federation-upstream");
                    
                    Arguments.Add("federation-upstream-set", upstreamSet.Trim());
                }

                public void SetFederationUpstream(string upstream)
                {
                    Validate("federation-upstream", "federation-upstream-set");
                    
                    Arguments.Add("federation-upstream", upstream.Trim());
                }

                public void SetHighAvailabilityMode(string mode)
                {
                    Validate("ha-mode");
                    
                    Arguments.Add("ha-mode", mode);
                }

                void Validate(string arg)
                {
                    if (Arguments.ContainsKey(arg))
                        throw new PolicyDefinitionException($"Argument '{arg}' has already been set");
                }

                void Validate(string arg, string targetArg)
                {
                    if (Arguments.ContainsKey(arg) && Arguments.ContainsKey(targetArg))
                        throw new PolicyDefinitionException($"Argument '{arg}' has already been set or would conflict with argument '{targetArg}'");
                }

                void Validate(string arg, string targetArg, string conflictingArg)
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