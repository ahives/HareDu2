namespace HareDu.Internal.Resources
{
    using System;
    using Common.Logging;

    internal abstract class Logging
    {
        readonly ILog _logger;
        readonly bool _isEnabled;

        protected Logging(ILog logger)
        {
            _logger = logger;
            _isEnabled = _logger != null;
        }

        protected virtual void LogError(Exception e)
        {
            if (_isEnabled)
                _logger.Error(format => format("[Msg]: {0}, [Stack Trace] {1}", e.Message, e.StackTrace));
        }

        protected virtual void LogError(string message)
        {
            if (_isEnabled)
                _logger.Error(message);
        }

        protected virtual void LogInfo(string message)
        {
            if (_isEnabled)
                _logger.Info(message);
        }
    }
}