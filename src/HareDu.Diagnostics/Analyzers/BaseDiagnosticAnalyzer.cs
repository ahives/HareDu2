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
namespace HareDu.Diagnostics.Analyzers
{
    using System;
    using System.Collections.Generic;
    using Configuration;
    using KnowledgeBase;

    public abstract class BaseDiagnosticAnalyzer :
        IObservable<DiagnosticAnalyzerContext>
    {
        protected readonly IDiagnosticScannerConfigProvider _configProvider;
        protected readonly IKnowledgeBaseProvider _knowledgeBaseProvider;
        protected DiagnosticScannerConfig _config;
        protected DiagnosticAnalyzerStatus _status;
        readonly List<IObserver<DiagnosticAnalyzerContext>> _observers;

        protected BaseDiagnosticAnalyzer(IDiagnosticScannerConfigProvider configProvider, IKnowledgeBaseProvider knowledgeBaseProvider)
        {
            _configProvider = configProvider;
            _knowledgeBaseProvider = knowledgeBaseProvider;
            _observers = new List<IObserver<DiagnosticAnalyzerContext>>();
        }

        protected virtual void NotifyObservers(DiagnosticResult result)
        {
            foreach (var observer in _observers)
            {
                observer.OnNext(new DiagnosticAnalyzerContextImpl(result));
            }
        }

        public IDisposable Subscribe(IObserver<DiagnosticAnalyzerContext> observer)
        {
            if (!_observers.Contains(observer))
                _observers.Add(observer);

            return new UnsubscribeObserver<DiagnosticAnalyzerContext>(_observers, observer);
        }


        class DiagnosticAnalyzerContextImpl :
            DiagnosticAnalyzerContext
        {
            public DiagnosticAnalyzerContextImpl(DiagnosticResult result)
            {
                Result = result;
                Timestamp = DateTimeOffset.UtcNow;
            }

            public DiagnosticResult Result { get; }
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
    }
}