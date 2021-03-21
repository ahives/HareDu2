namespace HareDu.Snapshotting.Tests.Fakes
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Core.Testing;
    using HareDu.Model;

    public class FakeQueueObject :
        Queue,
        HareDuTestingFake
    {
        public async Task<ResultList<QueueInfo>> GetAll(CancellationToken cancellationToken = default)
        {
            QueueInfo channel = new FakeQueueInfo();

            List<QueueInfo> data = new List<QueueInfo> {channel};

            return new SuccessfulResultList<QueueInfo>(data, null);
        }

        public async Task<Result> Create(Action<QueueCreateAction> action, CancellationToken cancellationToken = default) => throw new NotImplementedException();

        public async Task<Result> Delete(Action<QueueDeleteAction> action, CancellationToken cancellationToken = default) => throw new NotImplementedException();

        public async Task<Result> Empty(Action<QueueEmptyAction> action, CancellationToken cancellationToken = default) => throw new NotImplementedException();

        public async Task<ResultList<PeekedMessageInfo>> Peek(Action<QueuePeekAction> action, CancellationToken cancellationToken = default) => throw new NotImplementedException();

        public async Task<Result> Create(string queue, string vhost, string node, Action<QueueConfigurator> configurator = null,
            CancellationToken cancellationToken = default) =>
            throw new NotImplementedException();

        public async Task<Result> Delete(string queue, string vhost, Action<QueueDeletionConfigurator> configurator = null, CancellationToken cancellationToken = default) => throw new NotImplementedException();

        public async Task<Result> Empty(string queue, string vhost, CancellationToken cancellationToken = default) => throw new NotImplementedException();

        public async Task<Result> Sync(string queue, string vhost, QueueSyncAction syncAction, CancellationToken cancellationToken = default) => throw new NotImplementedException();
    }
}