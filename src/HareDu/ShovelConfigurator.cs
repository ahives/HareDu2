namespace HareDu
{
    using System;

    public interface ShovelConfigurator
    {
        /// <summary>
        /// The duration to wait before reconnecting to the brokers after being disconnected at either end.
        /// </summary>
        /// <param name="delayInSeconds"></param>
        void ReconnectDelay(int delayInSeconds);

        /// <summary>
        /// Determine how the shovel should acknowledge consumed messages.
        /// </summary>
        /// <param name="mode">Define how the shovel will acknowledge consumed messages.</param>
        void AcknowledgementMode(AckMode mode);
        
        /// <summary>
        /// Describes how the shovel is confirmed from the source end.
        /// </summary>
        /// <param name="queue">The name of the source queue.</param>
        /// <param name="configurator">Describes the source shovel.</param>
        void Source(string queue, Action<ShovelSourceConfigurator> configurator = null);
        
        /// <summary>
        /// Describes how the shovel is confirmed from the destination end.
        /// </summary>
        /// <param name="queue">The name of the destination queue.</param>
        /// <param name="configurator">Describes the destination shovel.</param>
        void Destination(string queue, Action<ShovelDestinationConfigurator> configurator = null);
    }
}