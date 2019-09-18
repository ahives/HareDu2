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
namespace HareDu.Diagnostics.Sensors
{
    using System;
    using System.Collections.Generic;
    using Configuration;
    using KnowledgeBase;

    public abstract class BaseDiagnosticSensor :
        IObservable<DiagnosticContext>,
        IObservable<DiagnosticSensorContext>
    {
        protected readonly IDiagnosticScannerConfigProvider _configProvider;
        protected readonly IKnowledgeBaseProvider _knowledgeBaseProvider;
        protected DiagnosticScannerConfig _config;
        protected DiagnosticSensorStatus _sensorStatus;
        readonly List<IObserver<DiagnosticContext>> _diagnosticObservers;
        readonly List<IObserver<DiagnosticSensorContext>> _sensorObservers;

        protected BaseDiagnosticSensor(IDiagnosticScannerConfigProvider configProvider, IKnowledgeBaseProvider knowledgeBaseProvider)
        {
            _configProvider = configProvider;
            _knowledgeBaseProvider = knowledgeBaseProvider;
            _diagnosticObservers = new List<IObserver<DiagnosticContext>>();
            _sensorObservers = new List<IObserver<DiagnosticSensorContext>>();
        }

        protected virtual void NotifyObservers(DiagnosticResult result)
        {
            foreach (var observer in _diagnosticObservers)
            {
                observer.OnNext(new DiagnosticContextImpl(result));
            }
        }

        protected virtual void NotifyObservers(DiagnosticSensorResult result)
        {
            foreach (var observer in _sensorObservers)
            {
                observer.OnNext(new DiagnosticSensorContextImpl(result));
            }
        }

        public IDisposable Subscribe(IObserver<DiagnosticContext> observer)
        {
            if (!_diagnosticObservers.Contains(observer))
                _diagnosticObservers.Add(observer);

            return new UnsubscribeObserver<DiagnosticContext>(_diagnosticObservers, observer);
        }

        public IDisposable Subscribe(IObserver<DiagnosticSensorContext> observer)
        {
            if (!_sensorObservers.Contains(observer))
                _sensorObservers.Add(observer);

            return new UnsubscribeObserver<DiagnosticSensorContext>(_sensorObservers, observer);
        }

        
        class DiagnosticSensorContextImpl :
            DiagnosticSensorContext
        {
            public DiagnosticSensorContextImpl(DiagnosticSensorResult result)
            {
                Result = result;
                Timestamp = DateTimeOffset.UtcNow;
            }

            public DiagnosticSensorResult Result { get; }
            public DateTimeOffset Timestamp { get; }
        }

        
        class DiagnosticContextImpl :
            DiagnosticContext
        {
            public DiagnosticContextImpl(DiagnosticResult result)
            {
                Result = result;
                Timestamp = DateTimeOffset.UtcNow;
            }

            public DiagnosticResult Result { get; }
            public DateTimeOffset Timestamp { get; }
        }
        
        
        class UnsubscribeObserver<T> :
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
    }
}