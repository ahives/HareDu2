namespace HareDu.Core.Configuration
{
    public interface BrokerCredentials
    {
        string Username { get; }
        
        string Password { get; }
    }
}