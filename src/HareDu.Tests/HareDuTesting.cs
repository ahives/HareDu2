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
namespace HareDu.Tests
{
    using Autofac;
    using Core;
    using Core.Configuration;
    using Fakes;
    using NUnit.Framework;
    using Registration;

    public class HareDuTesting
    {
        protected ContainerBuilder GetContainerBuilder(string path)
        {
            var builder = new ContainerBuilder();

            builder.Register(x =>
                {
                    var registration = x.Resolve<IBrokerObjectRegistry>();
                    var settingsProvider = x.Resolve<IBrokerConfigProvider>();
                    var connection = x.Resolve<IBrokerConnectionClient>();

                    if (!settingsProvider.TryGet(out BrokerConfig config))
                        throw new HareDuClientConfigurationException(
                            "Settings cannot be null and should at least have user credentials, RabbitMQ server URL and port.");
                    
                    var client = connection.Create(config);

                    registration.RegisterAll(client);

                    return new BrokerObjectFactory(client, registration.ObjectCache);
                })
                .As<IBrokerObjectFactory>()
                .SingleInstance();

            builder.Register(x => new FakeBrokerConnectionClient(path))
                .As<IBrokerConnectionClient>()
                .SingleInstance();

            builder.RegisterType<BrokerObjectRegistry>()
                .As<IBrokerObjectRegistry>()
                .SingleInstance();

            builder.RegisterType<BrokerConfigProvider>()
                .As<IBrokerConfigProvider>()
                .SingleInstance();

            builder.RegisterType<ConfigurationProvider>()
                .As<IConfigurationProvider>()
                .SingleInstance();

            return builder;
        }

        protected ContainerBuilder GetContainerBuilder()
        {
            var builder = new ContainerBuilder();

            builder.Register(x =>
                {
                    var registration = x.Resolve<IBrokerObjectRegistry>();
                    var settingsProvider = x.Resolve<IBrokerConfigProvider>();
                    var connection = x.Resolve<IBrokerConnectionClient>();

                    if (!settingsProvider.TryGet(out BrokerConfig config))
                        throw new HareDuClientConfigurationException(
                            "Settings cannot be null and should at least have user credentials, RabbitMQ server URL and port.");

                    var client = connection.Create(config);

                    registration.RegisterAll(client);

                    return new BrokerObjectFactory(client, registration.ObjectCache);
                })
                .As<IBrokerObjectFactory>()
                .SingleInstance();

            builder.Register(x => new FakeBrokerConnectionClient())
                .As<IBrokerConnectionClient>()
                .SingleInstance();

            builder.RegisterType<BrokerObjectRegistry>()
                .As<IBrokerObjectRegistry>()
                .SingleInstance();

            builder.RegisterType<BrokerConfigProvider>()
                .As<IBrokerConfigProvider>()
                .SingleInstance();

            builder.RegisterType<ConfigurationProvider>()
                .As<IConfigurationProvider>()
                .SingleInstance();

            return builder;
        }
    }
}