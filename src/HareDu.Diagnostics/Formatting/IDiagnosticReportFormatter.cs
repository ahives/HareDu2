namespace HareDu.Diagnostics.Formatting
{
    public interface IDiagnosticReportFormatter
    {
        string Format(ScannerResult report);
    }
}