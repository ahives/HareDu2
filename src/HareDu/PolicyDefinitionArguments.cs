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
namespace HareDu
{
    public interface PolicyDefinitionArguments
    {
        void Set<T>(string arg, T value);

        void SetExpiry(long milliseconds);

        void SetFederationUpstreamSet(string value);

        void SetFederationUpstream(string value);

        void SetHighAvailabilityMode(HighAvailabilityModes mode);

        void SetHighAvailabilityParams(string value);

        void SetHighAvailabilitySyncMode(HighAvailabilitySyncModes mode);

        void SetMessageTimeToLive(long milliseconds);

        void SetMessageMaxSizeInBytes(long value);

        void SetMessageMaxSize(long value);

        void SetDeadLetterExchange(string value);

        void SetDeadLetterRoutingKey(string value);

        void SetQueueMode();

        void SetAlternateExchange(string value);
    }
}