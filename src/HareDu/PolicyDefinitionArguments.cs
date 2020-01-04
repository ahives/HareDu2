// Copyright 2013-2020 Albert L. Hives
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
namespace HareDu
{
    public interface PolicyDefinitionArguments
    {
        /// <summary>
        /// Set user-defined argument on the policy.
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        void Set<T>(string arg, T value);

        /// <summary>
        /// Set 'expires' argument on the policy.
        /// </summary>
        /// <param name="milliseconds"></param>
        void SetExpiry(long milliseconds);

        /// <summary>
        /// Set 'federation-upstream-set' argument on the policy.
        /// </summary>
        /// <param name="value"></param>
        void SetFederationUpstreamSet(string value);

        /// <summary>
        /// Set 'federation-upstream' argument on the policy.
        /// </summary>
        /// <param name="value"></param>
        void SetFederationUpstream(string value);

        /// <summary>
        /// Set 'ha-mode' argument on the policy.
        /// </summary>
        /// <param name="mode"></param>
        void SetHighAvailabilityMode(HighAvailabilityModes mode);

        /// <summary>
        /// Set 'ha-params' argument on the policy.
        /// </summary>
        /// <param name="value"></param>
        void SetHighAvailabilityParams(string value);

        /// <summary>
        /// Set 'ha-sync-mode' argument on the policy.
        /// </summary>
        /// <param name="mode"></param>
        void SetHighAvailabilitySyncMode(HighAvailabilitySyncModes mode);

        /// <summary>
        /// Set 'message-ttl' argument on the policy.
        /// </summary>
        /// <param name="milliseconds"></param>
        void SetMessageTimeToLive(long milliseconds);

        /// <summary>
        /// Set 'max-length-bytes' argument on the policy.
        /// </summary>
        /// <param name="value"></param>
        void SetMessageMaxSizeInBytes(long value);

        /// <summary>
        /// Set 'max-length' argument on the policy.
        /// </summary>
        /// <param name="value"></param>
        void SetMessageMaxSize(long value);

        /// <summary>
        /// Set 'dead-letter-exchange' argument on the policy.
        /// </summary>
        /// <param name="value"></param>
        void SetDeadLetterExchange(string value);

        /// <summary>
        /// Set 'dead-letter-routing-key' argument on the policy.
        /// </summary>
        /// <param name="value"></param>
        void SetDeadLetterRoutingKey(string value);

        /// <summary>
        /// Set 'queue-mode' argument on the policy.
        /// </summary>
        void SetQueueMode();

        /// <summary>
        /// Set 'alternate-exchange' argument on the policy.
        /// </summary>
        /// <param name="value"></param>
        void SetAlternateExchange(string value);
    }
}