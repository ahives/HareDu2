namespace HareDu.Model
{
    public interface ConnectionCapabilities
    {
        bool AuthenticationFailureNotificationEnabled { get; }

        bool NegativeAcknowledgmentNotificationsEnabled { get; }

        bool ConnectionBlockedNotificationsEnabled { get; }

        bool ConsumerCancellationNotificationsEnabled { get; }

        bool ExchangeBindingEnabled { get; }

        bool PublisherConfirmsEnabled { get; }
    }
}