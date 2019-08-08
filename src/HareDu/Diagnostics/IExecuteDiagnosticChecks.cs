namespace HareDu.Diagnostics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public interface IExecuteDiagnosticChecks
    {
        DiagnosticCheckReport Execute();
    }

    public interface DiagnosticCheckReport
    {
        IReadOnlyList<DiagnosticResult> Results { get; }
    }

    public static class DiagnosticCheck
    {
        static IReadOnlyList<IDiagnosticCheck> _checks;

        public static DiagnosticCheckReport Run<T>(this T snapshot)
        {
            throw new NotImplementedException();
        }
    }
}