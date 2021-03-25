namespace HareDu.Snapshotting.Model
{
    public interface RAM
    {
        ulong Target { get; }
        
        /// <summary>
        /// Total messages in RAM that are written to disk.
        /// </summary>
        ulong Total { get; }
        
        /// <summary>
        /// Total size in bytes of the messages that were written to disk from RAM.
        /// </summary>
        ulong Bytes { get; }
        
        ulong Unacknowledged { get; }
        
        ulong Ready { get; }
    }
}