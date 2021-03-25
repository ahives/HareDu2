namespace HareDu
{
    using System;

    public interface VirtualHostCreateAction
    {
        /// <summary>
        /// Specify the name of the virtual host.
        /// </summary>
        /// <param name="name">RabbitMQ virtual host name</param>
        void VirtualHost(string name);

        /// <summary>
        /// Specify how should the virtual host be configured.
        /// </summary>
        /// <param name="configurator">User-defined configuration</param>
        void Configure(Action<VirtualHostConfigurator> configurator);
    }
}