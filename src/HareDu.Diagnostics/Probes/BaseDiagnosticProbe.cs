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
    using KnowledgeBase;

    public abstract class BaseDiagnosticProbe :
        IObservable<DiagnosticProbeContext>
    {
        protected readonly IKnowledgeBaseProvider _knowledgeBaseProvider;
        protected DiagnosticProbeStatus _status;
        readonly List<IObserver<DiagnosticProbeContext>> _observers;

        protected BaseDiagnosticProbe(IKnowledgeBaseProvider knowledgeBaseProvider)
        {
            _knowledgeBaseProvider = knowledgeBaseProvider;
            _observers = new List<IObserver<DiagnosticProbeContext>>();
        }

        protected virtual void NotifyObservers(DiagnosticProbeResult result)
        {
            foreach (var observer in _observers)
            {
                observer.OnNext(new DiagnosticProbeContextImpl(result));
            }
        }

        public IDisposable Subscribe(IObserver<DiagnosticProbeContext> observer)
        {
            if (!_observers.Contains(observer))
                _observers.Add(observer);

            return new UnsubscribeObserver<DiagnosticProbeContext>(_observers, observer);
        }


        class DiagnosticProbeContextImpl :
            DiagnosticProbeContext
        {
            public DiagnosticProbeContextImpl(DiagnosticProbeResult result)
            {
                Result = result;
                Timestamp = DateTimeOffset.UtcNow;
            }

            public DiagnosticProbeResult Result { get; }
            public DateTimeOffset Timestamp { get; }
        }
        
        
        protected class UnsubscribeObserver<T> :
            IDisposable
        {
            readonly List<IObserver<T>> _observers;
            readonly IObserver<T> _observer;

            public UnsubscribeObserver(List<IObserver<T>> observers, IObserver<T> observer)
            {
                _observers = observers;
                _observer = observer;
            }

            public void Dispose()
            {
                if (_observer != null && _observers.Contains(_observer))
                    _observers.Remove(_observer);
            }
        }


        protected class HealthyProbeResult :
            DiagnosticProbeResult
        {
            public HealthyProbeResult(string parentComponentIdentifier, string componentIdentifier, string analyzerIdentifier,
                ComponentType componentType, List<DiagnosticProbeData> analyzerData, KnowledgeBaseArticle knowledgeBaseArticle)
            {
                ParentComponentIdentifier = parentComponentIdentifier;
                ComponentIdentifier = componentIdentifier;
                ProbeIdentifier = analyzerIdentifier;
                ComponentType = componentType;
                ProbeData = analyzerData;
                KnowledgeBaseArticle = knowledgeBaseArticle;
                Status = DiagnosticStatus.Healthy;
                Timestamp = DateTimeOffset.Now;
            }

            public string ParentComponentIdentifier { get; }
            public string ComponentIdentifier { get; }
            public ComponentType ComponentType { get; }
            public string ProbeIdentifier { get; }
            public DiagnosticStatus Status { get; }
            public KnowledgeBaseArticle KnowledgeBaseArticle { get; }
            public IReadOnlyList<DiagnosticProbeData> ProbeData { get; }
            public DateTimeOffset Timestamp { get; }
        }


        protected class UnhealthyProbeResult :
            DiagnosticProbeResult
        {
            public UnhealthyProbeResult(string parentComponentIdentifier, string componentIdentifier, string analyzerIdentifier,
                ComponentType componentType, IReadOnlyList<DiagnosticProbeData> analyzerData, KnowledgeBaseArticle knowledgeBaseArticle)
            {
                ParentComponentIdentifier = parentComponentIdentifier;
                ComponentIdentifier = componentIdentifier;
                ProbeIdentifier = analyzerIdentifier;
                ComponentType = componentType;
                ProbeData = analyzerData;
                KnowledgeBaseArticle = knowledgeBaseArticle;
                Status = DiagnosticStatus.Unhealthy;
                Timestamp = DateTimeOffset.Now;
            }

            public string ParentComponentIdentifier { get; }
            public string ComponentIdentifier { get; }
            public string ProbeIdentifier { get; }
            public DiagnosticStatus Status { get; }
            public KnowledgeBaseArticle KnowledgeBaseArticle { get; }
            public IReadOnlyList<DiagnosticProbeData> ProbeData { get; }
            public ComponentType ComponentType { get; }
            public DateTimeOffset Timestamp { get; }
        }


        protected class WarningProbeResult :
            DiagnosticProbeResult
        {
            public WarningProbeResult(string parentComponentIdentifier, string componentIdentifier, string analyzerIdentifier,
                ComponentType componentType, IReadOnlyList<DiagnosticProbeData> analyzerData, KnowledgeBaseArticle knowledgeBaseArticle)
            {
                ParentComponentIdentifier = parentComponentIdentifier;
                ComponentIdentifier = componentIdentifier;
                ProbeIdentifier = analyzerIdentifier;
                ComponentType = componentType;
                ProbeData = analyzerData;
                KnowledgeBaseArticle = knowledgeBaseArticle;
                Status = DiagnosticStatus.Warning;
                Timestamp = DateTimeOffset.Now;
            }

            public string ParentComponentIdentifier { get; }
            public string ComponentIdentifier { get; }
            public ComponentType ComponentType { get; }
            public string ProbeIdentifier { get; }
            public DiagnosticStatus Status { get; }
            public KnowledgeBaseArticle KnowledgeBaseArticle { get; }
            public IReadOnlyList<DiagnosticProbeData> ProbeData { get; }
            public DateTimeOffset Timestamp { get; }
        }


        protected class InconclusiveDiagnosticProbeResult :
            DiagnosticProbeResult
        {
            public InconclusiveDiagnosticProbeResult(string parentComponentIdentifier, string componentIdentifier,
                string analyzerIdentifier, ComponentType componentType, IReadOnlyList<DiagnosticProbeData> analyzerData,
                KnowledgeBaseArticle knowledgeBaseArticle)
            {
                ParentComponentIdentifier = parentComponentIdentifier;
                ComponentIdentifier = componentIdentifier;
                ComponentType = componentType;
                ProbeIdentifier = analyzerIdentifier;
                KnowledgeBaseArticle = knowledgeBaseArticle;
                ProbeData = analyzerData;
                Status = DiagnosticStatus.Inconclusive;
                Timestamp = DateTimeOffset.Now;
            }

            public InconclusiveDiagnosticProbeResult(string parentComponentIdentifier, string componentIdentifier,
                string analyzerIdentifier, ComponentType componentType)
            {
                ParentComponentIdentifier = parentComponentIdentifier;
                ComponentIdentifier = componentIdentifier;
                ComponentType = componentType;
                ProbeIdentifier = analyzerIdentifier;
                ProbeData = DiagnosticCache.EmptyProbeData;
                Status = DiagnosticStatus.Inconclusive;
                Timestamp = DateTimeOffset.Now;
            }

            public string ParentComponentIdentifier { get; }
            public string ComponentIdentifier { get; }
            public ComponentType ComponentType { get; }
            public string ProbeIdentifier { get; }
            public DiagnosticStatus Status { get; }
            public KnowledgeBaseArticle KnowledgeBaseArticle { get; }
            public IReadOnlyList<DiagnosticProbeData> ProbeData { get; }
            public DateTimeOffset Timestamp { get; }
        }


        protected class DiagnosticProbeDataImpl :
            DiagnosticProbeData
        {
            public DiagnosticProbeDataImpl(string propertyName, string propertyValue)
            {
                PropertyName = propertyName;
                PropertyValue = propertyValue;
            }

            public string PropertyName { get; }
            public string PropertyValue { get; }
        }
    }
}