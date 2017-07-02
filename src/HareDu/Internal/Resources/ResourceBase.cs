namespace HareDu.Internal.Resources
{
    using System.Net.Http;
    using Common.Logging;

    internal class ResourceBase :
        Logging
    {
        readonly HttpClient _client;

        protected ResourceBase(HttpClient client, ILog logger)
            : base(logger)
        {
            _client = client;
        }
    }
}