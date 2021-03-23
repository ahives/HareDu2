namespace HareDu.Model
{
    public interface TopicPermissionsInfo
    {
        string User { get; }
        
        string VirtualHost { get; }
        
        string Exchange { get; }
        
        string Write { get; }
        
        string Read { get; }
    }
}