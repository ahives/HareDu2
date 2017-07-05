namespace HareDu.Internal.Resources
{
    using System.Net.Http;
    using Common.Logging;

    internal class QueueResourceImpl :
        ResourceBase,
        QueueResource
    {
        public QueueResourceImpl(HttpClient client, ILog logger)
            : base(client, logger)
        {
        }

        public BindingResource Binding { get; }
        public void GetAll()
        {
            throw new System.NotImplementedException();
        }
    }
}