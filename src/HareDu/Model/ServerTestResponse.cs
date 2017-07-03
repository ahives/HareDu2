namespace HareDu.Model
{
    public interface ServerTestResponse :
        Result
    {
        string Status { get; }
    }
}