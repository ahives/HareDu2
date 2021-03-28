namespace HareDu.Core.Configuration
{
    public interface IFileConfigProvider
    {
        bool TryGet(string path, out HareDuConfig config);
    }
}