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
namespace HareDu.Snapshotting.Tests
{
    using System.Threading.Tasks;
    using Autofac;
    using Fakes;
    using HareDu.Registration;
    using NUnit.Framework;
    using Observers;
    using Registration;

    [TestFixture]
    public class ClusterSnapshotTests
    {
        IContainer _container;

        [OneTimeSetUp]
        public void Init()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<FakeSnapshotRegistration>()
                .As<ISnapshotRegistration>()
                .SingleInstance();

            builder.RegisterType<FakeBrokerObjectRegistration>()
                .As<IBrokerObjectRegistration>()
                .SingleInstance();
            
            builder.Register(x =>
                {
                    return new FakeBrokerObjectFactory();
                })
                .As<IBrokerObjectFactory>()
                .SingleInstance();

            builder.Register(x =>
                {
                    var registration = x.Resolve<ISnapshotRegistration>();
                    var factory = x.Resolve<IBrokerObjectFactory>();

                    registration.RegisterAll(factory);

                    return new SnapshotFactory(factory, registration.Cache);
                })
                .As<ISnapshotFactory>()
                .SingleInstance();

            _container = builder.Build();
        }

        [Test]
        public async Task Test()
        {
            var resource = _container.Resolve<ISnapshotFactory>()
                .Snapshot<RmqCluster>()
                .RegisterObserver(new DefaultClusterSnapshotConsoleLogger())
                .Execute();
        }
    }
}