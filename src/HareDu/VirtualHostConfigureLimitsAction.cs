namespace HareDu
{
    using System;

    public interface VirtualHostConfigureLimitsAction
    {
        /// <summary>
        /// Specify the name of the virtual host.
        /// </summary>
        /// <param name="name">RabbitMQ virtual host name</param>
        void VirtualHost(string name);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        void Configure(Action<VirtualHostLimitsConfiguration> configuration);
    }
}