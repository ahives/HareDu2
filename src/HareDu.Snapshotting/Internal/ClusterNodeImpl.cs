namespace HareDu.Snapshotting.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Core.Extensions;
    using Core.Model;
    using Extensions;
    using Model;

    class ClusterNodeImpl :
        BaseSnapshot<NodeSnapshot>,
        ClusterNode
    {
        readonly List<IDisposable> _observers;
        
        public ClusterNodeImpl(IResourceFactory factory)
            : base(factory)
        {
            _observers = new List<IDisposable>();
        }

        public ResourceSnapshot<NodeSnapshot> TakeSnapshot(CancellationToken cancellationToken = default)
        {
            var cluster = _factory
                .Resource<Cluster>()
                .GetDetails(cancellationToken)
                .Select(x => x.Data);

            var nodes = _factory
                .Resource<Node>()
                .GetAll(cancellationToken)
                .Select(x => x.Data);
            
            NodeSnapshot data = new NodeSnapshotImpl(cluster, nodes);

            NotifyObservers(data);
            
            var snapshot = new SuccessfulResult<NodeSnapshot>(data, null);

            return this;
        }

        public ResourceSnapshot<NodeSnapshot> RegisterObserver(IObserver<SnapshotContext<NodeSnapshot>> observer) => throw new NotImplementedException();

        public ResourceSnapshot<NodeSnapshot> RegisterObservers(IReadOnlyList<IObserver<SnapshotContext<NodeSnapshot>>> observers) => throw new NotImplementedException();
        public IReadOnlyList<Result<NodeSnapshot>> Snapshots { get; }


        class NodeSnapshotImpl :
            NodeSnapshot
        {
            public NodeSnapshotImpl(ClusterInfo cluster, IReadOnlyList<NodeInfo> nodes)
            {
//                OS = new OperatingSystemMetricsImpl(node);
//                Erlang = new ErlangMetricsImpl(cluster, node);
//                IO = new IOImpl(cluster.MessageStats, node);
//                ContextSwitching = new ContextSwitchDetailsImpl(node);
            }

            public OperatingSystemDetails OS { get; }
            public string RatesMode { get; }
            public long Uptime { get; }
            public int RunQueue { get; }
            public long InterNodeHeartbeat { get; }
            public string Name { get; }
            public string Type { get; }
            public bool IsRunning { get; }
            public IList<string> NetworkPartitions { get; }
            public DiskSnapshot Disk { get; }
            public IO IO { get; }
            public BrokerRuntimeSnapshot Runtime { get; }
            public Mnesia Mnesia { get; }
            public MemorySnapshot Memory { get; }
            public GarbageCollection GC { get; }
            public ContextSwitchingDetails ContextSwitching { get; }
            public IReadOnlyList<ConnectionSnapshot> Connections { get; }
        }
    }
}