namespace HareDu.Snapshotting.Tests.Fakes
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Core.Testing;
    using HareDu.Model;

    public class FakeNodeObject :
        Node,
        HareDuTestingFake
    {
        public async Task<ResultList<NodeInfo>> GetAll(CancellationToken cancellationToken = default)
        {
            NodeInfo node1 = new FakeNodeInfo();
            NodeInfo node2 = new FakeNodeInfo();

            List<NodeInfo> data = new List<NodeInfo> {node1, node2};

            return new SuccessfulResultList<NodeInfo>(data, null);
        }

        public async Task<Result<NodeHealthInfo>> GetHealth(string node = null, CancellationToken cancellationToken = default) => throw new System.NotImplementedException();

        public async Task<Result<NodeMemoryUsageInfo>> GetMemoryUsage(string node, CancellationToken cancellationToken = default) => throw new System.NotImplementedException();
    }
}