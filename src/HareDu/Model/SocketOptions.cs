namespace HareDu.Model
{
    public interface SocketOptions
    {
        long Backlog { get; }
        
        bool NoDelay { get; }
        
        bool ExitOnClose { get; }
    }
}