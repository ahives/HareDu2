namespace HareDu.Diagnostics
{
    public interface AnalyzerSummary
    {
        /// <summary>
        /// 
        /// </summary>
        string Id { get; }
        
        AnalyzerResult Healthy { get; }
        
        AnalyzerResult Unhealthy { get; }
        
        AnalyzerResult Warning { get; }
        
        AnalyzerResult Inconclusive { get; }
    }
}