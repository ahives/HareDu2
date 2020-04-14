namespace HareDu.PrometheusIntegration
{
    using Diagnostics;
    using Prometheus;

    public static class PrometheusMetricsConfigurator
    {
        static Counter _healthyTotal;
        static Counter _unhealthyTotal;
        static Counter _warningTotal;
        // static Gauge _healthy;
        // static Gauge _unhealthy;
        // static Gauge _warning;

        // string[] _systemLabels 

        public static void Report(AnalyzerSummary summary)
        {
            _healthyTotal.WithLabels(summary.Id).IncTo(summary.Healthy.Total);
            _unhealthyTotal.WithLabels(summary.Id).IncTo(summary.Unhealthy.Total);
            _warningTotal.WithLabels(summary.Id).IncTo(summary.Warning.Total);
        }

        public static void Configure()
        {
            PrometheusMetricsOptions options = PrometheusMetricsOptions.Default;

            string[] serviceLabels = {"haredu"};

            // _healthy = Metrics.CreateGauge(options.Healthy.Label,
            //     options.Healthy.HelpText,
            //     new GaugeConfiguration {LabelNames = serviceLabels});
            //
            // _unhealthy = Metrics.CreateGauge(options.Unhealthy.Label,
            //     options.Unhealthy.HelpText,
            //     new GaugeConfiguration {LabelNames = serviceLabels});
            //
            // _warning = Metrics.CreateGauge(options.Warning.Label,
            //     options.Warning.HelpText,
            //     new GaugeConfiguration {LabelNames = serviceLabels});

            _healthyTotal = Metrics.CreateCounter(options.Healthy.Label,
                options.Healthy.HelpText,
                new CounterConfiguration {LabelNames = serviceLabels});

            _unhealthyTotal = Metrics.CreateCounter(options.Unhealthy.Label,
                options.Unhealthy.HelpText,
                new CounterConfiguration {LabelNames = serviceLabels});

            _warningTotal = Metrics.CreateCounter(options.Warning.Label,
                options.Warning.HelpText,
                new CounterConfiguration {LabelNames = serviceLabels});
        }
    }
}