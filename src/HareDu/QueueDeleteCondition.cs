namespace HareDu
{
    public interface QueueDeleteCondition
    {
        /// <summary>
        /// Prevent delete actions on the specified queue from being successful if the queue has consumers.
        /// </summary>
        void HasNoConsumers();
        
        /// <summary>
        /// Prevent delete actions on the specified queue from being successful if the queue contains messages.
        /// </summary>
        void IsEmpty();
    }
}