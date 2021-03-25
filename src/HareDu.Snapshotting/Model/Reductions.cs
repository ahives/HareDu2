namespace HareDu.Snapshotting.Model
{
    public interface Reductions
    {
        /// <summary>
        /// Total number of CPU reductions.
        /// </summary>
        long Total { get; }
        
        /// <summary>
        /// Rate at which CPU reductions are happening.
        /// </summary>
        decimal Rate { get; }
    }
}