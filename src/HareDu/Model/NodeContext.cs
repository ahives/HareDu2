namespace HareDu.Model
{
    public interface NodeContext
    {
        string Description { get; }

        string Path { get; }
        
        string Port { get; }
    }
}