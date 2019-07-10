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
namespace HareDu.Model
{
    using Core.Model;
    using Newtonsoft.Json;

    public interface IO
    {
        DiskDetails Disk { get; }
        
        DiskUsageDetails Reads { get; }
        
        DiskUsageDetails Writes { get; }
        
        DiskUsageDetails Seeks { get; }

        [JsonProperty("io_sync_count")]
        long IOSyncCount { get; }

        [JsonProperty("io_sync_count_details")]
        IOSyncCountDetails RateOfIOSyncs { get; }

        [JsonProperty("io_sync_avg_time")]
        decimal AverageIOSyncTime { get; }

        [JsonProperty("io_sync_avg_time_details")]
        AvgIOSyncTimeDetails AvgIOSyncTimeDetails { get; }

        [JsonProperty("io_seek_count")]
        long IOSeekCount { get; }

        [JsonProperty("io_seek_count_details")]
        IOSeekCountDetails RateOfIOSeeks { get; }

        [JsonProperty("io_seek_avg_time")]
        decimal AverageIOSeekTime { get; }

        [JsonProperty("io_seek_avg_time_details")]
        AvgIOSeekTimeDetails AvgIOSeekTimeDetails { get; }

        [JsonProperty("io_reopen_count")]
        long IOReopenCount { get; }

        [JsonProperty("io_reopen_count_details")]
        IOReopenCountDetails RateOfIOReopened { get; }
    }

    public interface DiskDetails
    {
        DiskCapacityDetails Capacity { get; }

        string FreeLimit { get; }

        bool FreeAlarm { get; }
    }

    public interface DiskCapacityDetails
    {
        long Available { get; }
        
        decimal Rate { get; }
    }
}