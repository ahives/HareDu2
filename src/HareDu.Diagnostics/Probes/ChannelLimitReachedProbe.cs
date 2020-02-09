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

    public class ChannelLimitReachedProbe :
        BaseDiagnosticProbe,
        IDiagnosticProbe
    {
        public string Identifier => GetType().GetIdentifier();
        public string Name => "Channel Limit Reached Analyzer";
        public string Description => "Measures actual number of channels to the defined limit on connection";
        public ComponentType ComponentType => ComponentType.Connection;
        public DiagnosticProbeCategory Category => DiagnosticProbeCategory.Throughput;
        public DiagnosticProbeStatus Status => _status;

        public ChannelLimitReachedProbe(DiagnosticsConfig config, IKnowledgeBaseProvider knowledgeBaseProvider)
            : base(knowledgeBaseProvider)
        {
            _status = DiagnosticProbeStatus.Online;
        }

        public DiagnosticProbeResult Execute<T>(T snapshot)
        {
            ConnectionSnapshot data = snapshot as ConnectionSnapshot;
            DiagnosticProbeResult result;

            var analyzerData = new List<DiagnosticProbeData>
            {
                new DiagnosticProbeDataImpl("Channels.Count", data.Channels.Count.ToString()),
                new DiagnosticProbeDataImpl("ChannelLimit", data.OpenChannelsLimit.ToString())
            };

            KnowledgeBaseArticle knowledgeBaseArticle;
            
            if (Convert.ToUInt64(data.Channels.Count) >= data.OpenChannelsLimit)
            {
                _knowledgeBaseProvider.TryGet(Identifier, DiagnosticStatus.Red, out knowledgeBaseArticle);
                result = new NegativeDiagnosticProbeResult(data.NodeIdentifier,
                    data.Identifier,
                    Identifier,
                    ComponentType,
                    analyzerData,
                    knowledgeBaseArticle);
            }
            else
            {
                _knowledgeBaseProvider.TryGet(Identifier, DiagnosticStatus.Green, out knowledgeBaseArticle);
                result = new PositiveDiagnosticProbeResult(data.NodeIdentifier,
                    data.Identifier,
                    Identifier,
                    ComponentType,
                    analyzerData, knowledgeBaseArticle);
            }

            NotifyObservers(result);

            return result;
        }
    }
}