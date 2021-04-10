namespace HareDu.Diagnostics.Probes
{
    public interface DiagnosticProbeMetadata
    {
        string Id { get; }
        
        string Name { get; }
        
        string Description { get; }
    }
}