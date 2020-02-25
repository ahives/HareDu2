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
namespace HareDu.Core.Configuration
{
    using Extensions;

    public class HareDuConfigValidator :
        IConfigValidator
    {
        public bool Validate(HareDuConfig config)
        {
            if (config.IsNull() ||
                config.Broker.Credentials.IsNull() ||
                string.IsNullOrWhiteSpace(config.Broker.Credentials.Username) ||
                string.IsNullOrWhiteSpace(config.Broker.Credentials.Password) ||
                string.IsNullOrWhiteSpace(config.Broker.Url))
            {
                return false;
            }

            return true;
        }
    }
}