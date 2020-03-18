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
    using KnowledgeBase;

    public abstract class BaseDiagnosticProbe :
        IObservable<ProbeContext>,
        IObservable<ProbeConfigurationContext>
    {
        protected readonly IKnowledgeBaseProvider _kb;
        readonly List<IObserver<ProbeContext>> _resultObservers;
        readonly List<IObserver<ProbeConfigurationContext>> _configObservers;

        protected BaseDiagnosticProbe(IKnowledgeBaseProvider kb)
        {
            _kb = kb;
            _resultObservers = new List<IObserver<ProbeContext>>();
            _configObservers = new List<IObserver<ProbeConfigurationContext>>();
        }

        public IDisposable Subscribe(IObserver<ProbeContext> observer)
        {
            if (!_resultObservers.Contains(observer))
                _resultObservers.Add(observer);

            return new UnsubscribeObserver<ProbeContext>(_resultObservers, observer);
        }

        public IDisposable Subscribe(IObserver<ProbeConfigurationContext> observer)
        {
            if (!_configObservers.Contains(observer))
                _configObservers.Add(observer);

            return new UnsubscribeObserver<ProbeConfigurationContext>(_configObservers, observer);
        }

        protected virtual void NotifyObservers(ProbeResult result)
        {
            foreach (var observer in _resultObservers)
            {
                observer.OnNext(new ProbeContextImpl(result));
            }
        }

        protected virtual void NotifyObservers(string probeId, string probeName, DiagnosticsConfig current, DiagnosticsConfig @new)
        {
            foreach (var observer in _configObservers)
            {
                observer.OnNext(new ProbeConfigurationContextImpl(probeId, probeName, current, @new));
            }
        }

        
        class ProbeConfigurationContextImpl :
            ProbeConfigurationContext
        {
            public ProbeConfigurationContextImpl(string probeId, string probeName, DiagnosticsConfig current, DiagnosticsConfig @new)
            {
                ProbeId = probeId;
                ProbeName = probeName;
                Current = current;
                New = @new;
                Timestamp = DateTimeOffset.UtcNow;
            }

            public string ProbeId { get; }
            public string ProbeName { get; }
            public DiagnosticsConfig Current { get; }
            public DiagnosticsConfig New { get; }
            public DateTimeOffset Timestamp { get; }
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