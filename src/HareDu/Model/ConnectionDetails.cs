namespace HareDu.Model
{
    public interface ConnectionDetails
    {
        string PeerHost { get; }

        long PeerPort { get; }

        string Name { get; }
    }
}