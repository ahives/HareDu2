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
    using Registration;
    using Snapshotting.Persistence;
    using Snapshotting.Registration;

    public class HareDuSnapshotModule :
        Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(x =>
                {
                    var provider = x.Resolve<IFileConfigProvider>();
                    string file = $"{Directory.GetCurrentDirectory()}/haredu.yaml";

                    provider.TryGet(file, out HareDuConfig config);

                    var validator = x.Resolve<IConfigValidator>();

                    if (!validator.Validate(config))
                        throw new HareDuConfigurationException($"Invalid settings in {file}.");
                    
                    return new BrokerObjectFactory(config.Broker);
                })
                .As<IBrokerObjectFactory>()
                .SingleInstance();

            builder.RegisterType<SnapshotFactory>()
                   .As<ISnapshotFactory>()
                   .SingleInstance();
            
            builder.RegisterType<SnapshotWriter>()
                .As<ISnapshotWriter>()
                .SingleInstance();

            builder.RegisterType<BrokerConfigProvider>()
                .As<IBrokerConfigProvider>()
                .SingleInstance();

            builder.RegisterType<YamlFileConfigProvider>()
                .As<IFileConfigProvider>()
                .SingleInstance();

            builder.RegisterType<YamlConfigProvider>()
                .As<IConfigProvider>()
                .SingleInstance();

            builder.RegisterType<HareDuConfigValidator>()
                .As<IConfigValidator>()
                .SingleInstance();
            
            base.Load(builder);
        }
    }
}