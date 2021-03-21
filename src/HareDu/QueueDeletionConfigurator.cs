namespace HareDu
{
    public interface QueueDeletionConfigurator
    {
        /// <summary>
        /// Specify acceptable conditions for which the queue can be deleted.
        /// </summary>
        /// <param name="condition"></param>
        void WhenHasNoConsumers();

        /// <summary>
        /// Prevent delete actions on the specified queue from being successful if the queue contains messages.
        /// </summary>
        void WhenEmpty();
    }
}