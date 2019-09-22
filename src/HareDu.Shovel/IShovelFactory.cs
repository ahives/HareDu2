namespace HareDu.Shovel
{
    using System;
    using System.Threading;

    public interface IShovelFactory
    {
        void Shovel<TSource, TDestination>(Action<ShovelCreateAction<TSource, TDestination>> action,
            CancellationToken cancellationToken = default)
            where TSource : AMQP091Source
            where TDestination : AMQP091Destination;
    }
}