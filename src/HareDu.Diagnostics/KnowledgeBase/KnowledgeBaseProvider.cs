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
namespace HareDu.Diagnostics.KnowledgeBase
{
    using Core.Extensions;
    using Probes;

    public class KnowledgeBaseProvider :
        BaseKnowledgeBaseProvider
    {
        protected override void Load()
        {
            _articles.Add(new KnowledgeBaseArticleImpl<MessagePagingProbe>(DiagnosticStatus.Unhealthy, "", ""));
            _articles.Add(new KnowledgeBaseArticleImpl<MessagePagingProbe>(DiagnosticStatus.Healthy, ""));

            _articles.Add(new KnowledgeBaseArticleImpl<QueueNoFlowProbe>(DiagnosticStatus.Unhealthy,
                "There are no messages being published to the queue", ""));
            _articles.Add(new KnowledgeBaseArticleImpl<QueueNoFlowProbe>(DiagnosticStatus.Healthy, ""));
            
            _articles.Add(new KnowledgeBaseArticleImpl<QueueHighFlowProbe>(DiagnosticStatus.Unhealthy, "", ""));
            _articles.Add(new KnowledgeBaseArticleImpl<QueueHighFlowProbe>(DiagnosticStatus.Healthy, ""));
            
            _articles.Add(new KnowledgeBaseArticleImpl<QueueLowFlowProbe>(DiagnosticStatus.Unhealthy, "", ""));
            _articles.Add(new KnowledgeBaseArticleImpl<QueueLowFlowProbe>(DiagnosticStatus.Healthy, ""));
            
            _articles.Add(new KnowledgeBaseArticleImpl<UnroutableMessageProbe>(DiagnosticStatus.Unhealthy,
                "Some messages were published to an exchange but there is no queue bound to the exchange.",
                "Bind an appropriate queue to the exchange or stop publishing to the exchange."));
            _articles.Add(new KnowledgeBaseArticleImpl<UnroutableMessageProbe>(DiagnosticStatus.Healthy, "No exchanges were published to that is not bound to a corresponding queue."));
            
            _articles.Add(new KnowledgeBaseArticleImpl<ConsumerUtilizationProbe>(DiagnosticStatus.Warning,
                "The queue is not able to push messages to consumers efficiently due to network congestion and/or the prefetch limit on the consumer being set too low.",
                "Check your network connection between the consumer and RabbitMQ node and/or readjust the prefetch limit."));
            _articles.Add(new KnowledgeBaseArticleImpl<ConsumerUtilizationProbe>(DiagnosticStatus.Unhealthy,
                "The queue is not able to push messages to consumers efficiently due to network congestion and/or the prefetch limit on the consumer being set too low.",
                "Check your network connection between the consumer and RabbitMQ node and/or readjust the prefetch limit."));
            _articles.Add(new KnowledgeBaseArticleImpl<ConsumerUtilizationProbe>(DiagnosticStatus.Healthy, "The queue is able to efficiently push messages to consumers as fast as possible without delay."));
            
            _articles.Add(new KnowledgeBaseArticleImpl<BlockedConnectionProbe>(DiagnosticStatus.Unhealthy,
                "The connection is blocked meaning that an application has published but is now prevented from consuming. This is not the case with consume only connections.",
                ""));
            _articles.Add(new KnowledgeBaseArticleImpl<BlockedConnectionProbe>(DiagnosticStatus.Healthy, "Client applications are able to publish and/or consume on this connection."));
            
            _articles.Add(new KnowledgeBaseArticleImpl<FileDescriptorThrottlingProbe>(DiagnosticStatus.Unhealthy,
                "The max limit of available file descriptors that are in use has been reached. This will prevent applications from being able to open more connections to the broker and the RabbitMQ node from opening any files in support of current transactions.",
                "Increase the max number of allowed file handles (see https://www.rabbitmq.com/networking.html#open-file-handle-limit)."));
            _articles.Add(new KnowledgeBaseArticleImpl<FileDescriptorThrottlingProbe>(DiagnosticStatus.Warning, "", ""));
            _articles.Add(new KnowledgeBaseArticleImpl<FileDescriptorThrottlingProbe>(DiagnosticStatus.Healthy,
                "The number of file handles currently in use is below the max number allowed."));
            
            _articles.Add(new KnowledgeBaseArticleImpl<AvailableCpuCoresProbe>(DiagnosticStatus.Unhealthy,
                "Could not detect any CPU cores or none are available to the RabbitMQ broker.",
                "Add more CPU cores to the RabbitMQ node."));
            _articles.Add(new KnowledgeBaseArticleImpl<AvailableCpuCoresProbe>(DiagnosticStatus.Healthy, "Detected at least 1 CPU core available to the RabbitMQ broker."));
            
            _articles.Add(new KnowledgeBaseArticleImpl<NetworkPartitionProbe>(DiagnosticStatus.Unhealthy,
                "Network partitions detected between one or more nodes.",
                "Please consult the RabbitMQ documentation (https://www.rabbitmq.com/partitions.html) on which strategy best fits your scenario."));
            _articles.Add(new KnowledgeBaseArticleImpl<NetworkPartitionProbe>(DiagnosticStatus.Healthy, "No network partitions were detected between nodes."));
            
            _articles.Add(new KnowledgeBaseArticleImpl<RuntimeProcessLimitProbe>(DiagnosticStatus.Unhealthy,
                "The number of Erlang runtime processes in use is greater than or equal to the max number available.",
                ""));
            _articles.Add(new KnowledgeBaseArticleImpl<RuntimeProcessLimitProbe>(DiagnosticStatus.Healthy, "The number of Erlang runtime processes in use is less than the max number available."));
            
            _articles.Add(new KnowledgeBaseArticleImpl<MemoryAlarmProbe>(DiagnosticStatus.Unhealthy,
                "The threshold was reached for how much RAM can be used by the RabbitMQ Broker.",
                "Do one or a combination of the following:\n1) Increase the threshold of available RAM by changing either the vm_memory_high_watermark.absolute or vm_memory_high_watermark.relative broker configuration values.\n2) Spawn more consumers so that messages are not held in RAM for long periods.\n3) Increase the cluster hardware specification by adding more RAM."));
            _articles.Add(new KnowledgeBaseArticleImpl<MemoryAlarmProbe>(DiagnosticStatus.Healthy, "The amount of RAM used is less than the current threshold (i.e. vm_memory_high_watermark.absolute or vm_memory_high_watermark.relative)."));
            
            _articles.Add(new KnowledgeBaseArticleImpl<DiskAlarmProbe>(DiagnosticStatus.Unhealthy,
                "The node has reached the threshold for usable disk space.",
                "Increase message consumption throughput by spawning more consumers and/or increase disk size to keep up with incoming demand."));
            _articles.Add(new KnowledgeBaseArticleImpl<DiskAlarmProbe>(DiagnosticStatus.Healthy, "The node is under the allowable threshold for usable disk space."));
            
            _articles.Add(new KnowledgeBaseArticleImpl<SocketDescriptorThrottlingProbe>(DiagnosticStatus.Warning,
                "The number of network sockets being used is greater than the calculated high watermark but less than max number available.",
                ""));
            _articles.Add(new KnowledgeBaseArticleImpl<SocketDescriptorThrottlingProbe>(DiagnosticStatus.Unhealthy,
                "The number of network sockets being used is equal to the max number available.",
                ""));
            _articles.Add(new KnowledgeBaseArticleImpl<SocketDescriptorThrottlingProbe>(DiagnosticStatus.Inconclusive,
                "Either the sensor was not configured correctly or there was no data captured to analyze.",
                "Check the sensor configuration and the resultant snapshot data."));
            _articles.Add(new KnowledgeBaseArticleImpl<SocketDescriptorThrottlingProbe>(DiagnosticStatus.Healthy, "The number of network sockets used is less than the calculated high watermark."));
            
            _articles.Add(new KnowledgeBaseArticleImpl<RedeliveredMessagesProbe>(DiagnosticStatus.Warning,
                "The number of redelivered messages is less than or equal to the number of incoming messages and greater than or equal to the number of incoming messages multiplied a configurable coefficient.",
                ""));
            _articles.Add(new KnowledgeBaseArticleImpl<RedeliveredMessagesProbe>(DiagnosticStatus.Unhealthy,
                "The number of redelivered messages is less than or equal to the number of incoming messages.",
                ""));
            _articles.Add(new KnowledgeBaseArticleImpl<RedeliveredMessagesProbe>(DiagnosticStatus.Healthy, ""));
            
            _articles.Add(new KnowledgeBaseArticleImpl<QueueGrowthProbe>(DiagnosticStatus.Warning,
                "Messages are being published to the queue at a higher rate than are being consumed and acknowledged by consumers.",
                "Adjust application settings to spawn more consumers."));
            _articles.Add(new KnowledgeBaseArticleImpl<QueueGrowthProbe>(DiagnosticStatus.Healthy, "Messages are being consumed and acknowledged at a higher rate than are being published to the queue."));
           
            _articles.Add(new KnowledgeBaseArticleImpl<ChannelLimitReachedProbe>(DiagnosticStatus.Unhealthy,
                "Number of channels on connection exceeds the defined limit.",
                "Adjust application settings to reduce the number of connections to the RabbitMQ broker."));
            _articles.Add(new KnowledgeBaseArticleImpl<ChannelLimitReachedProbe>(DiagnosticStatus.Healthy,
                "Number of channels on connection is less than defined limit."));
            
            _articles.Add(new KnowledgeBaseArticleImpl<ChannelThrottlingProbe>(DiagnosticStatus.Unhealthy,
                "Unacknowledged messages on channel exceeds prefetch count causing the RabbitMQ broker to stop delivering messages to consumers.",
                "Acknowledged messages must be greater than or equal to the result of subtracting the number of unacknowledged messages from the prefetch count plus 1. Temporarily increase the number of consumers or prefetch count."));
            _articles.Add(new KnowledgeBaseArticleImpl<ChannelThrottlingProbe>(DiagnosticStatus.Healthy,
                "Unacknowledged messages on channel is less than prefetch count."));
            
            _articles.Add(new KnowledgeBaseArticleImpl<HighConnectionClosureRateProbe>(DiagnosticStatus.Warning, "", ""));
            _articles.Add(new KnowledgeBaseArticleImpl<HighConnectionClosureRateProbe>(DiagnosticStatus.Healthy, ""));
            
            _articles.Add(new KnowledgeBaseArticleImpl<HighConnectionCreationRateProbe>(DiagnosticStatus.Warning, "", ""));
            _articles.Add(new KnowledgeBaseArticleImpl<HighConnectionCreationRateProbe>(DiagnosticStatus.Healthy, ""));
            
            _articles.Add(new KnowledgeBaseArticleImpl<UnlimitedPrefetchCountProbe>(DiagnosticStatus.Warning,
                "Prefetch count of 0 means unlimited prefetch count, which can translate into high CPU utilization.",
                "Set a prefetch count above zero based on how many consumer cores available."));
            _articles.Add(new KnowledgeBaseArticleImpl<UnlimitedPrefetchCountProbe>(DiagnosticStatus.Inconclusive, "Unable to determine whether prefetch count has an inappropriate value."));
        }


        class KnowledgeBaseArticleImpl<T> :
            KnowledgeBaseArticle
            where T : DiagnosticProbe
        {
            public KnowledgeBaseArticleImpl(DiagnosticStatus diagnosticStatus, string reason, string remediation)
            {
                DiagnosticStatus = diagnosticStatus;
                Reason = reason;
                Remediation = remediation;
                Identifier = typeof(T).GetIdentifier();
            }

            public KnowledgeBaseArticleImpl(DiagnosticStatus diagnosticStatus, string reason)
            {
                DiagnosticStatus = diagnosticStatus;
                Reason = reason;
                Identifier = typeof(T).GetIdentifier();
            }

            public string Identifier { get; }
            public DiagnosticStatus DiagnosticStatus { get; }
            public string Reason { get; }
            public string Remediation { get; }
        }
    }
}