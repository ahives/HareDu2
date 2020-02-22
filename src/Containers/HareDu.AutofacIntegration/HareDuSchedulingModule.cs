// Copyright 2013-2020 Albert L. Hives
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
namespace HareDu.AutofacIntegration
{
    using System.IO;
    using Autofac;
    using Core;
    using Core.Configuration;
    using Diagnostics;
    using Diagnostics.Persistence;
    using Quartz;
    using Quartz.Impl;
    using Registration;
    using Scheduling;
    using Snapshotting.Persistence;
    using Snapshotting.Registration;

    public class HareDuSchedulingModule :
        Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(x =>
                {
                    var provider = x.Resolve<IFileConfigProvider>();

                    provider.TryGet($"{Directory.GetCurrentDirectory()}/haredu.yaml", out HareDuConfig config);

                    return new BrokerObjectFactory(config.Broker);
                })
                .As<IBrokerObjectFactory>()
                .SingleInstance();

            builder.Register(x => new SnapshotFactory(x.Resolve<IBrokerObjectFactory>()))
                .As<ISnapshotFactory>()
                .SingleInstance();

            builder.Register(x =>
                {
                    var factory = new StdSchedulerFactory();
                    var scheduler = factory
                        .GetScheduler()
                        .ConfigureAwait(false)
                        .GetAwaiter()
                        .GetResult();

                    scheduler.JobFactory =
                        new HareDuJobFactory(
                            x.Resolve<IDiagnosticScanner>(),
                            x.Resolve<ISnapshotFactory>(),
                            x.Resolve<ISnapshotWriter>(),
                            x.Resolve<IDiagnosticWriter>());

                    return scheduler;
                })
                .As<IScheduler>()
                .SingleInstance();

            builder.RegisterType<SnapshotWriter>()
                .As<ISnapshotWriter>()
                .SingleInstance();

            builder.RegisterType<BrokerConfigProvider>()
                .As<IBrokerConfigProvider>()
                .SingleInstance();

            builder.RegisterType<YamlConfigProvider>()
                .As<IFileConfigProvider>()
                .SingleInstance();
            
            base.Load(builder);
        }
    }
}