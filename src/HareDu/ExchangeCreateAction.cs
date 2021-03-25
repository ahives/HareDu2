namespace HareDu
{
    using System;

    public interface ExchangeCreateAction
    {
        /// <summary>
        /// Specify the name of the exchange.
        /// </summary>
        /// <param name="name">RabbitMQ exchange name</param>
        void Exchange(string name);

        /// <summary>
        /// Specify how should the exchange be configured.
        /// </summary>
        /// <param name="configuration">User-defined configuration</param>
        void Configure(Action<ExchangeConfiguration> configuration);

        /// <summary>
        /// Specify the target for which the exchange will be created.
        /// </summary>
        /// <param name="target">Define the location of the exchange (i.e. virtual host) that is targeted for deletion</param>
        void Targeting(Action<ExchangeTarget> target);
    }
}