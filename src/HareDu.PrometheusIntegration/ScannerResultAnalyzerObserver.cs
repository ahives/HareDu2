namespace HareDu.PrometheusIntegration
{
    using System;
    using Diagnostics;

    public class ScannerResultAnalyzerObserver :
        IObserver<AnalyzerContext>
    {
        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(AnalyzerContext value)
        {
            for (int i = 0; i < value.Summary.Count; i++)
            {
                PrometheusMetricsConfigurator.Report(value.Summary[i]);
            }
        }
    }
}