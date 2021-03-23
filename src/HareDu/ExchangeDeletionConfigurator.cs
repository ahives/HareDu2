namespace HareDu
{
    public interface ExchangeDeletionConfigurator
    {
        /// <summary>
        /// Specify the conditions for which the exchange can be deleted.
        /// </summary>
        /// <param name="condition"></param>
        void WhenUnused();
    }
}