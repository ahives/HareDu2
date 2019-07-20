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
namespace HareDu.Core.Model
{
    using Newtonsoft.Json;

    public interface MemoryInfo
    {
        [JsonProperty("connection_readers")]
        long ConnectionReaders { get; }
        
        [JsonProperty("connection_writers")]
        long ConnectionWriters { get; }
        
        [JsonProperty("connection_channels")]
        long ConnectionChannels { get; }
        
        [JsonProperty("connection_other")]
        long ConnectionOther { get; }
        
        [JsonProperty("queue_procs")]
        long QueueProcesses { get; }
        
        [JsonProperty("queue_slave_procs")]
        long QueueSlaveProcesses { get; }
        
        [JsonProperty("plugins")]
        long Plugins { get; }
        
        [JsonProperty("other_proc")]
        long OtherProcesses { get; }
        
        [JsonProperty("metrics")]
        long Metrics { get; }
        
        [JsonProperty("mgmt_db")]
        long ManagementDatabase { get; }
        
        [JsonProperty("mnesia")]
        long Mnesia { get; }
        
        [JsonProperty("other_ets")]
        long OtherInMemoryStorage { get; }
        
        [JsonProperty("binary")]
        long Binary { get; }
        
        [JsonProperty("msg_index")]
        long MessageIndex { get; }
        
        [JsonProperty("code")]
        long ByteCode { get; }
        
        [JsonProperty("atom")]
        long Atom { get; }
        
        [JsonProperty("other_system")]
        long OtherSystem { get; }
        
        [JsonProperty("allocated_unused")]
        long AllocatedUnused { get; }
        
        [JsonProperty("reserved_unallocated")]
        long ReservedUnallocated { get; }
        
        [JsonProperty("strategy")]
        long Strategy { get; }
        
        [JsonProperty("total")]
        TotalMemoryInfo Total { get; }
    }
}