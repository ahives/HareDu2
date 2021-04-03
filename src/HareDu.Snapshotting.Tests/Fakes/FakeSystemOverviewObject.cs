namespace HareDu.Snapshotting.Tests.Fakes
{
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Core.Testing;
    using HareDu.Model;

    public class FakeSystemOverviewObject :
        SystemOverview,
        HareDuTestingFake
    {
        public async Task<Result<SystemOverviewInfo>> Get(CancellationToken cancellationToken = default)
        {
            SystemOverviewInfo data = new FakeSystemOverviewInfo();
            
            return new SuccessfulResult<SystemOverviewInfo>(data, null);
        }
    }
}