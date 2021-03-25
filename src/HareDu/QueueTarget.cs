namespace HareDu
{
    public interface QueueTarget
    {
        /// <summary>
        /// Specify the name of the virtual host.
        /// </summary>
        /// <param name="name">Name of the virtual host being targeted.</param>
        void VirtualHost(string name);
    }
}