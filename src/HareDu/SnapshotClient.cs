// Copyright 2013-2019 Albert L. Hives
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
    using System;
    using Core;
    using Core.Configuration;
    using Core.Exceptions;
    using Internal;

    public static class SnapshotClient
    {
        public static ISnapshotFactory Init(Action<ClientConfigProvider> configuration)
        {
            if (configuration == null)
                throw new HareDuClientConfigurationException("Settings cannot be null and should at least have user credentials, RabbitMQ server URL and port.");

            try
            {
                var resourceFactory = ResourceClient.Init(configuration);
                
                SnapshotFactory.Instance.Init(resourceFactory);

                return SnapshotFactory.Instance;
            }
            catch (Exception e)
            {
                throw new HareDuClientInitException("Unable to initialize the HareDu client.", e);
            }
        }
    }
}