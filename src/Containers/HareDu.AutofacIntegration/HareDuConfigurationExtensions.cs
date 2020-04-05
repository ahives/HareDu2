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

    public static class HareDuConfigurationExtensions
    {
        public static ContainerBuilder AddHareDuConfiguration(this ContainerBuilder builder)
        {
            builder.Register($"{Directory.GetCurrentDirectory()}/haredu.yaml");
            
            return builder;
        }

        public static ContainerBuilder AddHareDuConfiguration(this ContainerBuilder builder, string file)
        {
            builder.Register(file);
            
            return builder;
        }

        static void Register(this ContainerBuilder builder, string file)
        {
            builder.RegisterType<HareDuConfigProvider>()
                .As<IHareDuConfigProvider>();
            
            builder.RegisterType<DiagnosticsConfigProvider>()
                .As<IDiagnosticsConfigProvider>();

            builder.RegisterType<BrokerConfigProvider>()
                .As<IBrokerConfigProvider>();
            
            builder.RegisterType<YamlFileConfigProvider>()
                .As<IFileConfigProvider>();
            
            builder.RegisterType<YamlConfigProvider>()
                .As<IConfigProvider>();
            
            builder.RegisterType<HareDuConfigValidator>()
                .As<IConfigValidator>();

            builder.Register(x =>
            {
                var provider = x.Resolve<IFileConfigProvider>();
            
                if (!provider.TryGet(file, out HareDuConfig config))
                    throw new HareDuConfigurationException($"Not able to get settings from {file}.");

                var validator = x.Resolve<IConfigValidator>();

                if (!validator.IsValid(config))
                    throw new HareDuConfigurationException($"Invalid settings in {file}.");

                return config;
            });
        }
    }
}