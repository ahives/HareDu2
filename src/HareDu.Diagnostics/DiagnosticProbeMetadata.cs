namespace HareDu.Diagnostics
{
    public interface DiagnosticProbeMetadata
    {
        string Id { get; }
        
        string Name { get; }
        
        string Description { get; }
    }
}