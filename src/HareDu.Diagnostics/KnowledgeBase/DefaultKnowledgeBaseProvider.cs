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
            _articles.Add(new KnowledgeBaseArticleImpl<UnlimitedPrefetchCountSensor>(DiagnosticStatus.Inconclusive, ""));
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
                Identifier = typeof(T).FullName.ComputeHash();
            }

            public KnowledgeBaseArticleImpl(DiagnosticStatus diagnosticStatus, string reason)
            {
                DiagnosticStatus = diagnosticStatus;
                Reason = reason;
                Identifier = typeof(T).FullName.ComputeHash();
            }

            public string Identifier { get; }
            public DiagnosticStatus DiagnosticStatus { get; }
            public string Reason { get; }
            public string Remediation { get; }
        }
    }
}