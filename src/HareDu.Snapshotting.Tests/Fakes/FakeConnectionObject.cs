namespace HareDu.Snapshotting.Tests.Fakes
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Core.Testing;
    using HareDu.Model;

    public class FakeConnectionObject :
        Connection,
        HareDuTestingFake
    {
        public async Task<ResultList<ConnectionInfo>> GetAll(CancellationToken cancellationToken = default)
        {
            ConnectionInfo connection1 = new FakeConnectionInfo(1, 1);
            ConnectionInfo connection2 = new FakeConnectionInfo(2, 1);
            ConnectionInfo connection3 = new FakeConnectionInfo(3, 1);

            List<ConnectionInfo> data = new List<ConnectionInfo> {connection1, connection2, connection3};

            return new SuccessfulResultList<ConnectionInfo>(data, null);
        }

        public async Task<Result> Delete(string connection, CancellationToken cancellationToken = default) => throw new System.NotImplementedException();
    }
}