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
    using HareDu.Model;
    using Newtonsoft.Json;

    public interface GarbageCollection
    {
        [JsonProperty("gc_num")]
        long NumberOfGarbageCollected { get; }

        [JsonProperty("gc_num_details")]
        GCDetails GcDetails { get; }

        [JsonProperty("gc_bytes_reclaimed")]
        long ReclaimedBytesFromGC { get; }

        [JsonProperty("gc_bytes_reclaimed_details")]
        ReclaimedBytesFromGCDetails ReclaimedBytesFromGCDetails { get; }

        [JsonProperty("metrics_gc_queue_length")]
        GarbageCollectionMetrics GarbageCollectionMetrics { get; }
    }
}