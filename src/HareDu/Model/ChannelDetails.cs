namespace HareDu.Model
{
    public interface ChannelDetails
    {
        string PeerHost { get; }
        
        long PeerPort { get; }
        
        long Number { get; }

        string Name { get; }

        string Node { get; }

        string ConnectionName { get; }

        string User { get; }
    }
}