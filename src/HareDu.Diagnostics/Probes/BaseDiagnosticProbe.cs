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
        IObservable<ProbeContext>
    {
        protected readonly IKnowledgeBaseProvider _kb;
        readonly List<IObserver<ProbeContext>> _observers;

        protected BaseDiagnosticProbe(IKnowledgeBaseProvider kb)
        {
            _kb = kb;
            _observers = new List<IObserver<ProbeContext>>();
        }

        protected virtual void NotifyObservers(ProbeResult result)
        {
            foreach (var observer in _observers)
            {
                observer.OnNext(new ProbeContextImpl(result));
            }
        }

        public IDisposable Subscribe(IObserver<ProbeContext> observer)
        {
            if (!_observers.Contains(observer))
                _observers.Add(observer);

            return new UnsubscribeObserver<ProbeContext>(_observers, observer);
        }


        class ProbeContextImpl :
            ProbeContext
        {
            public ProbeContextImpl(ProbeResult result)
            {
                Result = result;
                Timestamp = DateTimeOffset.UtcNow;
            }

            public ProbeResult Result { get; }
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

        
        protected class NotApplicableProbeResult :
            ProbeResult
        {
            public NotApplicableProbeResult(string parentComponentId, string componentId,
                string probeId, ComponentType componentType, KnowledgeBaseArticle article)
            {
                ParentComponentId = parentComponentId;
                ComponentId = componentId;
                ComponentType = componentType;
                ProbeId = probeId;
                Article = article;
                Timestamp = DateTimeOffset.UtcNow;
            }

            public string ParentComponentId { get; }
            public string ComponentId { get; }
            public ComponentType ComponentType { get; }
            public string ProbeId { get; }
            public DiagnosticProbeResultStatus Status => DiagnosticProbeResultStatus.NA;
            public KnowledgeBaseArticle Article { get; }
            public IReadOnlyList<ProbeData> ProbeData => Array.Empty<ProbeData>();
            public DateTimeOffset Timestamp { get; }
        }
        
        
        protected class HealthyProbeResult :
            ProbeResult
        {
            public HealthyProbeResult(string parentComponentId, string componentId, string probeId,
                ComponentType componentType, List<ProbeData> probeData, KnowledgeBaseArticle article)
            {
                ParentComponentId = parentComponentId;
                ComponentId = componentId;
                ProbeId = probeId;
                ComponentType = componentType;
                ProbeData = probeData;
                Article = article;
                Status = DiagnosticProbeResultStatus.Healthy;
                Timestamp = DateTimeOffset.UtcNow;
            }

            public string ParentComponentId { get; }
            public string ComponentId { get; }
            public ComponentType ComponentType { get; }
            public string ProbeId { get; }
            public DiagnosticProbeResultStatus Status { get; }
            public KnowledgeBaseArticle Article { get; }
            public IReadOnlyList<ProbeData> ProbeData { get; }
            public DateTimeOffset Timestamp { get; }
        }


        protected class UnhealthyProbeResult :
            ProbeResult
        {
            public UnhealthyProbeResult(string parentComponentId, string componentId, string probeId,
                ComponentType componentType, IReadOnlyList<ProbeData> probeData, KnowledgeBaseArticle article)
            {
                ParentComponentId = parentComponentId;
                ComponentId = componentId;
                ProbeId = probeId;
                ComponentType = componentType;
                ProbeData = probeData;
                Article = article;
                Status = DiagnosticProbeResultStatus.Unhealthy;
                Timestamp = DateTimeOffset.UtcNow;
            }

            public string ParentComponentId { get; }
            public string ComponentId { get; }
            public string ProbeId { get; }
            public DiagnosticProbeResultStatus Status { get; }
            public KnowledgeBaseArticle Article { get; }
            public IReadOnlyList<ProbeData> ProbeData { get; }
            public ComponentType ComponentType { get; }
            public DateTimeOffset Timestamp { get; }
        }


        protected class WarningProbeResult :
            ProbeResult
        {
            public WarningProbeResult(string parentComponentId, string componentId, string probeId,
                ComponentType componentType, IReadOnlyList<ProbeData> probeData, KnowledgeBaseArticle article)
            {
                ParentComponentId = parentComponentId;
                ComponentId = componentId;
                ProbeId = probeId;
                ComponentType = componentType;
                ProbeData = probeData;
                Article = article;
                Status = DiagnosticProbeResultStatus.Warning;
                Timestamp = DateTimeOffset.UtcNow;
            }

            public string ParentComponentId { get; }
            public string ComponentId { get; }
            public ComponentType ComponentType { get; }
            public string ProbeId { get; }
            public DiagnosticProbeResultStatus Status { get; }
            public KnowledgeBaseArticle Article { get; }
            public IReadOnlyList<ProbeData> ProbeData { get; }
            public DateTimeOffset Timestamp { get; }
        }


        protected class InconclusiveProbeResult :
            ProbeResult
        {
            public InconclusiveProbeResult(string parentComponentId, string componentId,
                string probeId, ComponentType componentType, IReadOnlyList<ProbeData> probeData,
                KnowledgeBaseArticle article)
            {
                ParentComponentId = parentComponentId;
                ComponentId = componentId;
                ComponentType = componentType;
                ProbeId = probeId;
                Article = article;
                ProbeData = probeData;
                Status = DiagnosticProbeResultStatus.Inconclusive;
                Timestamp = DateTimeOffset.UtcNow;
            }

            public InconclusiveProbeResult(string parentComponentId, string componentId,
                string probeId, ComponentType componentType)
            {
                ParentComponentId = parentComponentId;
                ComponentId = componentId;
                ComponentType = componentType;
                ProbeId = probeId;
                ProbeData = DiagnosticCache.EmptyProbeData;
                Status = DiagnosticProbeResultStatus.Inconclusive;
                Timestamp = DateTimeOffset.Now;
            }

            public string ParentComponentId { get; }
            public string ComponentId { get; }
            public ComponentType ComponentType { get; }
            public string ProbeId { get; }
            public DiagnosticProbeResultStatus Status { get; }
            public KnowledgeBaseArticle Article { get; }
            public IReadOnlyList<ProbeData> ProbeData { get; }
            public DateTimeOffset Timestamp { get; }
        }


        protected class ProbeDataImpl :
            ProbeData
        {
            public ProbeDataImpl(string propertyName, string propertyValue)
            {
                PropertyName = propertyName;
                PropertyValue = propertyValue;
            }

            public string PropertyName { get; }
            public string PropertyValue { get; }
        }
    }
}