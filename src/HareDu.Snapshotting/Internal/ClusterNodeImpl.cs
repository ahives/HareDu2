// Copyright 2013-2019 Albert L. Hives
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
namespace HareDu.Snapshotting.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using Core.Extensions;
    using HareDu.Model;
    using Model;

    class ClusterNodeImpl :
        BaseSnapshot<NodeSnapshot>,
        ClusterNode
    {
        readonly List<IDisposable> _observers;

        public IReadOnlyList<SnapshotContext<NodeSnapshot>> Timeline { get; }

        public ClusterNodeImpl(IBrokerObjectFactory factory)
            : base(factory)
        {
            _observers = new List<IDisposable>();
        }

        public HareDuSnapshot<NodeSnapshot> Execute(CancellationToken cancellationToken = default)
        {
            var cluster = _factory
                .Object<SystemOverview>()
                .Get(cancellationToken)
                .Select(x => x.Data);

            var nodes = _factory
                .Object<Node>()
                .GetAll(cancellationToken)
                .Select(x => x.Data);
            
            NodeSnapshot snapshot = new NodeSnapshotImpl(cluster, nodes);
            SnapshotContext<NodeSnapshot> context = new SnapshotContextImpl(snapshot);

            SaveSnapshot(context);
            NotifyObservers(context);

            return this;
        }

        public HareDuSnapshot<NodeSnapshot> RegisterObserver(IObserver<SnapshotContext<NodeSnapshot>> observer) => throw new NotImplementedException();

        public HareDuSnapshot<NodeSnapshot> RegisterObservers(IReadOnlyList<IObserver<SnapshotContext<NodeSnapshot>>> observers) => throw new NotImplementedException();


        class NodeSnapshotImpl :
            NodeSnapshot
        {
            public NodeSnapshotImpl(SystemOverviewInfo systemOverview, IReadOnlyList<NodeInfo> nodes)
            {
//                OS = new OperatingSystemMetricsImpl(node);
//                Erlang = new ErlangMetricsImpl(cluster, node);
//                IO = new IOImpl(cluster.MessageStats, node);
//                ContextSwitching = new ContextSwitchDetailsImpl(node);
            }

            public OperatingSystemSnapshot OS { get; }
            public string RatesMode { get; }
            public long Uptime { get; }
            public int RunQueue { get; }
            public long InterNodeHeartbeat { get; }
            public string Identifier { get; }
            public string ClusterIdentifier { get; }
            public string Type { get; }
            public bool IsRunning { get; }
            public ulong AvailableCoresDetected { get; }
            public IReadOnlyList<string> NetworkPartitions { get; }
            public DiskSnapshot Disk { get; }
            public IO IO { get; }
            public BrokerRuntimeSnapshot Runtime { get; }
            public RuntimeDatabase RuntimeDatabase { get; }
            public MemorySnapshot Memory { get; }
//            public GarbageCollection GC { get; }
            public ContextSwitchingDetails ContextSwitching { get; }
            public IReadOnlyList<ConnectionSnapshot> Connections { get; }
        }
    }
}