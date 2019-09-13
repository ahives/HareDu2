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
namespace HareDu.Diagnostics.KnowledgeBase
{
    using Sensors;

    public class DefaultKnowledgeBaseProvider :
        BaseKnowledgeBaseProvider
    {
        protected override void Load()
        {
            _articles.Add(new KnowledgeBaseArticleImpl<QueueMessagePagingSensor>(DiagnosticStatus.Yellow, "", ""));
            _articles.Add(new KnowledgeBaseArticleImpl<QueueMessagePagingSensor>(DiagnosticStatus.Green, ""));
            _articles.Add(new KnowledgeBaseArticleImpl<BlockedConnectionSensor>(DiagnosticStatus.Red,
                "The connection is blocked meaning that an application has published but is now prevented from consuming. This is not the case with consume only connections.",
                ""));
            _articles.Add(new KnowledgeBaseArticleImpl<BlockedConnectionSensor>(DiagnosticStatus.Green, "Client applications are able to publish and/or consume on this connection."));
            _articles.Add(new KnowledgeBaseArticleImpl<FileDescriptorThrottlingSensor>(DiagnosticStatus.Red,
                "The max limit of available file descriptors that are in use has been reached. This will prevent applications from being able to open more connections to the broker and the RabbitMQ node from opening any files in support of current transactions.",
                "Increase the max number of allowed file handles (see https://www.rabbitmq.com/networking.html#open-file-handle-limit)."));
            _articles.Add(new KnowledgeBaseArticleImpl<FileDescriptorThrottlingSensor>(DiagnosticStatus.Yellow, "", ""));
            _articles.Add(new KnowledgeBaseArticleImpl<FileDescriptorThrottlingSensor>(DiagnosticStatus.Green,
                "The number of file handles currently in use is below the max number allowed."));
            _articles.Add(new KnowledgeBaseArticleImpl<AvailableCpuCoresSensor>(DiagnosticStatus.Red,
                "Could not detect any CPU cores or none are available to the RabbitMQ broker.",
                "Add more CPU cores to the RabbitMQ node."));
            _articles.Add(new KnowledgeBaseArticleImpl<AvailableCpuCoresSensor>(DiagnosticStatus.Green, "Detected at least 1 CPU core available to the RabbitMQ broker."));
            _articles.Add(new KnowledgeBaseArticleImpl<NetworkPartitionSensor>(DiagnosticStatus.Red,
                "Network partitions detected between one or more nodes.",
                "Please consult the RabbitMQ documentation (https://www.rabbitmq.com/partitions.html) on which strategy best fits your scenario."));
            _articles.Add(new KnowledgeBaseArticleImpl<NetworkPartitionSensor>(DiagnosticStatus.Green, "No network partitions were detected between nodes."));
            _articles.Add(new KnowledgeBaseArticleImpl<RuntimeProcessLimitSensor>(DiagnosticStatus.Red,
                "The number of Erlang runtime processes in use is greater than or equal to the max number available.",
                ""));
            _articles.Add(new KnowledgeBaseArticleImpl<RuntimeProcessLimitSensor>(DiagnosticStatus.Green, "The number of Erlang runtime processes in use is less than the max number available."));
            _articles.Add(new KnowledgeBaseArticleImpl<MemoryAlarmSensor>(DiagnosticStatus.Red,
                "The threshold was reached for how much RAM can be used by the RabbitMQ Broker.",
                "Do one or a combination of the following:\n1) Increase the threshold of available RAM by changing either the vm_memory_high_watermark.absolute or vm_memory_high_watermark.relative broker configuration values.\n2) Spawn more consumers so that messages are not held in RAM for long periods.\n3) Increase the cluster hardware specification by adding more RAM."));
            _articles.Add(new KnowledgeBaseArticleImpl<MemoryAlarmSensor>(DiagnosticStatus.Green, "The amount of RAM used is less than the current threshold (i.e. vm_memory_high_watermark.absolute or vm_memory_high_watermark.relative)."));
            _articles.Add(new KnowledgeBaseArticleImpl<DiskAlarmSensor>(DiagnosticStatus.Red,
                "The node has reached the threshold for usable disk space.",
                "Increase message consumption throughput by spawning more consumers and/or increase disk size to keep up with incoming demand."));
            _articles.Add(new KnowledgeBaseArticleImpl<DiskAlarmSensor>(DiagnosticStatus.Green, "The node is under the allowable threshold for usable disk space."));
            _articles.Add(new KnowledgeBaseArticleImpl<NetworkThrottlingSensor>(DiagnosticStatus.Yellow,
                "The number of network sockets being used is greater than the calculated high watermark but less than max number available.",
                ""));
            _articles.Add(new KnowledgeBaseArticleImpl<NetworkThrottlingSensor>(DiagnosticStatus.Red,
                "The number of network sockets being used is equal to the max number available.",
                ""));
            _articles.Add(new KnowledgeBaseArticleImpl<NetworkThrottlingSensor>(DiagnosticStatus.Inconclusive,
                "Either the sensor was not configured correctly or there was no data captured to analyze.",
                "Check the sensor configuration and the resultant snapshot data."));
            _articles.Add(new KnowledgeBaseArticleImpl<NetworkThrottlingSensor>(DiagnosticStatus.Green, "The number of network sockets used is less than the calculated high watermark."));
            _articles.Add(new KnowledgeBaseArticleImpl<RedeliveredMessagesSensor>(DiagnosticStatus.Yellow,
                "The number of redelivered messages is less than or equal to the number of incoming messages and greater than or equal to the number of incoming messages multiplied a configurable coefficient.",
                ""));
            _articles.Add(new KnowledgeBaseArticleImpl<RedeliveredMessagesSensor>(DiagnosticStatus.Red,
                "The number of redelivered messages is less than or equal to the number of incoming messages.",
                ""));
            _articles.Add(new KnowledgeBaseArticleImpl<RedeliveredMessagesSensor>(DiagnosticStatus.Green, ""));
            _articles.Add(new KnowledgeBaseArticleImpl<QueueGrowthSensor>(DiagnosticStatus.Yellow,
                "Messages are being published to the queue at a higher rate than are being consumed and acknowledged by consumers.",
                "Adjust application settings to spawn more consumers."));
            _articles.Add(new KnowledgeBaseArticleImpl<QueueGrowthSensor>(DiagnosticStatus.Green, "Messages are being consumed and acknowledged at a higher rate than are being published to the queue."));
            _articles.Add(new KnowledgeBaseArticleImpl<ChannelLimitReachedSensor>(DiagnosticStatus.Red,
                "Number of channels on connection exceeds the defined limit.",
                "Adjust application settings to reduce the number of connections to the RabbitMQ broker."));
            _articles.Add(new KnowledgeBaseArticleImpl<ChannelLimitReachedSensor>(DiagnosticStatus.Green,
                "Number of channels on connection is less than defined limit."));
            _articles.Add(new KnowledgeBaseArticleImpl<ChannelThrottlingSensor>(DiagnosticStatus.Red,
                "Unacknowledged messages on channel exceeds prefetch count causing the RabbitMQ broker to stop delivering messages to consumers.",
                "Acknowledged messages must be greater than or equal to the result of subtracting the number of unacknowledged messages from the prefetch count plus 1. Temporarily increase the number of consumers or prefetch count."));
            _articles.Add(new KnowledgeBaseArticleImpl<ChannelThrottlingSensor>(DiagnosticStatus.Green,
                "Unacknowledged messages on channel is less than prefetch count."));
            _articles.Add(new KnowledgeBaseArticleImpl<HighConnectionClosureRateSensor>(DiagnosticStatus.Yellow, "", ""));
            _articles.Add(new KnowledgeBaseArticleImpl<HighConnectionClosureRateSensor>(DiagnosticStatus.Green, ""));
            _articles.Add(new KnowledgeBaseArticleImpl<HighConnectionCreationRateSensor>(DiagnosticStatus.Yellow, "", ""));
            _articles.Add(new KnowledgeBaseArticleImpl<HighConnectionCreationRateSensor>(DiagnosticStatus.Green, ""));
            _articles.Add(new KnowledgeBaseArticleImpl<UnlimitedPrefetchCountSensor>(DiagnosticStatus.Yellow,
                "Prefetch count of 0 means unlimited prefetch count, which can translate into high CPU utilization.",
                "Set a prefetch count above zero based on how many consumer cores available."));
            _articles.Add(new KnowledgeBaseArticleImpl<UnlimitedPrefetchCountSensor>(DiagnosticStatus.Inconclusive, "Unable to determine whether prefetch count has an inappropriate value."));
        }


        class KnowledgeBaseArticleImpl<T> :
            KnowledgeBaseArticle
            where T : IDiagnosticSensor
        {
            public KnowledgeBaseArticleImpl(DiagnosticStatus diagnosticStatus, string reason, string remediation)
            {
                DiagnosticStatus = diagnosticStatus;
                Reason = reason;
                Remediation = remediation;
                Identifier = typeof(T).FullName.GenerateIdentifier();
            }

            public KnowledgeBaseArticleImpl(DiagnosticStatus diagnosticStatus, string reason)
            {
                DiagnosticStatus = diagnosticStatus;
                Reason = reason;
                Identifier = typeof(T).FullName.GenerateIdentifier();
            }

            public string Identifier { get; }
            public DiagnosticStatus DiagnosticStatus { get; }
            public string Reason { get; }
            public string Remediation { get; }
        }
    }
}