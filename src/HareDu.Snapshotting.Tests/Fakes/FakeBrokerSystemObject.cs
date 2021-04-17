namespace HareDu.Snapshotting.Tests.Fakes
{
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Core.Testing;
    using HareDu.Model;

    public class FakeBrokerSystemObject :
        BrokerSystem,
        HareDuTestingFake
    {
        public async Task<Result<SystemOverviewInfo>> GetSystemOverview(CancellationToken cancellationToken = default)
        {
            SystemOverviewInfo data = new FakeSystemOverviewInfo();
            
            return new SuccessfulResult<SystemOverviewInfo>(data, null);
        }

        public async Task<Result> RebalanceAllQueues(CancellationToken cancellationToken = default) => throw new System.NotImplementedException();
    }
}