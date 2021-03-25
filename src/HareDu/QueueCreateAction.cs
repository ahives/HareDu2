namespace HareDu
{
    using System;

    public interface QueueCreateAction
    {
        /// <summary>
        /// Specify the name of the queue.
        /// </summary>
        /// <param name="name">Name of the queue</param>
        void Queue(string name);

        /// <summary>
        /// Specify how the queue should be configured.
        /// </summary>
        /// <param name="configuration">User-defined configuration</param>
        void Configure(Action<QueueConfiguration> configuration);

        /// <summary>
        /// Specify where the queue will live (i.e. virtual host, etc.).
        /// </summary>
        /// <param name="target">Define where the queue will live</param>
        void Targeting(Action<QueueCreateTarget> target);
    }
}