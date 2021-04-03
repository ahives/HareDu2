namespace HareDu.Snapshotting.Tests.Fakes
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Core.Testing;
    using HareDu.Model;

    public class FakeChannelObject :
        Channel,
        HareDuTestingFake
    {
        public async Task<ResultList<ChannelInfo>> GetAll(CancellationToken cancellationToken = default)
        {
            ChannelInfo channel = new FakeChannelInfo(1, 1);

            List<ChannelInfo> data = new List<ChannelInfo> {channel};

            return new SuccessfulResultList<ChannelInfo>(data, null);
        }
    }
}