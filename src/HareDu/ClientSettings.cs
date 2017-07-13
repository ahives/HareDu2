namespace HareDu
{
    using System;
    using System.Net;
    using Common.Logging;

    public interface ClientSettings
    {
        string Host { get; }
        ILog Logger { get; }
        TimeSpan Timeout { get; }
        HareDuCredentials Credentials { get; }
    }
}