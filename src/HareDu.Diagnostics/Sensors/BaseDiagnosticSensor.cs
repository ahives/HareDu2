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

    abstract class BaseDiagnosticSensor :
        IObservable<DiagnosticContext>
    {
        protected readonly IDiagnosticSensorConfigProvider _provider;
        readonly List<IObserver<DiagnosticContext>> _observers;

        protected BaseDiagnosticSensor(IDiagnosticSensorConfigProvider provider)
        {
            _provider = provider;
            _observers = new List<IObserver<DiagnosticContext>>();
        }

        protected virtual void NotifyObservers(DiagnosticResult result)
        {
            foreach (var observer in _observers)
            {
                observer.OnNext(new DiagnosticContextImpl(result));
            }
        }

        public IDisposable Subscribe(IObserver<DiagnosticContext> observer)
        {
            if (!_observers.Contains(observer))
                _observers.Add(observer);

            return new UnsubscribeObserver(_observers, observer);
        }

        
        class DiagnosticContextImpl :
            DiagnosticContext
        {
            public DiagnosticContextImpl(DiagnosticResult result)
            {
                Result = result;
                Timestamp = DateTimeOffset.Now;
            }

            public DiagnosticResult Result { get; }
            public DateTimeOffset Timestamp { get; }
        }
        
        
        class UnsubscribeObserver :
            IDisposable
        {
            readonly List<IObserver<DiagnosticContext>> _observers;
            readonly IObserver<DiagnosticContext> _observer;

            public UnsubscribeObserver(List<IObserver<DiagnosticContext>> observers, IObserver<DiagnosticContext> observer)
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