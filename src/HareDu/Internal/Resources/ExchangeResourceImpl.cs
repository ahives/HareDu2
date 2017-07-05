namespace HareDu.Internal.Resources
{
    using System.Net.Http;
    using Common.Logging;

    internal class ExchangeResourceImpl :
        ResourceBase,
        ExchangeResource
    {
        public ExchangeResourceImpl(HttpClient client, ILog logger)
            : base(client, logger)
        {
        }

        public BindingResource Binding { get; }
    }
}