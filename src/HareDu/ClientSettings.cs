namespace HareDu
{
    using System;

    public interface ClientSettings
    {
        void ConnectTo(string host);

        void EnableLogging(Action<LoggerSettings> logger);

        void TimeoutAfter(TimeSpan timeout);
    }
}