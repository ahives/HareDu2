namespace HareDu.PrometheusIntegration
{
    public class PrometheusMetricsOptions
    {
        public MetricOption Healthy { get; }
        
        public MetricOption Unhealthy { get; }
        
        public MetricOption Warning { get; }

        public PrometheusMetricsOptions()
        {
            Healthy = new MetricOptionImpl("haredu_healthy", "");
            Unhealthy = new MetricOptionImpl("haredu_unhealthy", "");
            Warning = new MetricOptionImpl("haredu_warning", "");
        }

        class MetricOptionImpl :
            MetricOption
        {
            public MetricOptionImpl(string label, string helpText)
            {
                Label = label;
                HelpText = helpText;
            }

            public string Label { get; }
            public string HelpText { get; }
        }

        public static PrometheusMetricsOptions Default => new PrometheusMetricsOptions();
    }

    public interface MetricOption
    {
        string Label { get; }
        
        string HelpText { get; }
    }
}