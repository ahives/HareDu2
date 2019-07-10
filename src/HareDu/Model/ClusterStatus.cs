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
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using Core;
    using Core.Model;
    using Newtonsoft.Json;

    public interface ClusterStatus
    {
        string RabbitMqVersion { get; }
        
        string ClusterName { get; }
        
        IReadOnlyList<NodeStatus> Nodes { get; }
        
        QueueDetails Queue { get; }
    }

    public interface Mnesia
    {
        TransactionDetails Transactions { get; }
        
        IndexDetails Index { get; }
        
        StorageDetails Storage { get; }
    }

    public interface TransactionDetails
    {
        PersistenceDetails RAM { get; }
        
        PersistenceDetails Disk { get; }
    }

    public interface PersistenceDetails
    {
        long Total { get; }
        
        decimal Rate { get; }
    }

    public interface IndexDetails
    {
        IndexUsageDetails Reads { get; }
        
        IndexUsageDetails Writes { get; }
        
        JournalDetails Journal { get; }
    }

    public interface JournalDetails
    {
        IndexUsageDetails Writes { get; }
    }

    public interface IndexUsageDetails
    {
        long Total { get; }

        decimal Rate { get; }
    }

    public interface StorageDetails
    {
        MessageStoreDetails Reads { get; }
        
        MessageStoreDetails Writes { get; }
    }

    public interface MessageStoreDetails
    {
        long Total { get; }
        
        decimal Rate { get; }
    }

    public interface ErlangDetails
    {
        string Version { get; }
        
        long AvailableCPUCores { get; }

        ErlangProcessDetails Processes { get; }
    }

    public interface ErlangProcessDetails
    {
        long Used { get; }

        decimal UsageRate { get; }
    }

    public interface NodeStatus
    {
        OperatingSystemDetails OS { get; }

        string RatesMode { get; }

        long Uptime { get; }

        int RunQueue { get; }

        long InterNodeHeartbeat { get; }

        string Name { get; }

        string Type { get; }

        bool IsRunning { get; }

        IO IO { get; }
        
        ErlangDetails Erlang { get; }
        
        Mnesia Mnesia { get; }
        
        MemoryDetails Memory { get; }


        [JsonProperty("gc_num")]
        long NumberOfGarbageCollected { get; }

        [JsonProperty("gc_num_details")]
        GCDetails GcDetails { get; }

        [JsonProperty("gc_bytes_reclaimed")]
        long ReclaimedBytesFromGC { get; }

        [JsonProperty("gc_bytes_reclaimed_details")]
        ReclaimedBytesFromGCDetails ReclaimedBytesFromGCDetails { get; }

        ContextSwitchingDetails ContextSwitching { get; }
        
        [JsonProperty("context_switches")]
        long ContextSwitches { get; }

        [JsonProperty("context_switches_details")]
        ContextSwitchDetails ContextSwitchDetails { get; }



        [JsonProperty("metrics_gc_queue_length")]
        GarbageCollectionMetrics GarbageCollectionMetrics { get; }

    }

    public interface ContextSwitchingDetails
    {
        long Total { get; }
        
        decimal Rate { get; }
    }

    public interface MemoryDetails
    {
        long Used { get; }

        long Limit { get; }

        bool Alarm { get; }
    }

    public interface OperatingSystemDetails
    {
        string ProcessId { get; }

        FileDescriptorDetails FileDescriptors { get; }
        
        SocketDetails Sockets { get; }
    }

    public interface FileDescriptorDetails
    {
        long Available { get; }

        long Used { get; }

        decimal UsageRate { get; }

        FileDescriptorOpenAttempts FileDescriptorOpenAttempts { get; }
    }

    public interface FileDescriptorOpenAttempts
    {
        long OpenAttempts { get; }

        decimal OpenAttemptRate { get; }

        decimal OpenAttemptAvgTime { get; }

        decimal FileHandleOpenAttemptAvgTimeRate { get; }
    }

    public interface SocketDetails
    {
        long Available { get; }

        long Used { get; }

        decimal UsageRate { get; }
    }
}