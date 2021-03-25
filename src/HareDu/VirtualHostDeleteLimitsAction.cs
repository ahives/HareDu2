namespace HareDu
{
    public interface VirtualHostDeleteLimitsAction
    {
        /// <summary>
        /// Defines which virtual host will limits be deleted for.
        /// </summary>
        /// <param name="vhost">RabbitMQ virtual host name</param>
        void For(string vhost);
    }
}