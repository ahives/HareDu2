namespace HareDu.Internal.Model
{
    using HareDu.Model;

    class InternalCapabilities :
        ConnectionCapabilities
    {
        public InternalCapabilities(ConnectionCapabilitiesImpl obj)
        {
            AuthenticationFailureNotificationEnabled = obj.AuthenticationFailureNotificationEnabled;
            NegativeAcknowledgmentNotificationsEnabled = obj.NegativeAcknowledgmentNotificationsEnabled;
            ConnectionBlockedNotificationsEnabled = obj.ConnectionBlockedNotificationsEnabled;
            ConsumerCancellationNotificationsEnabled = obj.ConsumerCancellationNotificationsEnabled;
            ExchangeBindingEnabled = obj.ExchangeBindingEnabled;
            PublisherConfirmsEnabled = obj.PublisherConfirmsEnabled;
        }

        public bool AuthenticationFailureNotificationEnabled { get; }
        public bool NegativeAcknowledgmentNotificationsEnabled { get; }
        public bool ConnectionBlockedNotificationsEnabled { get; }
        public bool ConsumerCancellationNotificationsEnabled { get; }
        public bool ExchangeBindingEnabled { get; }
        public bool PublisherConfirmsEnabled { get; }
    }
}