namespace HareDu
{
    using System;

    public interface QueueConfiguration
    {
        /// <summary>
        /// Specify whether the queue is durable. By default this is set to false, which means that it will created as a RAM queue.
        /// </summary>
        void IsDurable();

        /// <summary>
        /// Specify arguments for the queue.
        /// </summary>
        /// <param name="arguments">Pre-defined arguments applied to the definition of the queue.</param>
        void HasArguments(Action<QueueCreateArguments> arguments);

        /// <summary>
        /// Specify whether the queue is deleted when there are no consumers.
        /// </summary>
        void AutoDeleteWhenNotInUse();
    }
}