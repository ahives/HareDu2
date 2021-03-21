namespace HareDu.Model
{
    using System.Text.Json.Serialization;

    public interface ConnectionCapabilities
    {
        [JsonPropertyName("authentication_failure_close")]
        bool AuthenticationFailureNotificationEnabled { get; }

        [JsonPropertyName("basic.nack")]
        bool NegativeAcknowledgmentNotificationsEnabled { get; }

        [JsonPropertyName("connection.blocked")]
        bool ConnectionBlockedNotificationsEnabled { get; }

        [JsonPropertyName("consumer_cancel_notify")]
        bool ConsumerCancellationNotificationsEnabled { get; }

        [JsonPropertyName("exchange_exchange_bindings")]
        bool ExchangeBindingEnabled { get; }

        [JsonPropertyName("publisher_confirms")]
        bool PublisherConfirmsEnabled { get; }
    }
}