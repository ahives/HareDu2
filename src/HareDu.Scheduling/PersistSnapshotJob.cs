namespace HareDu.Scheduling
{
    using System.IO;
    using System.Threading.Tasks;
    using Quartz;
    using Snapshotting;
    using Snapshotting.Extensions;

    public class PersistSnapshotJob<T> :
        IJob
        where T : HareDuSnapshot<Snapshot>
    {
        readonly ISnapshotFactory _factory;
        readonly ISnapshotWriter _writer;

        public PersistSnapshotJob(ISnapshotFactory factory, ISnapshotWriter writer)
        {
            _factory = factory;
            _writer = writer;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var snapshot = _factory.Snapshot<T>().Execute();
            var result = snapshot.Timeline.MostRecent();

            _writer.TrySave(result, $"snapshot_{result.Identifier}.json", $"{Directory.GetCurrentDirectory()}/snapshots");
        }
    }
}