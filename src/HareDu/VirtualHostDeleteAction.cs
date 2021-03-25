namespace HareDu
{
    public interface VirtualHostDeleteAction
    {
        /// <summary>
        /// Define which virtual host will be deleted.
        /// </summary>
        /// <param name="name">RabbitMQ virtual host name</param>
        void VirtualHost(string name);
    }
}