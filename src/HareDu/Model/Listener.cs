namespace HareDu.Model
{
    public interface Listener
    {
        string Node { get; }
        
        string Protocol { get; }
        
        string IPAddress { get; }
        
        string Port { get; }
        
        SocketOptions SocketOptions { get; }
    }
}