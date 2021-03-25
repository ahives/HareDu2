namespace HareDu.Snapshotting.Model
{
    public interface QueueMemoryDetails
    {
        /// <summary>
        /// 
        /// </summary>
        ulong Total { get; }
        
        PagedOut PagedOut { get; }
        
        /// <summary>
        /// 
        /// </summary>
        RAM RAM { get; }
    }
}