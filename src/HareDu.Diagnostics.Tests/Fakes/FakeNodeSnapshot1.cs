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
    using Snapshotting.Model;

    public class FakeNodeSnapshot1 :
        NodeSnapshot
    {
        public FakeNodeSnapshot1(ulong availableSockets, ulong usedSockets, decimal socketUsageRate)
        {
            OS = new OperatingSystemSnapshotImpl(availableSockets, usedSockets, socketUsageRate);
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
    }
}