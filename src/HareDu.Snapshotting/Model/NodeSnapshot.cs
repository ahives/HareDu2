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
namespace HareDu.Snapshotting.Model
{
    using System.Collections.Generic;

    public interface NodeSnapshot :
        Snapshot
    {
        OperatingSystemSnapshot OS { get; }

        string RatesMode { get; }

        long Uptime { get; }
        
        long InterNodeHeartbeat { get; }

        string Identifier { get; }
        
        string ClusterIdentifier { get; }

        string Type { get; }

        bool IsRunning { get; }
        
        ulong AvailableCoresDetected { get; }

        IReadOnlyList<string> NetworkPartitions { get; }
        
        DiskSnapshot Disk { get; }
        
        BrokerRuntimeSnapshot Runtime { get; }
        
        MemorySnapshot Memory { get; }

        ContextSwitchingDetails ContextSwitching { get; }
    }
}