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
namespace HareDu.CoreIntegration
{
    using System.IO;
    using Core;
    using Core.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;

    public static class HareDuConfigurationExtensions
    {
        public static IServiceCollection AddHareDuConfiguration(this IServiceCollection services)
        {
            services.Register($"{Directory.GetCurrentDirectory()}/haredu.yaml");
            
            return services;
        }

        public static IServiceCollection AddHareDuConfiguration(this IServiceCollection services, string file)
        {
            services.Register(file);
            
            return services;
        }

        static void Register(this IServiceCollection services, string file)
        {
            services.TryAddSingleton<IHareDuConfigProvider, HareDuConfigProvider>();
            
            services.TryAddSingleton<IDiagnosticsConfigProvider, DiagnosticsConfigProvider>();

            services.TryAddSingleton<IBrokerConfigProvider, BrokerConfigProvider>();
            
            services.TryAddSingleton<IFileConfigProvider, YamlFileConfigProvider>();
            
            services.TryAddSingleton<IConfigProvider, YamlConfigProvider>();
            
            services.TryAddSingleton<IConfigValidator, HareDuConfigValidator>();

            services.TryAddSingleton(x =>
            {
                var provider = x.GetService<IFileConfigProvider>();
            
                if (!provider.TryGet(file, out HareDuConfig config))
                    throw new HareDuConfigurationException($"Not able to get settings from {file}.");

                var validator = x.GetService<IConfigValidator>();

                if (!validator.IsValid(config))
                    throw new HareDuConfigurationException($"Invalid settings in {file}.");

                return config;
            });
        }
    }
}