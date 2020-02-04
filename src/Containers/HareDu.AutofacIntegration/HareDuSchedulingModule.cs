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
    using Autofac;
    using Core;
    using Core.Configuration;
    using Diagnostics.Persistence;
    using Diagnostics.Scanning;
    using Quartz;
    using Quartz.Impl;
    using Registration;
    using Scheduling;
    using Snapshotting;
    using Snapshotting.Persistence;
    using Snapshotting.Registration;

    public class HareDuSchedulingModule :
        Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(x =>
                {
                    var settingsProvider = x.Resolve<IBrokerConfigProvider>();
                    var comm = x.Resolve<IBrokerCommunication>();

                    if (!settingsProvider.TryGet(out BrokerConfig settings))
                        throw new HareDuClientConfigurationException("Settings cannot be null and should at least have user credentials, RabbitMQ server URL and port.");

                    // return new BrokerObjectFactory(comm.GetClient(settings), x.Resolve<IBrokerObjectRegistrar>());
                    return new BrokerObjectFactory(comm.GetClient(settings));
                })
                .As<IBrokerObjectFactory>()
                .SingleInstance();

            builder.Register(x => new SnapshotFactory(
                    x.Resolve<IBrokerObjectFactory>(), x.Resolve<ISnapshotObjectRegistrar>()))
                .As<ISnapshotFactory>()
                .SingleInstance();

            builder.Register(x =>
                {
                    var creator = x.Resolve<ISnapshotInstanceCreator>();
                    var finder = x.Resolve<ISnapshotTypeFinder>();
                    var registrar = new SnapshotObjectRegistrar(finder, creator);

                    registrar.RegisterAll();

                    return registrar;
                })
                .As<ISnapshotObjectRegistrar>()
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

            builder.RegisterType<SnapshotTypeFinder>()
                .As<ISnapshotTypeFinder>()
                .SingleInstance();

            builder.RegisterType<SnapshotInstanceCreator>()
                .As<ISnapshotInstanceCreator>()
                .SingleInstance();

            builder.RegisterType<SnapshotWriter>()
                .As<ISnapshotWriter>()
                .SingleInstance();

            builder.RegisterType<BrokerCommunication>()
                .As<IBrokerCommunication>()
                .SingleInstance();

            builder.RegisterType<BrokerConfigProvider>()
                .As<IBrokerConfigProvider>()
                .SingleInstance();

            builder.RegisterType<ConfigurationProvider>()
                .As<IConfigurationProvider>()
                .SingleInstance();
            
            base.Load(builder);
        }
    }
}