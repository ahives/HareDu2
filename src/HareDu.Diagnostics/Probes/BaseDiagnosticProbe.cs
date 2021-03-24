namespace HareDu.Diagnostics.Probes
{
    using System;
    using System.Collections.Generic;
    using Core.Extensions;
    using KnowledgeBase;

    public abstract class BaseDiagnosticProbe :
        IObservable<ProbeContext>
    {
        protected readonly IKnowledgeBaseProvider _kb;
        readonly List<IObserver<ProbeContext>> _resultObservers;

        protected BaseDiagnosticProbe(IKnowledgeBaseProvider kb)
        {
            _kb = kb;
            _resultObservers = new List<IObserver<ProbeContext>>();
        }

        public IDisposable Subscribe(IObserver<ProbeContext> observer)
        {
            if (!_resultObservers.Contains(observer))
                _resultObservers.Add(observer);

            return new UnsubscribeObserver<ProbeContext>(_resultObservers, observer);
        }

        protected virtual void NotifyObservers(ProbeResult result)
        {
            foreach (var observer in _resultObservers)
                observer.OnNext(new ProbeContextImpl(result));
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

        protected class DiagnosticProbeMetadataImpl<T> :
            DiagnosticProbeMetadata
        {
            public DiagnosticProbeMetadataImpl(string name, string description)
            {
                Id = typeof(T).GetIdentifier();
                Name = name;
                Description = description;
            }

            public string Id { get; }
            public string Name { get; }
            public string Description { get; }
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
                string probeId, string name, ComponentType componentType, KnowledgeBaseArticle article)
            {
                ParentComponentId = parentComponentId;
                ComponentId = componentId;
                ComponentType = componentType;
                Id = probeId;
                Name = name;
                KB = article;
                Timestamp = DateTimeOffset.UtcNow;
            }

            public string ParentComponentId { get; }
            public string ComponentId { get; }
            public ComponentType ComponentType { get; }
            public string Id { get; }
            public string Name { get; }
            public ProbeResultStatus Status => ProbeResultStatus.NA;
            public KnowledgeBaseArticle KB { get; }
            public IReadOnlyList<ProbeData> Data => Array.Empty<ProbeData>();
            public DateTimeOffset Timestamp { get; }
        }
        
        
        protected class HealthyProbeResult :
            ProbeResult
        {
            public HealthyProbeResult(string parentComponentId, string componentId, string probeId, string name,
                ComponentType componentType, List<ProbeData> probeData, KnowledgeBaseArticle article)
            {
                ParentComponentId = parentComponentId;
                ComponentId = componentId;
                Id = probeId;
                Name = name;
                ComponentType = componentType;
                Data = probeData;
                KB = article;
                Status = ProbeResultStatus.Healthy;
                Timestamp = DateTimeOffset.UtcNow;
            }

            public string ParentComponentId { get; }
            public string ComponentId { get; }
            public ComponentType ComponentType { get; }
            public string Id { get; }
            public string Name { get; }
            public ProbeResultStatus Status { get; }
            public KnowledgeBaseArticle KB { get; }
            public IReadOnlyList<ProbeData> Data { get; }
            public DateTimeOffset Timestamp { get; }
        }


        protected class UnhealthyProbeResult :
            ProbeResult
        {
            public UnhealthyProbeResult(string parentComponentId, string componentId, string probeId, string name,
                ComponentType componentType, IReadOnlyList<ProbeData> probeData, KnowledgeBaseArticle article)
            {
                ParentComponentId = parentComponentId;
                ComponentId = componentId;
                Id = probeId;
                Name = name;
                ComponentType = componentType;
                Data = probeData;
                KB = article;
                Status = ProbeResultStatus.Unhealthy;
                Timestamp = DateTimeOffset.UtcNow;
            }

            public string ParentComponentId { get; }
            public string ComponentId { get; }
            public string Id { get; }
            public string Name { get; }
            public ProbeResultStatus Status { get; }
            public KnowledgeBaseArticle KB { get; }
            public IReadOnlyList<ProbeData> Data { get; }
            public ComponentType ComponentType { get; }
            public DateTimeOffset Timestamp { get; }
        }


        protected class WarningProbeResult :
            ProbeResult
        {
            public WarningProbeResult(string parentComponentId, string componentId, string probeId, string name,
                ComponentType componentType, IReadOnlyList<ProbeData> probeData, KnowledgeBaseArticle article)
            {
                ParentComponentId = parentComponentId;
                ComponentId = componentId;
                Id = probeId;
                Name = name;
                ComponentType = componentType;
                Data = probeData;
                KB = article;
                Status = ProbeResultStatus.Warning;
                Timestamp = DateTimeOffset.UtcNow;
            }

            public string ParentComponentId { get; }
            public string ComponentId { get; }
            public ComponentType ComponentType { get; }
            public string Id { get; }
            public string Name { get; }
            public ProbeResultStatus Status { get; }
            public KnowledgeBaseArticle KB { get; }
            public IReadOnlyList<ProbeData> Data { get; }
            public DateTimeOffset Timestamp { get; }
        }


        protected class InconclusiveProbeResult :
            ProbeResult
        {
            public InconclusiveProbeResult(string parentComponentId, string componentId,
                string probeId, string name, ComponentType componentType, IReadOnlyList<ProbeData> probeData,
                KnowledgeBaseArticle article)
            {
                ParentComponentId = parentComponentId;
                ComponentId = componentId;
                ComponentType = componentType;
                Id = probeId;
                Name = name;
                KB = article;
                Data = probeData;
                Status = ProbeResultStatus.Inconclusive;
                Timestamp = DateTimeOffset.UtcNow;
            }

            public InconclusiveProbeResult(string parentComponentId, string componentId,
                string probeId, ComponentType componentType)
            {
                ParentComponentId = parentComponentId;
                ComponentId = componentId;
                ComponentType = componentType;
                Id = probeId;
                Data = DiagnosticCache.EmptyProbeData;
                Status = ProbeResultStatus.Inconclusive;
                Timestamp = DateTimeOffset.Now;
            }

            public string ParentComponentId { get; }
            public string ComponentId { get; }
            public ComponentType ComponentType { get; }
            public string Id { get; }
            public string Name { get; }
            public ProbeResultStatus Status { get; }
            public KnowledgeBaseArticle KB { get; }
            public IReadOnlyList<ProbeData> Data { get; }
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