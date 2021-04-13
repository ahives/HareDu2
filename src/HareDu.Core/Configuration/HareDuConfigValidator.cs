namespace HareDu.Core.Configuration
{
    using Extensions;

    public class HareDuConfigValidator :
        IConfigValidator
    {
        public bool IsValid(HareDuConfig config)
        {
            if (config.IsNull() ||
                config.Broker.Credentials.IsNull() ||
                string.IsNullOrWhiteSpace(config.Broker.Credentials.Username) ||
                string.IsNullOrWhiteSpace(config.Broker.Credentials.Password) ||
                string.IsNullOrWhiteSpace(config.Broker.Url))
            {
                return false;
            }

            return true;
        }
    }
}