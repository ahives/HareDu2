﻿// Copyright 2013-2020 Albert L. Hives
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
    using Registration;

    public static class HareDuExtensions
    {
        /// <summary>
        /// Registers all the necessary components to use the low level HareDu Broker Object API.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddHareDu(this IServiceCollection services)
        {
            HareDuConfig config = GetConfig($"{Directory.GetCurrentDirectory()}/haredu.yaml");
            
            services.Register(config);
            
            return services;
        }

        /// <summary>
        /// Registers all the necessary components to use the low level HareDu Broker Object API.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public static IServiceCollection AddHareDu(this IServiceCollection services, string file)
        {
            HareDuConfig config = GetConfig(file);
            
            services.Register(config);
            
            return services;
        }

        static void Register(this IServiceCollection services, HareDuConfig config)
        {
            services.TryAddSingleton<IBrokerConfigProvider, BrokerConfigProvider>();
            
            services.TryAddSingleton<IFileConfigProvider, YamlFileConfigProvider>();
            
            services.TryAddSingleton<IConfigProvider, YamlConfigProvider>();
            
            services.TryAddSingleton<IConfigValidator, HareDuConfigValidator>();

            services.TryAddSingleton<IBrokerObjectFactory>(x =>
                new BrokerObjectFactory(config.Broker));
        }

        static HareDuConfig GetConfig(string file)
        {
            IFileConfigProvider provider = new YamlFileConfigProvider();
            
            if (!provider.TryGet(file, out HareDuConfig config))
                throw new HareDuConfigurationException($"Not able to get settings from {file}.");

            IConfigValidator validator = new HareDuConfigValidator();

            if (!validator.IsValid(config))
                throw new HareDuConfigurationException($"Invalid settings in {file}.");

            return config;
        }
    }
}