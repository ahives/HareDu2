namespace HareDu.Core.Configuration
{
    public interface IConfigProvider
    {
        bool TryGet(string text, out HareDuConfig config);
    }
}