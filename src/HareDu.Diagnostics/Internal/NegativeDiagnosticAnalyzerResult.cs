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
namespace HareDu.Diagnostics.Internal
{
    using System;
    using System.Collections.Generic;
    using KnowledgeBase;

    class NegativeDiagnosticAnalyzerResult :
        DiagnosticAnalyzerResult
    {
        public NegativeDiagnosticAnalyzerResult(string parentComponentIdentifier, string componentIdentifier, string analyzerIdentifier,
            ComponentType componentType, IReadOnlyList<DiagnosticAnalyzerData> analyzerData, KnowledgeBaseArticle knowledgeBaseArticle)
        {
            ParentComponentIdentifier = parentComponentIdentifier;
            ComponentIdentifier = componentIdentifier;
            AnalyzerIdentifier = analyzerIdentifier;
            ComponentType = componentType;
            AnalyzerData = analyzerData;
            KnowledgeBaseArticle = knowledgeBaseArticle;
            Status = DiagnosticStatus.Red;
            Timestamp = DateTimeOffset.Now;
        }

        public string ParentComponentIdentifier { get; }
        public string ComponentIdentifier { get; }
        public string AnalyzerIdentifier { get; }
        public DiagnosticStatus Status { get; }
        public KnowledgeBaseArticle KnowledgeBaseArticle { get; }
        public IReadOnlyList<DiagnosticAnalyzerData> AnalyzerData { get; }
        public ComponentType ComponentType { get; }
        public DateTimeOffset Timestamp { get; }
    }
}