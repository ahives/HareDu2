namespace HareDu.Diagnostics
{
    using System;
    using System.Collections.Generic;
    using MassTransit;

    public class SuccessfulScannerResult :
        ScannerResult
    {
        public SuccessfulScannerResult(string scannerIdentifier, IReadOnlyList<ProbeResult> results)
        {
            Id = NewId.NextGuid();
            ScannerId = scannerIdentifier;
            Results = results;
            Timestamp = DateTimeOffset.Now;
        }

        public Guid Id { get; }
        public string ScannerId { get; }
        public IReadOnlyList<ProbeResult> Results { get; }
        public DateTimeOffset Timestamp { get; }
    }
}