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
namespace HareDu.Snapshotting.Model
{
    public interface NodeSnapshot :
        Snapshot
    {
        OperatingSystemDetails OS { get; }

        string RatesMode { get; }

        long Uptime { get; }

        int RunQueue { get; }

        long InterNodeHeartbeat { get; }

        string Name { get; }

        string Type { get; }

        bool IsRunning { get; }

        DiskSnapshot Disk { get; }
        
        IO IO { get; }
        
        ErlangDetails Erlang { get; }
        
        Mnesia Mnesia { get; }
        
        NodeMemoryDetails NodeMemory { get; }

        GarbageCollection GC { get; }

        ContextSwitchingDetails ContextSwitching { get; }
    }
}