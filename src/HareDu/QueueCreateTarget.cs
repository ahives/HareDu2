namespace HareDu
{
    public interface QueueCreateTarget
    {
        /// <summary>
        /// Specify the node for which the queue will be created.
        /// </summary>
        /// <param name="node"></param>
        void Node(string node);

        /// <summary>
        /// Specify the virtual host for which the queue will be created.
        /// </summary>
        /// <param name="name">Name of the virtual host being targeted.</param>
        void VirtualHost(string name);
    }
}