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
namespace HareDu.Diagnostics.Tests.Fakes
{
    using System.Collections.Generic;
    using System.Linq;
    using Snapshotting.Model;

    public class FakeClusterSnapshotSnapshot1 :
        ClusterSnapshot
    {
        public FakeClusterSnapshotSnapshot1()
        {
            Nodes = GetNodes().ToList();
        }

        IEnumerable<NodeSnapshot> GetNodes()
        {
            yield return new FakeNodeSnapshot();
        }

        public string RabbitMqVersion { get; }
        public string ClusterName { get; }
        public IReadOnlyList<NodeSnapshot> Nodes { get; }

        
        class FakeNodeSnapshot :
            NodeSnapshot
        {
            public FakeNodeSnapshot()
            {
                NetworkPartitions = new List<string>
                {
                    "node1@rabbitmq",
                    "node2@rabbitmq",
                    "node3@rabbitmq"
                };
                Runtime = new FakeBrokerRuntimeSnapshot(38, 36, 5.3M);
                Disk = new FakeDiskSnapshot(8, true, 5.5M);
                Memory = new FakeMemorySnapshot(273, 270, true);
                OS = new OperatingSystemSnapshotImpl(100, 90, 5.5M);
                AvailableCoresDetected = 1;
            }

            public OperatingSystemSnapshot OS { get; }
            public string RatesMode { get; }
            public long Uptime { get; }
            public int RunQueue { get; }
            public long InterNodeHeartbeat { get; }
            public string Identifier { get; }
            public string ClusterIdentifier { get; }
            public string Type { get; }
            public bool IsRunning { get; }
            public long AvailableCoresDetected { get; }
            public IList<string> NetworkPartitions { get; }
            public DiskSnapshot Disk { get; }
            public IO IO { get; }
            public BrokerRuntimeSnapshot Runtime { get; }
            public Mnesia Mnesia { get; }
            public MemorySnapshot Memory { get; }
            public GarbageCollection GC { get; }
            public ContextSwitchingDetails ContextSwitching { get; }
        }

        
        class OperatingSystemSnapshotImpl :
            OperatingSystemSnapshot
        {
            public OperatingSystemSnapshotImpl(ulong available, ulong used, decimal usageRate)
            {
                SocketDescriptors = new SocketDescriptorChurnMetricsImpl(available, used, usageRate);
            }

            public string NodeIdentifier { get; }
            public string ProcessId { get; }
            public FileDescriptorChurnMetrics FileDescriptors { get; }
            public SocketDescriptorChurnMetrics SocketDescriptors { get; }

            
            class SocketDescriptorChurnMetricsImpl :
                SocketDescriptorChurnMetrics
            {
                public SocketDescriptorChurnMetricsImpl(ulong available, ulong used, decimal usageRate)
                {
                    Available = available;
                    Used = used;
                    UsageRate = usageRate;
                }

                public ulong Available { get; }
                public ulong Used { get; }
                public decimal UsageRate { get; }
            }
        }


        class FakeMemorySnapshot :
            MemorySnapshot
        {
            public FakeMemorySnapshot(ulong used, ulong limit, bool alarmInEffect)
            {
                Used = used;
                Limit = limit;
                AlarmInEffect = alarmInEffect;
            }

            public string NodeIdentifier { get; }
            public ulong Used { get; }
            public decimal UsageRate { get; }
            public ulong Limit { get; }
            public bool AlarmInEffect { get; }
        }

        
        class FakeBrokerRuntimeSnapshot :
            BrokerRuntimeSnapshot
        {
            public FakeBrokerRuntimeSnapshot(ulong limit, ulong used, decimal usageRate)
            {
                Processes = new RuntimeProcessChurnMetricsImpl(limit, used, usageRate);
            }

            public string Identifier { get; }
            public string ClusterIdentifier { get; }
            public string Version { get; }
            public RuntimeProcessChurnMetrics Processes { get; }

        
            class RuntimeProcessChurnMetricsImpl :
                RuntimeProcessChurnMetrics
            {
                public RuntimeProcessChurnMetricsImpl(ulong limit, ulong used, decimal usageRate)
                {
                    Limit = limit;
                    Used = used;
                    UsageRate = usageRate;
                }

                public ulong Limit { get; }
                public ulong Used { get; }
                public decimal UsageRate { get; }
            }
        }

        
        class FakeDiskSnapshot :
            DiskSnapshot
        {
            public FakeDiskSnapshot(ulong available, bool alarmInEffect, decimal rate)
            {
                AlarmInEffect = alarmInEffect;
                Capacity = new DiskCapacityDetailsImpl(available, rate);
            }

            public string NodeIdentifier { get; }
            public DiskCapacityDetails Capacity { get; }
            public ulong Limit { get; }
            public bool AlarmInEffect { get; }

        
            class DiskCapacityDetailsImpl :
                DiskCapacityDetails
            {
                public DiskCapacityDetailsImpl(ulong available, decimal rate)
                {
                    Available = available;
                    Rate = rate;
                }

                public ulong Available { get; }
                public decimal Rate { get; }
            }
        }
    }
}