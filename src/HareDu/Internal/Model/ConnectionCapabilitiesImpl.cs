namespace HareDu.Internal.Model
{
    using System.Text.Json.Serialization;

    class ConnectionCapabilitiesImpl
    {
        [JsonPropertyName("authentication_failure_close")]
        public bool AuthenticationFailureNotificationEnabled { get; set; }

        [JsonPropertyName("basic.nack")]
        public bool NegativeAcknowledgmentNotificationsEnabled { get; set; }

        [JsonPropertyName("connection.blocked")]
        public bool ConnectionBlockedNotificationsEnabled { get; set; }

        [JsonPropertyName("consumer_cancel_notify")]
        public bool ConsumerCancellationNotificationsEnabled { get; set; }

        [JsonPropertyName("exchange_exchange_bindings")]
        public bool ExchangeBindingEnabled { get; set; }

        [JsonPropertyName("publisher_confirms")]
        public bool PublisherConfirmsEnabled { get; set; }
    }
}