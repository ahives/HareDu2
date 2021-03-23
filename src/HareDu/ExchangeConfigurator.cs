namespace HareDu
{
    using System;

    public interface ExchangeConfigurator
    {
        /// <summary>
        /// Specify the message routing type (e.g. fanout, direct, topic, etc.).
        /// </summary>
        /// <param name="routingType"></param>
        void HasRoutingType(ExchangeRoutingType routingType);

        /// <summary>
        /// Specify that the exchange is durable.
        /// </summary>
        void IsDurable();

        /// <summary>
        /// Specify that the exchange is for internal use only.
        /// </summary>
        void IsForInternalUse();

        /// <summary>
        /// Specify user-defined arguments used to configure the exchange.
        /// </summary>
        /// <param name="arguments"></param>
        void HasArguments(Action<ExchangeArgumentConfigurator> arguments);

        /// <summary>
        /// Specify that the exchange will be deleted when there are no consumers.
        /// </summary>
        void AutoDeleteWhenNotInUse();
    }
}