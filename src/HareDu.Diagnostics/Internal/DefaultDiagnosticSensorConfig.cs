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
namespace HareDu.Diagnostics.Internal
{
    using Configuration;

    class DefaultDiagnosticSensorConfig :
        DiagnosticSensorConfig
    {
        public DefaultDiagnosticSensorConfig()
        {
            ConnectionSensorConfig = new ConnectionSensorConfigImpl();
            ChannelSensorConfig = new ChannelSensorConfigImpl();
        }

        public ConnectionSensorConfig ConnectionSensorConfig { get; }
        public ChannelSensorConfig ChannelSensorConfig { get; }

        
        class ChannelSensorConfigImpl :
            ChannelSensorConfig
        {
        }

        class ConnectionSensorConfigImpl :
            ConnectionSensorConfig
        {
            public ConnectionSensorConfigImpl()
            {
                HighClosureRateThreshold = 100;
                HighCreationRateThreshold = 100;
            }

            public int HighClosureRateThreshold { get; }
            public int HighCreationRateThreshold { get; }
        }
    }
}