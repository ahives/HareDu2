namespace HareDu.Core.Configuration
{
    public interface IConfigValidator
    {
        bool IsValid(HareDuConfig config);
    }
}