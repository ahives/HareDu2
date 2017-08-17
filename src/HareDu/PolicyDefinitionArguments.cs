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
        void Define<T>(string arg, T value);

        void DefineExpiry(long milliseconds);

        void DefineFederationUpstreamSet(string value);

        void DefineFederationUpstream(string value);

        void DefineHighAvailabilityMode(HighAvailabilityModes mode);

        void DefineHighAvailabilityParams(string value);

        void DefineHighAvailabilitySyncMode(HighAvailabilitySyncModes mode);

        void DefineMessageTimeToLive(long milliseconds);

        void DefineMessageMaxSizeInBytes(long value);

        void DefineMessageMaxSize(long value);

        void DefineDeadLetterExchange(string value);

        void DefineDeadLetterRoutingKey(string value);

        void DefineQueueMode();

        void DefineAlternateExchange(string value);
    }
}