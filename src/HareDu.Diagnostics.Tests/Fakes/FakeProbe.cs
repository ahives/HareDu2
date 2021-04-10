namespace HareDu.Diagnostics.Tests.Fakes
{
    using System;
    using Diagnostics.Probes;
    using KnowledgeBase;

    public class FakeProbe :
        BaseDiagnosticProbe,
        DiagnosticProbe
    {
        public IDisposable Subscribe(IObserver<ProbeContext> observer) => throw new NotImplementedException();

        public DiagnosticProbeMetadata Metadata =>
            new DiagnosticProbeMetadataImpl<FakeProbe>("Fake Probe", "");
        public ComponentType ComponentType { get; }
        public ProbeCategory Category { get; }

        public FakeProbe(IKnowledgeBaseProvider kb) : base(kb)
        {
        }
        
        public ProbeResult Execute<T>(T snapshot) => new InconclusiveProbeResult(
            string.Empty, string.Empty, string.Empty, string.Empty, ComponentType.NA, new ProbeData[]{}, new MissingKnowledgeBaseArticle(string.Empty, ProbeResultStatus.Inconclusive));
    }
}