namespace HareDu.Internal.Resources
{
    using Common.Logging;

    internal class Logging
    {
        readonly ILog _logger;

        public Logging(ILog logger)
        {
            _logger = logger;
        }
    }
}