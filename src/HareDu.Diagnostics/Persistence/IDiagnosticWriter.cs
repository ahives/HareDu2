namespace HareDu.Diagnostics.Persistence
{
    public interface IDiagnosticWriter
    {
        bool TrySave(ScannerResult result, string file, string path);
    }
}