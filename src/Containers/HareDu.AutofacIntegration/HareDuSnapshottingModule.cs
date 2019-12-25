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
namespace HareDu.AutofacIntegration
{
    using Autofac;
    using Core;
    using Core.Configuration;
    using Registration;
    using Snapshotting;
    using Snapshotting.Registration;

    public class HareDuSnapshottingModule :
        Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(x =>
                {
                    var registry = x.Resolve<IBrokerObjectRegistry>();
                    var settingsProvider = x.Resolve<IBrokerConfigProvider>();
                    var comm = x.Resolve<IBrokerCommunication>();

                    if (!settingsProvider.TryGet(out BrokerConfig settings))
                        throw new HareDuClientConfigurationException("Settings cannot be null and should at least have user credentials, RabbitMQ server URL and port.");
                    
                    var client = comm.GetClient(settings);

                    return new BrokerObjectFactory(client, registry.ObjectCache);
                })
                .As<IBrokerObjectFactory>()
                .SingleInstance();

            builder.Register(x =>
                {
                    var registry = x.Resolve<ISnapshotObjectRegistry>();
                    var factory = x.Resolve<IBrokerObjectFactory>();

                    return new SnapshotFactory(factory, registry.ObjectCache);
                })
                .As<ISnapshotFactory>()
                .SingleInstance();

            builder.Register(x =>
                {
                    var factory = x.Resolve<IBrokerObjectFactory>();
                    var registry = new SnapshotObjectRegistry(factory);

                    registry.RegisterAll();

                    return registry;
                })
                .As<ISnapshotObjectRegistry>()
                .SingleInstance();
            
            builder.Register(x =>
                {
                    var comm = x.Resolve<IBrokerCommunication>();
                    var configProvider = x.Resolve<IBrokerConfigProvider>();

                    if (!configProvider.TryGet(out var config))
                        throw new HareDuClientConfigurationException(
                            "Settings cannot be null and should at least have user credentials, RabbitMQ server URL and port.");

                    var registry = new BrokerObjectRegistry();

                    registry.RegisterAll(comm.GetClient(config));

                    return registry;
                })
                .As<IBrokerObjectRegistry>()
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