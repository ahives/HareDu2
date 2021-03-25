namespace HareDu.Snapshotting.Model
{
    public interface OperatingSystemSnapshot :
        Snapshot
    {
        string NodeIdentifier { get; }
        
        string ProcessId { get; }

        FileDescriptorChurnMetrics FileDescriptors { get; }
        
        SocketDescriptorChurnMetrics SocketDescriptors { get; }
    }
}