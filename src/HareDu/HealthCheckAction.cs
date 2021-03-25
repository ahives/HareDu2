namespace HareDu
{
    public interface HealthCheckAction
    {
        /// <summary>
        /// Specify the name of the virtual host for which the health check should be performed on.
        /// </summary>
        /// <param name="name"></param>
        void VirtualHost(string name);

        /// <summary>
        /// Specify the name of the node for which the health check should be performed on.
        /// </summary>
        /// <param name="name"></param>
        void Node(string name);
    }
}