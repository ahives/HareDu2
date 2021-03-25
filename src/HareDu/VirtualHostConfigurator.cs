namespace HareDu
{
    using System;

    public interface VirtualHostConfigurator
    {
        /// <summary>
        /// Configuration setting that enables tracing.
        /// </summary>
        void WithTracingEnabled();

        /// <summary>
        /// Metadata used to describe a vhost.
        /// </summary>
        /// <param name="description"></param>
        void Description(string description);

        /// <summary>
        /// Metadata used to define tags on a vhost.
        /// </summary>
        /// <param name="configurator"></param>
        void Tags(Action<VirtualHostTagConfigurator> configurator);
    }
}