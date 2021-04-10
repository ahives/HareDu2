namespace HareDu.Diagnostics.Scanners
{
    public class BaseDiagnosticScanner
    {
        protected class DiagnosticScannerMetadataImpl :
            DiagnosticScannerMetadata
        {
            public DiagnosticScannerMetadataImpl(string identifier)
            {
                Identifier = identifier;
            }

            public string Identifier { get; }
        }
    }
}