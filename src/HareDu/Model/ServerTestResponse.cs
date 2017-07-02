namespace HareDu.Model
{
    public interface ServerTestResponse :
        ServerResponse
    {
        string Status { get; }
    }
}