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
    using System.Linq;
    using System.Threading;
    using Core.Extensions;
    using HareDu.Model;
    using Model;

    class SchemaImpl :
        BaseSnapshot<VirtualHostSchemaSnapshot>,
        Schema
    {
        readonly List<IDisposable> _observers;

        public IReadOnlyList<SnapshotContext<VirtualHostSchemaSnapshot>> Snapshots => _snapshots;

        public SchemaImpl(IBrokerObjectFactory factory)
            : base(factory)
        {
            _observers = new List<IDisposable>();
        }

        public ResourceSnapshot<VirtualHostSchemaSnapshot> Execute(CancellationToken cancellationToken = default)
        {
            var server = _factory
                .Object<Server>()
                .GetDefinition(cancellationToken)
                .Unfold();

            if (server.HasFaulted)
            {
                NotifyObserversOfError(new HareDuSnapshotException("Unable to retrieve cluster information."));
                return this;
            }

            var queues = _factory
                .Object<Queue>()
                .GetAll(cancellationToken)
                .Unfold();

            if (queues.HasFaulted)
            {
                NotifyObserversOfError(new HareDuSnapshotException("Unable to retrieve queue information."));
                return this;
            }

            var exchanges = _factory
                .Object<Exchange>()
                .GetAll(cancellationToken)
                .Unfold();

            if (exchanges.HasFaulted)
            {
                NotifyObserversOfError(new HareDuSnapshotException("Unable to retrieve queue information."));
                return this;
            }
            
            VirtualHostSchemaSnapshot snapshot = new VirtualHostSchemaSnapshotImpl(
                server.Select(x => x.Data),
                queues.Select(x => x.Data),
                exchanges.Select(x => x.Data));
            SnapshotContext<VirtualHostSchemaSnapshot> context = new SnapshotContextImpl(snapshot);

            SaveSnapshot(context);
            NotifyObservers(context);

            return this;
        }

        public ResourceSnapshot<VirtualHostSchemaSnapshot> RegisterObserver(
            IObserver<SnapshotContext<VirtualHostSchemaSnapshot>> observer)
        {
            if (observer != null)
            {
                _observers.Add(Subscribe(observer));
            }

            return this;
        }

        public ResourceSnapshot<VirtualHostSchemaSnapshot> RegisterObservers(
            IReadOnlyList<IObserver<SnapshotContext<VirtualHostSchemaSnapshot>>> observers)
        {
            if (observers != null)
            {
                for (int i = 0; i < observers.Count; i++)
                {
                    _observers.Add(Subscribe(observers[i]));
                }
            }

            return this;
        }

        
        class VirtualHostSchemaSnapshotImpl :
            VirtualHostSchemaSnapshot
        {
            public VirtualHostSchemaSnapshotImpl(ServerDefinitionInfo server, IReadOnlyList<QueueInfo> queues, IReadOnlyList<ExchangeInfo> exchanges)
            {
                var bindings = server
                    .Select(x => x.Bindings)
                    .Where(x => x.DestinationType == "queue")
                    .Select(x => new
                    {
                        x.VirtualHost, x.Source, x.Destination
                    });

                var rollup = new Dictionary<string, IList<ExchangeSnapshot>>();

                foreach (var binding in bindings)
                {
                    var source = new BindingDestinationImpl(binding.Source, BrokerObjectType.Exchange);
                    var destination = new BindingDestinationImpl(binding.Destination, BrokerObjectType.Queue);
                    
                    if (rollup.ContainsKey(binding.VirtualHost))
                        rollup[binding.VirtualHost].Add(new ExchangeSnapshotImpl(source, destination));
                    else
                        rollup[binding.VirtualHost] = new List<ExchangeSnapshot>{new ExchangeSnapshotImpl(source, destination)};
                }

                var vhosts = new List<VirtualHostSchema>();
                foreach (var keyValue in rollup)
                {
                    vhosts.Add(new VirtualHostSchemaImpl(keyValue.Key, keyValue.Value));
                }

                // Schema = vhosts;
                
                
                // var schema = new Dictionary<string, IReadOnlyList<ExchangeSnapshot>>();
                //
                // foreach (var keyValue in rollup)
                // {
                //     schema[keyValue.Key] = keyValue.Value as IReadOnlyList<ExchangeSnapshot>;
                // }
                //
                // Schema = schema;
            }

            class VirtualHostSchemaImpl : VirtualHostSchema
            {
                public VirtualHostSchemaImpl(string name, IList<ExchangeSnapshot> definition)
                {
                    Name = name;
                    Definition = definition as IReadOnlyList<ExchangeSnapshot>;
                }

                public string Name { get; }
                public IReadOnlyList<ExchangeSnapshot> Definition { get; }
            }

            class BindingDestinationImpl : BindingDestination
            {
                public BindingDestinationImpl(string name, BrokerObjectType sourceType)
                {
                    Name = name;
                    SourceType = sourceType;
                }

                public string Name { get; }
                public BrokerObjectType SourceType { get; }
            }

            class ExchangeSnapshotImpl :
                ExchangeSnapshot
            {
                public ExchangeSnapshotImpl(BindingDestination source, BindingDestination destination)
                {
                    Source = source;
                    Destination = destination;
                }

                public BindingDestination Source { get; }
                public BindingDestination Destination { get; }
            }

            public string Node { get; }
            public IReadOnlyList<VirtualHostSchema> VirtualHosts { get; }
            // public IDictionary<string, IReadOnlyList<ExchangeSnapshot>> Schema { get; }
        }
    }
}