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
                    var registration = x.Resolve<IBrokerObjectRegistration>();
                    var settingsProvider = x.Resolve<IBrokerConfigProvider>();
                    var brokerConnection = x.Resolve<IBrokerConnectionClient>();

                    if (!settingsProvider.TryGet(out BrokerConfig settings))
                        throw new HareDuClientConfigurationException("Settings cannot be null and should at least have user credentials, RabbitMQ server URL and port.");
                    
                    var client = brokerConnection.Create(settings);

                    registration.RegisterAll(client);

                    return new BrokerObjectFactory(registration.Cache, client);
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

            builder.RegisterType<SnapshotRegistration>()
                .As<ISnapshotRegistration>()
                .SingleInstance();

            builder.RegisterType<BrokerObjectRegistration>()
                .As<IBrokerObjectRegistration>()
                .SingleInstance();

            builder.RegisterType<BrokerConnectionClient>()
                .As<IBrokerConnectionClient>()
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