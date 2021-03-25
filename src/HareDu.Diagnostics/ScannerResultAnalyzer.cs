namespace HareDu.Diagnostics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Extensions;
    using MassTransit;

    public class ScannerResultAnalyzer :
        IScannerResultAnalyzer,
        IObservable<AnalyzerContext>
    {
        readonly IList<IDisposable> _disposableObservers;
        readonly List<IObserver<AnalyzerContext>> _observers;

        public ScannerResultAnalyzer()
        {
            _observers = new List<IObserver<AnalyzerContext>>();
            _disposableObservers = new List<IDisposable>();
        }

        public IReadOnlyList<AnalyzerSummary> Analyze(ScannerResult report, Func<ProbeResult, string> filterBy)
        {
            var rollup = GetRollup(report.Results, filterBy);
            
            var summary = (from result in rollup
                    let healthy = new AnalyzerResultImpl(
                        result.Value
                            .Count(x => x == ProbeResultStatus.Healthy)
                            .ConvertTo(),
                        CalcPercentage(result.Value, ProbeResultStatus.Healthy))
                    let unhealthy = new AnalyzerResultImpl(
                        result.Value
                            .Count(x => x == ProbeResultStatus.Unhealthy)
                            .ConvertTo(),
                        CalcPercentage(result.Value, ProbeResultStatus.Unhealthy))
                    let warning = new AnalyzerResultImpl(
                        result.Value
                            .Count(x => x == ProbeResultStatus.Warning)
                            .ConvertTo(),
                        CalcPercentage(result.Value, ProbeResultStatus.Warning))
                    let inconclusive = new AnalyzerResultImpl(
                        result.Value
                            .Count(x => x == ProbeResultStatus.Inconclusive)
                            .ConvertTo(),
                        CalcPercentage(result.Value, ProbeResultStatus.Inconclusive))
                    select new AnalyzerSummaryImpl(result.Key, healthy, unhealthy, warning, inconclusive))
                .Cast<AnalyzerSummary>()
                .ToList();

            NotifyObservers(summary);
            
            return summary;
        }

        public IScannerResultAnalyzer RegisterObserver(IObserver<AnalyzerContext> observer)
        {
            _disposableObservers.Add(Subscribe(observer));

            return this;
        }

        public IDisposable Subscribe(IObserver<AnalyzerContext> observer)
        {
            if (!_observers.Contains(observer))
                _observers.Add(observer);

            return new UnsubscribeObserver(_observers, observer);
        }

        void NotifyObservers(List<AnalyzerSummary> result)
        {
            foreach (var observer in _observers)
            {
                observer.OnNext(new AnalyzerContextImpl(result));
            }
        }

        decimal CalcPercentage(List<ProbeResultStatus> results, ProbeResultStatus status)
        {
            decimal totalCount = Convert.ToDecimal(results.Count);
            decimal statusCount = Convert.ToDecimal(results.Count(x => x == status));
            
            return Convert.ToDecimal(statusCount / totalCount * 100);
        }

        IDictionary<string, List<ProbeResultStatus>> GetRollup(IReadOnlyList<ProbeResult> results, Func<ProbeResult, string> filterBy)
        {
            var rollup = new Dictionary<string, List<ProbeResultStatus>>();

            for (int i = 0; i < results.Count; i++)
            {
                string key = filterBy(results[i]);

                if (rollup.ContainsKey(key))
                    rollup[key].Add(results[i].Status);
                else
                    rollup.Add(key, new List<ProbeResultStatus> {results[i].Status});
            }

            return rollup;
        }

        
        class AnalyzerContextImpl :
            AnalyzerContext
        {
            public AnalyzerContextImpl(List<AnalyzerSummary> summary)
            {
                Summary = summary;
                Id = NewId.NextGuid();
                Timestamp = DateTimeOffset.UtcNow;
            }

            public Guid Id { get; }
            public IReadOnlyList<AnalyzerSummary> Summary { get; }
            public DateTimeOffset Timestamp { get; }
        }


        class UnsubscribeObserver :
            IDisposable
        {
            readonly List<IObserver<AnalyzerContext>> _observers;
            readonly IObserver<AnalyzerContext> _observer;

            public UnsubscribeObserver(List<IObserver<AnalyzerContext>> observers, IObserver<AnalyzerContext> observer)
            {
                _observers = observers;
                _observer = observer;
            }

            public void Dispose()
            {
                if (!_observer.IsNull() && _observers.Contains(_observer))
                    _observers.Remove(_observer);
            }
        }


        class AnalyzerSummaryImpl :
            AnalyzerSummary
        {
            public AnalyzerSummaryImpl(string id, AnalyzerResult healthy, AnalyzerResult unhealthy,
                AnalyzerResult warning, AnalyzerResult inconclusive)
            {
                Id = id;
                Healthy = healthy;
                Unhealthy = unhealthy;
                Warning = warning;
                Inconclusive = inconclusive;
            }

            public string Id { get; }
            public AnalyzerResult Healthy { get; }
            public AnalyzerResult Unhealthy { get; }
            public AnalyzerResult Warning { get; }
            public AnalyzerResult Inconclusive { get; }
        }

        
        class AnalyzerResultImpl :
            AnalyzerResult
        {
            public AnalyzerResultImpl(uint total, decimal percentage)
            {
                Total = total;
                Percentage = percentage;
            }

            public uint Total { get; }
            public decimal Percentage { get; }
        }
    }
}