namespace HareDu.Model
{
    public interface UserPermissionsInfo
    {
        string User { get; }
        
        string VirtualHost { get; }
        
        string Configure { get; }
        
        string Write { get; }
        
        string Read { get; }
    }
}