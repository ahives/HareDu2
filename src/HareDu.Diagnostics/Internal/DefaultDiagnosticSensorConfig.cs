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

    public class DefaultDiagnosticSensorConfig :
        DiagnosticSensorConfig
    {
        public DefaultDiagnosticSensorConfig()
        {
            Connection = new ConnectionSensorConfigImpl();
            Channel = new ChannelSensorConfigImpl();
            Queue = new QueueSensorConfigImpl();
            Node = new NodeSensorConfigImpl();
        }

        public ConnectionSensorConfig Connection { get; }
        public ChannelSensorConfig Channel { get; }
        public QueueSensorConfig Queue { get; }
        public NodeSensorConfig Node { get; }

        
        class NodeSensorConfigImpl :
            NodeSensorConfig
        {
            public NodeSensorConfigImpl()
            {
                SocketUsageCoefficient = 0.50M;
            }

            public decimal SocketUsageCoefficient { get; }
        }


        class ChannelSensorConfigImpl :
            ChannelSensorConfig
        {
        }

        
        class QueueSensorConfigImpl :
            QueueSensorConfig
        {
            public QueueSensorConfigImpl()
            {
                MessageRedeliveryCoefficient = 0.50M;
            }

            public decimal MessageRedeliveryCoefficient { get; }
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