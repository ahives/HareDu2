namespace HareDu
{
    using System;

    public interface HareDuClientBehavior
    {
        void ConnectTo(string host);

        void EnableLogging(Action<LoggerSettings> logger);

        void TimeoutAfter(TimeSpan timeout);
    }
}