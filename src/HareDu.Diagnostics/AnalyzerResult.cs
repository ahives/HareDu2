namespace HareDu.Diagnostics
{
    public interface AnalyzerResult
    {
        uint Total { get; }

        decimal Percentage { get; }
    }
}