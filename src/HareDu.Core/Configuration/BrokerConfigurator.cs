namespace HareDu.Core.Configuration
{
    using System;

    public interface BrokerConfigurator
    {
        /// <summary>
        /// Specify the RabbitMQ server url to connect to.
        /// </summary>
        /// <param name="url"></param>
        void ConnectTo(string url);

        /// <summary>
        /// Specify the maximum time before the HTTP request to the RabbitMQ server will fail.
        /// </summary>
        /// <param name="timeout"></param>
        void TimeoutAfter(TimeSpan timeout);
        
        /// <summary>
        /// Specify the user credentials to connect to the RabbitMQ server.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        void UsingCredentials(string username, string password);
    }
}