namespace HareDu.Model
{
    public interface TotalMemoryInfo
    {
        long Erlang { get; }
        
        long Strategy { get; }
        
        long Allocated { get; }
    }
}