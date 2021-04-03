namespace HareDu.Diagnostics.KnowledgeBase
{
    using Probes;

    public class KnowledgeBaseProvider :
        BaseKnowledgeBaseProvider
    {
        protected override void Load()
        {
            Add<MessagePagingProbe>(ProbeResultStatus.Unhealthy,
                "Broker is nearing RAM high watermark and has paged messages to disk to prevent publishers from being blocked.",
                "Increase the amount of RAM available to the broker and configure the broker with the new watermark value.");
            Add<MessagePagingProbe>(ProbeResultStatus.Healthy,
                "RAM used by broker is less than high watermark.", null);
            
            Add<QueueNoFlowProbe>(ProbeResultStatus.Unhealthy,
                "There are no messages being published to the specified queue.", null);
            Add<QueueNoFlowProbe>(ProbeResultStatus.Healthy,
                "One or more messages have being published to the specified queue.", null);
            
            Add<QueueHighFlowProbe>(ProbeResultStatus.Unhealthy,
                "Messages being published to broker exceeds the specified threshold.", null);
            Add<QueueHighFlowProbe>(ProbeResultStatus.Healthy,
                "Messages being published does not exceed the specified threshold.", null);
            Add<QueueHighFlowProbe>(ProbeResultStatus.NA,
                "Configuration is missing.",
                "Either programatically configure the API using the DI extension method or via a JSON file.");
            
            Add<QueueLowFlowProbe>(ProbeResultStatus.Unhealthy,
                "Messages being published to broker is less or equal to the specified threshold.", null);
            Add<QueueLowFlowProbe>(ProbeResultStatus.Healthy,
                "Messages being published to broker is greater than specified threshold.", null);
            Add<QueueLowFlowProbe>(ProbeResultStatus.NA,
                "Configuration is missing.",
                "Either programatically configure the API using the DI extension method or via a JSON file.");
            
            Add<UnroutableMessageProbe>(ProbeResultStatus.Unhealthy,
            "Some messages were published to an exchange but there is no queue bound to the exchange.",
            "Bind an appropriate queue to the exchange or stop publishing to the exchange.");
            Add<UnroutableMessageProbe>(ProbeResultStatus.Healthy,
                "No exchanges were published to that is not bound to a corresponding queue.", null);
            
            Add<ConsumerUtilizationProbe>(ProbeResultStatus.Warning,
            "The queue is not able to push messages to consumers efficiently due to network congestion and/or the prefetch limit on the consumer being set too low.",
            "Check your network connection between the consumer and RabbitMQ node and/or readjust the prefetch limit.");
            Add<ConsumerUtilizationProbe>(ProbeResultStatus.Unhealthy,
            "The queue is not able to push messages to consumers efficiently due to network congestion and/or the prefetch limit on the consumer being set too low.",
            "Check your network connection between the consumer and RabbitMQ node and/or readjust the prefetch limit.");
            Add<ConsumerUtilizationProbe>(ProbeResultStatus.Healthy, "The queue is able to efficiently push messages to consumers as fast as possible without delay.", null);
            
            Add<BlockedConnectionProbe>(ProbeResultStatus.Unhealthy,
            "The connection is blocked meaning that an application has published but is now prevented from consuming. This is not the case with consume only connections.", null);
            Add<BlockedConnectionProbe>(ProbeResultStatus.Healthy,
                "Client applications are able to publish and/or consume on this connection.", null);
            
            Add<FileDescriptorThrottlingProbe>(ProbeResultStatus.Unhealthy,
            "The max limit of available file descriptors that are in use has been reached. This will prevent applications from being able to open more connections to the broker and the RabbitMQ node from opening any files in support of current transactions.",
            "Increase the max number of allowed file handles (see https://www.rabbitmq.com/networking.html#open-file-handle-limit).");
            Add<FileDescriptorThrottlingProbe>(ProbeResultStatus.Warning,
                "Used file descriptors are less than the amount available but greater than the calculated threshold.",
                "Increase the max number of allowed file handles (see https://www.rabbitmq.com/networking.html#open-file-handle-limit).");
            Add<FileDescriptorThrottlingProbe>(ProbeResultStatus.Healthy,
                "The number of file handles currently in use is below the max number allowed.", null);
            Add<FileDescriptorThrottlingProbe>(ProbeResultStatus.NA,
                "Configuration is missing.",
                "Either programatically configure the API using the DI extension method or via a JSON file.");
            
            Add<AvailableCpuCoresProbe>(ProbeResultStatus.Unhealthy,
                "Could not detect any CPU cores or none are available to the RabbitMQ broker.",
                "Add more CPU cores to the RabbitMQ node.");
            Add<AvailableCpuCoresProbe>(ProbeResultStatus.Healthy,
                "Detected at least 1 CPU core available to the RabbitMQ broker.", null);
            
            Add<NetworkPartitionProbe>(ProbeResultStatus.Unhealthy,
            "Network partitions detected between one or more nodes.",
            "Please consult the RabbitMQ documentation (https://www.rabbitmq.com/partitions.html) on which strategy best fits your scenario.");
            Add<NetworkPartitionProbe>(ProbeResultStatus.Healthy,
                "No network partitions were detected between nodes.", null);
            
            Add<RuntimeProcessLimitProbe>(ProbeResultStatus.Unhealthy,
            "The number of Erlang runtime processes in use is greater than or equal to the max number available.", null);
            Add<RuntimeProcessLimitProbe>(ProbeResultStatus.Healthy, "The number of Erlang runtime processes in use is less than the max number available.", null);
            Add<RuntimeProcessLimitProbe>(ProbeResultStatus.NA,
                "Configuration is missing.",
                "Either programatically configure the API using the DI extension method or via a JSON file.");
            
            Add<MemoryAlarmProbe>(ProbeResultStatus.Unhealthy,
            "The threshold was reached for how much RAM can be used by the RabbitMQ Broker.",
            "Do one or a combination of the following:\n1) Increase the threshold of available RAM by changing either the vm_memory_high_watermark.absolute or vm_memory_high_watermark.relative broker configuration values.\n2) Spawn more consumers so that messages are not held in RAM for long periods.\n3) Increase the cluster hardware specification by adding more RAM.");
            Add<MemoryAlarmProbe>(ProbeResultStatus.Healthy,
                "The amount of RAM used is less than the current threshold (i.e. vm_memory_high_watermark.absolute or vm_memory_high_watermark.relative).", null);
            
            Add<DiskAlarmProbe>(ProbeResultStatus.Unhealthy,
            "The node has reached the threshold for usable disk space.",
            "Increase message consumption throughput by spawning more consumers and/or increase disk size to keep up with incoming demand.");
            Add<DiskAlarmProbe>(ProbeResultStatus.Healthy,
                "The node is under the allowable threshold for usable disk space.", null);
            
            Add<SocketDescriptorThrottlingProbe>(ProbeResultStatus.Warning,
            "The number of network sockets being used is greater than the calculated high watermark but less than max number available.", null);
            Add<SocketDescriptorThrottlingProbe>(ProbeResultStatus.Unhealthy,
            "The number of network sockets being used is equal to the max number available.", null);
            Add<SocketDescriptorThrottlingProbe>(ProbeResultStatus.Inconclusive,
            "Either the sensor was not configured correctly or there was no data captured to analyze.",
            "Check the sensor configuration and the resultant snapshot data.");
            Add<SocketDescriptorThrottlingProbe>(ProbeResultStatus.Healthy, "The number of network sockets used is less than the calculated high watermark.", null);
            Add<SocketDescriptorThrottlingProbe>(ProbeResultStatus.NA,
                "Configuration is missing.",
                "Either programatically configure the API using the DI extension method or via a JSON file.");
            
            Add<RedeliveredMessagesProbe>(ProbeResultStatus.Warning,
            "The number of redelivered messages is less than or equal to the number of incoming messages and greater than or equal to the number of incoming messages multiplied a configurable coefficient.", null);
            Add<RedeliveredMessagesProbe>(ProbeResultStatus.Unhealthy,
            "The number of redelivered messages is less than or equal to the number of incoming messages.", "");
            Add<RedeliveredMessagesProbe>(ProbeResultStatus.Healthy, "", null);
            Add<RedeliveredMessagesProbe>(ProbeResultStatus.NA,
                "Configuration is missing.",
                "Either programatically configure the API using the DI extension method or via a JSON file.");
            
            Add<QueueGrowthProbe>(ProbeResultStatus.Warning,
            "Messages are being published to the queue at a higher rate than are being consumed and acknowledged by consumers.",
            "Adjust application settings to spawn more consumers.");
            Add<QueueGrowthProbe>(ProbeResultStatus.Healthy,
                "Messages are being consumed and acknowledged at a higher rate than are being published to the queue.", null);
            
            Add<ChannelLimitReachedProbe>(ProbeResultStatus.Unhealthy,
            "Number of channels on connection exceeds the defined limit.",
            "Adjust application settings to reduce the number of connections to the RabbitMQ broker.");
            Add<ChannelLimitReachedProbe>(ProbeResultStatus.Healthy,
            "Number of channels on connection is less than defined limit.", null);
            
            Add<ChannelThrottlingProbe>(ProbeResultStatus.Unhealthy,
            "Unacknowledged messages on channel exceeds prefetch count causing the RabbitMQ broker to stop delivering messages to consumers.",
            "Acknowledged messages must be greater than or equal to the result of subtracting the number of unacknowledged messages from the prefetch count plus 1. Temporarily increase the number of consumers or prefetch count.");
            Add<ChannelThrottlingProbe>(ProbeResultStatus.Healthy,
            "Unacknowledged messages on channel is less than prefetch count.", null);
            
            Add<HighConnectionClosureRateProbe>(ProbeResultStatus.Warning,
                "The rate at which connections to the broker are closed is greater than the specified threshold.",
                "");
            Add<HighConnectionClosureRateProbe>(ProbeResultStatus.Healthy,
                "The rate at which connections to the broker are being closed is less than the specified threshold.", null);
            Add<HighConnectionClosureRateProbe>(ProbeResultStatus.NA,
                "Configuration is missing.",
                "Either programatically configure the API using the DI extension method or via a JSON file.");
            
            Add<HighConnectionCreationRateProbe>(ProbeResultStatus.Warning,
                "The rate at which connections to the broker are created is greater than or equal to the specified threshold.", "");
            Add<HighConnectionCreationRateProbe>(ProbeResultStatus.Healthy,
                "The rate at which connections to the broker are created is less than the specified threshold.", null);
            Add<HighConnectionCreationRateProbe>(ProbeResultStatus.NA,
                "Configuration is missing.",
                "Either programatically configure the API using the DI extension method or via a JSON file.");
            
            Add<UnlimitedPrefetchCountProbe>(ProbeResultStatus.Warning,
            "Prefetch count of 0 means unlimited prefetch count, which can translate into high CPU utilization.",
            "Set a prefetch count above zero based on how many consumer cores available.");
            Add<UnlimitedPrefetchCountProbe>(ProbeResultStatus.Inconclusive,
                "Unable to determine whether prefetch count has an inappropriate value.", null);
        }
    }
}