namespace HareDu.Model
{
    public interface UserInfo
    {
        string Username { get; }
        
        string PasswordHash { get; }
        
        string HashingAlgorithm { get; }
        
        string Tags { get; }
    }
}