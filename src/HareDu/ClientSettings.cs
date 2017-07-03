namespace HareDu
{
    using System;
    using Common.Logging;

    public interface ClientSettings
    {
        string Host { get; }
        ILog Logger { get; }
        TimeSpan Timeout { get; }
    }
}