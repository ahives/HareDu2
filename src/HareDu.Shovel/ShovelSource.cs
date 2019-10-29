namespace HareDu.Shovel
{
    using System;

    public interface ShovelSource
    {
//        void Uri(Action<ShovelUriBuilder<AMQP091ShovelUri>> builder);
        void Uri(Action<ShovelUriBuilder> builder);
        
        void Queue(string name);

        void Exchange(string name, string routingKey);

        void PrefetchCount(int prefetchCount);

        void DeleteAfter(string value);

        void Protocol(ShovelProtocol protocol);
    }

    public enum ShovelProtocol
    {
        AMQP_091,
        AMQP_10
    }

    public interface Amq10Source :
        ShovelSource
    {
    }
}