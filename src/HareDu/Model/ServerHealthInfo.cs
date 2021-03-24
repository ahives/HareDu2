namespace HareDu.Model
{
    public interface ServerHealthInfo
    {
        string Status { get; }
        
        string Reason { get; }
    }
}