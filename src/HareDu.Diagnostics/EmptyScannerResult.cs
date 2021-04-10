namespace HareDu.Diagnostics
{
    using System;
    using System.Collections.Generic;
    using Core.Extensions;
    using MassTransit;
    using Scanners;
    using Snapshotting.Model;

    class EmptyScannerResult :
        ScannerResult
    {
        public Guid Id => NewId.NextGuid();
        public string ScannerId => typeof(NoOpScanner<EmptySnapshot>).GetIdentifier();
        public IReadOnlyList<ProbeResult> Results => new List<ProbeResult>();
        public DateTimeOffset Timestamp => DateTimeOffset.UtcNow;
    }
}