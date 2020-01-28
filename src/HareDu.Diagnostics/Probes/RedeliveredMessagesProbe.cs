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
namespace HareDu.Diagnostics.Probes
{
    using System;
    using System.Collections.Generic;
    using Core.Configuration;
    using Core.Extensions;
    using Internal;
    using KnowledgeBase;
    using Snapshotting.Model;

    public class RedeliveredMessagesProbe :
        BaseDiagnosticProbe,
        IDiagnosticProbe
    {
        readonly DiagnosticsConfig _config;
        public string Identifier => GetType().GetIdentifier();
        public string Name => "Redelivered Messages Analyzer";
        public string Description { get; }
        public ComponentType ComponentType => ComponentType.Queue;
        public DiagnosticProbeCategory Category => DiagnosticProbeCategory.FaultTolerance;
        public DiagnosticProbeStatus Status => _status;

        public RedeliveredMessagesProbe(DiagnosticsConfig config, IKnowledgeBaseProvider knowledgeBaseProvider)
            : base(knowledgeBaseProvider)
        {
            _config = config;
            _status = !_config.IsNull() ? DiagnosticProbeStatus.Online : DiagnosticProbeStatus.Offline;
        }

        public DiagnosticProbeResult Execute<T>(T snapshot)
        {
            DiagnosticProbeResult result;
            QueueSnapshot data = snapshot as QueueSnapshot;
            
            var analyzerData = new List<DiagnosticProbeData>
            {
                new DiagnosticProbeDataImpl("Messages.Incoming.Total", data.Messages.Incoming.Total.ToString()),
                new DiagnosticProbeDataImpl("Messages.Redelivered.Total", data.Messages.Redelivered.Total.ToString()),
                new DiagnosticProbeDataImpl("MessageRedeliveryCoefficient", _config.MessageRedeliveryCoefficient.ToString()),
                new DiagnosticProbeDataImpl("MessageRedeliveryCoefficient", _config.MessageRedeliveryCoefficient.ToString())
            };
            
            KnowledgeBaseArticle knowledgeBaseArticle;
            ulong warningThreshold = ComputeWarningThreshold(data.Messages.Incoming.Total);
            
            if (data.Messages.Redelivered.Total >= warningThreshold && data.Messages.Redelivered.Total < data.Messages.Incoming.Total && warningThreshold < data.Messages.Incoming.Total)
            {
                _knowledgeBaseProvider.TryGet(Identifier, DiagnosticStatus.Yellow, out knowledgeBaseArticle);
                result = new WarningDiagnosticProbeResult(data.Node,
                    data.Identifier,
                    Identifier,
                    ComponentType,
                    analyzerData,
                    knowledgeBaseArticle);
            }
            else if (data.Messages.Redelivered.Total >= data.Messages.Incoming.Total)
            {
                _knowledgeBaseProvider.TryGet(Identifier, DiagnosticStatus.Red, out knowledgeBaseArticle);
                result = new NegativeDiagnosticProbeResult(data.Node,
                    data.Identifier,
                    Identifier,
                    ComponentType,
                    analyzerData,
                    knowledgeBaseArticle);
            }
            else
            {
                _knowledgeBaseProvider.TryGet(Identifier, DiagnosticStatus.Green, out knowledgeBaseArticle);
                result = new PositiveDiagnosticProbeResult(data.Node,
                    data.Identifier,
                    Identifier,
                    ComponentType,
                    analyzerData,
                    knowledgeBaseArticle);
            }

            NotifyObservers(result);

            return result;
        }

        ulong ComputeWarningThreshold(ulong total)
            => _config.MessageRedeliveryCoefficient >= 1
                ? total
                : Convert.ToUInt64(Math.Ceiling(total * _config.MessageRedeliveryCoefficient));
    }
}