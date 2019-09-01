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
    using Snapshotting.Model;

    public class FakeNodeSnapshot1 :
        NodeSnapshot
    {
        public FakeNodeSnapshot1(long availableSockets, long usedSockets, decimal socketUsageRate)
        {
            OS = new OperatingSystemDetailsImpl(availableSockets, usedSockets, socketUsageRate);
        }

        public OperatingSystemDetails OS { get; }
        public string RatesMode { get; }
        public long Uptime { get; }
        public int RunQueue { get; }
        public long InterNodeHeartbeat { get; }
        public string Name { get; }
        public string Type { get; }
        public bool IsRunning { get; }
        public DiskSnapshot Disk { get; }
        public IO IO { get; }
        public ErlangDetails Erlang { get; }
        public Mnesia Mnesia { get; }
        public NodeMemoryDetails NodeMemory { get; }
        public GarbageCollection GC { get; }
        public ContextSwitchingDetails ContextSwitching { get; }

        
        class OperatingSystemDetailsImpl :
            OperatingSystemDetails
        {
            public OperatingSystemDetailsImpl(long available, long used, decimal usageRate)
            {
                Sockets = new SocketChurnMetricsImpl(available, used, usageRate);
            }

            public string ProcessId { get; }
            public FileDescriptorChurnMetrics FileDescriptors { get; }
            public SocketChurnMetrics Sockets { get; }

            
            class SocketChurnMetricsImpl :
                SocketChurnMetrics
            {
                public SocketChurnMetricsImpl(long available, long used, decimal usageRate)
                {
                    Available = available;
                    Used = used;
                    UsageRate = usageRate;
                }

                public long Available { get; }
                public long Used { get; }
                public decimal UsageRate { get; }
            }
        }
    }
}