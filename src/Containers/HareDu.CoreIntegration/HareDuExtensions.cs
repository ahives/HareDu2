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
namespace HareDu.CoreIntegration
{
    using System.IO;
    using Core;
    using Core.Configuration;
    using Diagnostics.Formatting;
    using Diagnostics.KnowledgeBase;
    using Diagnostics.Registration;
    using Diagnostics.Scanning;
    using Microsoft.Extensions.DependencyInjection;
    using Quartz;
    using Quartz.Impl;
    using Registration;
    using Scheduling;
    using Snapshotting;
    using Snapshotting.Registration;

    public static class HareDuExtensions
    {
        /// <summary>
        /// Registers all the necessary components to use the low level HareDu Broker Object API.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        /// <exception cref="HareDuClientConfigurationException"></exception>
        public static IServiceCollection AddHareDu(this IServiceCollection services)
        {
            services.AddSingleton<IBrokerCommunication, BrokerCommunication>();
            
            services.AddSingleton<IBrokerConfigProvider, BrokerConfigProvider>();
            
            services.AddSingleton<IConfigurationProvider, ConfigurationProvider>();

            services.AddBrokerObjectFactory();

            return services;
        }

        /// <summary>
        /// Registers all the necessary components to use the low level HareDu Broker Object API.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="HareDuClientConfigurationException"></exception>
        public static IServiceCollection AddHareDu(this IServiceCollection services, string path)
        {
            services.AddSingleton<IBrokerCommunication, BrokerCommunication>();
            
            services.AddSingleton<IBrokerConfigProvider, BrokerConfigProvider>();
            
            services.AddSingleton<IConfigurationProvider, ConfigurationProvider>();

            services.AddBrokerObjectFactory(path);

            return services;
        }

        /// <summary>
        /// Registers all the necessary components to use the HareDu Snapshotting API.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddHareDuSnapshotting(this IServiceCollection services)
        {
            services.AddSingleton<ISnapshotObjectRegistrar, SnapshotObjectRegistrar>();

            services.AddSingleton<IBrokerObjectRegistrar, BrokerObjectRegistrar>();
            
            services.AddSingleton<IBrokerCommunication, BrokerCommunication>();
            
            services.AddSingleton<IBrokerConfigProvider, BrokerConfigProvider>();
            
            services.AddSingleton<IConfigurationProvider, ConfigurationProvider>();

            services.AddBrokerObjectFactory();

            services.AddSnapshotObjectRegistry();
            
            services.AddSingleton<ISnapshotFactory>(x => new SnapshotFactory(
                x.GetService<IBrokerObjectFactory>(), x.GetService<ISnapshotObjectRegistrar>()));

            services.AddSingleton<ISnapshotWriter, SnapshotWriter>();

            return services;
        }

        /// <summary>
        /// Registers all the necessary components to use the HareDu Snapshotting API.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="HareDuClientConfigurationException"></exception>
        public static IServiceCollection AddHareDuSnapshotting(this IServiceCollection services, string path)
        {
            services.AddSingleton<ISnapshotObjectRegistrar, SnapshotObjectRegistrar>();

            services.AddSingleton<IBrokerObjectRegistrar, BrokerObjectRegistrar>();
            
            services.AddSingleton<IBrokerCommunication, BrokerCommunication>();
            
            services.AddSingleton<IBrokerConfigProvider, BrokerConfigProvider>();
            
            services.AddSingleton<IConfigurationProvider, ConfigurationProvider>();

            services.AddBrokerObjectFactory(path);

            services.AddSnapshotObjectRegistry();

            services.AddSingleton<ISnapshotFactory>(x => new SnapshotFactory(
                x.GetService<IBrokerObjectFactory>(), x.GetService<ISnapshotObjectRegistrar>()));

            services.AddSingleton<ISnapshotWriter, SnapshotWriter>();

            return services;
        }

        /// <summary>
        /// Registers all the necessary components to use the HareDu Diagnostics API.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        /// <exception cref="HareDuClientConfigurationException"></exception>
        public static IServiceCollection AddHareDuDiagnostics(this IServiceCollection services)
        {
            services.AddSingleton<ISnapshotObjectRegistrar, SnapshotObjectRegistrar>();

            services.AddSingleton<IBrokerObjectRegistrar, BrokerObjectRegistrar>();
            
            services.AddSingleton<IBrokerCommunication, BrokerCommunication>();
            
            services.AddSingleton<IBrokerConfigProvider, BrokerConfigProvider>();
            
            services.AddSingleton<IConfigurationProvider, ConfigurationProvider>();

            services.AddSingleton<IDiagnosticReportFormatter, DiagnosticReportTextFormatter>();

            services.AddSingleton<IDiagnosticScanner, DiagnosticScanner>();

            services.AddSingleton<IKnowledgeBaseProvider, DefaultKnowledgeBaseProvider>();

            string path = $"{Directory.GetCurrentDirectory()}/config.yaml";
            
            services.AddBrokerObjectFactory(path);

            services.AddSnapshotObjectRegistry();

            services.AddSingleton<ISnapshotFactory>(x => new SnapshotFactory(
                x.GetService<IBrokerObjectFactory>(), x.GetService<ISnapshotObjectRegistrar>()));

            services.AddDiagnosticAnalyzerRegistry(path);

            services.AddComponentDiagnosticRegistry();

            services.AddSingleton<IComponentDiagnosticFactory>(x => new ComponentDiagnosticFactory(
                x.GetService<IDiagnosticAnalyzerRegistrar>(), x.GetService<IComponentDiagnosticRegistrar>()));

            return services;
        }

        /// <summary>
        /// Registers all the necessary components to use the HareDu Diagnostics API.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="HareDuClientConfigurationException"></exception>
        public static IServiceCollection AddHareDuDiagnostics(this IServiceCollection services, string path)
        {
            services.AddSingleton<ISnapshotObjectRegistrar, SnapshotObjectRegistrar>();

            services.AddSingleton<IBrokerObjectRegistrar, BrokerObjectRegistrar>();
            
            services.AddSingleton<IBrokerCommunication, BrokerCommunication>();
            
            services.AddSingleton<IBrokerConfigProvider, BrokerConfigProvider>();
            
            services.AddSingleton<IConfigurationProvider, ConfigurationProvider>();

            services.AddSingleton<IDiagnosticReportFormatter, DiagnosticReportTextFormatter>();

            services.AddSingleton<IDiagnosticScanner, DiagnosticScanner>();

            services.AddSingleton<IKnowledgeBaseProvider, DefaultKnowledgeBaseProvider>();

            services.AddBrokerObjectFactory(path);

            services.AddSnapshotObjectRegistry();

            services.AddSingleton<ISnapshotFactory>(x => new SnapshotFactory(
                x.GetService<IBrokerObjectFactory>(), x.GetService<ISnapshotObjectRegistrar>()));

            services.AddDiagnosticAnalyzerRegistry(path);

            services.AddComponentDiagnosticRegistry();

            services.AddSingleton<IComponentDiagnosticFactory>(x => new ComponentDiagnosticFactory(
                x.GetService<IDiagnosticAnalyzerRegistrar>(), x.GetService<IComponentDiagnosticRegistrar>()));

            return services;
        }

        public static IServiceCollection AddSnapshotScheduling<T>(this IServiceCollection services)
            where T : HareDuSnapshot<Snapshot>
        {
            services.AddSingleton(x =>
            {
                var factory = new StdSchedulerFactory();
                var scheduler = factory
                    .GetScheduler()
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();

                scheduler.JobFactory =
                    new HareDuJobFactory<T>(x.GetService<ISnapshotFactory>(), x.GetService<ISnapshotWriter>());

                return scheduler;
            });

            services.AddSingleton<ISnapshotScheduler, SnapshotScheduler>();

            return services;
        }

        static void AddBrokerObjectFactory(this IServiceCollection services, string path)
        {
            services.AddSingleton<IBrokerObjectFactory>(x =>
            {
                var settingsProvider = x.GetService<IBrokerConfigProvider>();
                var comm = x.GetService<IBrokerCommunication>();

                if (!settingsProvider.TryGet(path, out BrokerConfig config))
                    throw new HareDuClientConfigurationException(
                        "Settings cannot be null and should at least have user credentials, RabbitMQ server URL and port.");
                
                return new BrokerObjectFactory(comm.GetClient(config), x.GetService<IBrokerObjectRegistrar>());
            });
        }

        static void AddBrokerObjectFactory(this IServiceCollection services)
        {
            services.AddSingleton<IBrokerObjectFactory>(x =>
            {
                var settingsProvider = x.GetService<IBrokerConfigProvider>();
                var comm = x.GetService<IBrokerCommunication>();

                if (!settingsProvider.TryGet(out BrokerConfig config))
                    throw new HareDuClientConfigurationException(
                        "Settings cannot be null and should at least have user credentials, RabbitMQ server URL and port.");

                return new BrokerObjectFactory(comm.GetClient(config), x.GetService<IBrokerObjectRegistrar>());
            });
        }

        static void AddSnapshotObjectRegistry(this IServiceCollection services)
        {
            services.AddSingleton<ISnapshotObjectRegistrar>(x =>
            {
                var registrar = new SnapshotObjectRegistrar(x.GetService<IBrokerObjectFactory>());
                
                registrar.RegisterAll();

                return registrar;
            });
        }

        static void AddDiagnosticAnalyzerRegistry(this IServiceCollection services, string path)
        {
            services.AddSingleton<IDiagnosticAnalyzerRegistrar>(x =>
            {
                var configProvider = x.GetService<IConfigurationProvider>();

                configProvider.TryGet(path, out HareDuConfig config);

                var knowledgeBaseProvider = x.GetService<IKnowledgeBaseProvider>();
                var registrar = new DiagnosticAnalyzerRegistrar(config.Analyzer, knowledgeBaseProvider);
                
                registrar.RegisterAll();

                return registrar;
            });
        }

        static void AddComponentDiagnosticRegistry(this IServiceCollection services)
        {
            services.AddSingleton<IComponentDiagnosticRegistrar>(x =>
            {
                var registrar = new ComponentDiagnosticRegistrar(x.GetService<IDiagnosticAnalyzerRegistrar>());
                
                registrar.RegisterAll();

                return registrar;
            });
        }
    }
}