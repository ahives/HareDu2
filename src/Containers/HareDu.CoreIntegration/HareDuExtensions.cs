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
    using Diagnostics;
    using Diagnostics.Formatting;
    using Diagnostics.KnowledgeBase;
    using Diagnostics.Persistence;
    using Diagnostics.Registration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Quartz.Impl;
    using Registration;
    using Scheduling;
    using Snapshotting;
    using Snapshotting.Persistence;
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
            services.TryAddSingleton<IBrokerCommunication, BrokerCommunication>();
            
            services.TryAddSingleton<IBrokerConfigProvider, BrokerConfigProvider>();
            
            services.TryAddSingleton<IFileConfigurationProvider, FileConfigurationProvider>();
            
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
            services.TryAddSingleton<IBrokerCommunication, BrokerCommunication>();
            
            services.TryAddSingleton<IBrokerConfigProvider, BrokerConfigProvider>();
            
            services.TryAddSingleton<IFileConfigurationProvider, FileConfigurationProvider>();

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
            services.TryAddSingleton<IBrokerCommunication, BrokerCommunication>();
            
            services.TryAddSingleton<IBrokerConfigProvider, BrokerConfigProvider>();
            
            services.TryAddSingleton<IFileConfigurationProvider, FileConfigurationProvider>();

            services.AddBrokerObjectFactory();
            
            services.TryAddSingleton<ISnapshotFactory>(x => new SnapshotFactory(x.GetService<IBrokerObjectFactory>()));

            services.TryAddSingleton<ISnapshotWriter, SnapshotWriter>();

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
            services.TryAddSingleton<IBrokerCommunication, BrokerCommunication>();
            
            services.TryAddSingleton<IBrokerConfigProvider, BrokerConfigProvider>();
            
            services.TryAddSingleton<IFileConfigurationProvider, FileConfigurationProvider>();

            services.AddBrokerObjectFactory(path);

            services.TryAddSingleton<ISnapshotFactory>(x => new SnapshotFactory(x.GetService<IBrokerObjectFactory>()));

            services.TryAddSingleton<ISnapshotWriter, SnapshotWriter>();

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
            services.TryAddSingleton<IBrokerCommunication, BrokerCommunication>();
            
            services.TryAddSingleton<IDiagnosticsConfigProvider, DiagnosticsConfigProvider>();
            
            services.TryAddSingleton<IFileConfigurationProvider, FileConfigurationProvider>();

            services.TryAddSingleton<IDiagnosticReportFormatter, DiagnosticReportTextFormatter>();

            services.TryAddSingleton<IDiagnosticScanner, DiagnosticScanner>();

            services.TryAddSingleton<IKnowledgeBaseProvider, DefaultKnowledgeBaseProvider>();

            string path = $"{Directory.GetCurrentDirectory()}/haredu.yaml";
            
            services.AddBrokerObjectFactory(path);

            services.TryAddSingleton<ISnapshotFactory>(x => new SnapshotFactory(x.GetService<IBrokerObjectFactory>()));

            services.AddDiagnostics(path);

            services.TryAddSingleton<IDiagnosticWriter, DiagnosticWriter>();

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
            services.TryAddSingleton<IBrokerCommunication, BrokerCommunication>();
            
            services.TryAddSingleton<IDiagnosticsConfigProvider, DiagnosticsConfigProvider>();
            
            services.TryAddSingleton<IFileConfigurationProvider, FileConfigurationProvider>();

            services.TryAddSingleton<IDiagnosticReportFormatter, DiagnosticReportTextFormatter>();

            services.TryAddSingleton<IDiagnosticScanner, DiagnosticScanner>();

            services.TryAddSingleton<IKnowledgeBaseProvider, DefaultKnowledgeBaseProvider>();

            services.AddBrokerObjectFactory(path);

            services.TryAddSingleton<ISnapshotFactory>(x => new SnapshotFactory(x.GetService<IBrokerObjectFactory>()));

            services.AddDiagnostics(path);

            services.TryAddSingleton<IDiagnosticWriter, DiagnosticWriter>();
            
            return services;
        }

        public static IServiceCollection AddHareDuScheduling<T>(this IServiceCollection services)
            where T : HareDuSnapshot<Snapshot>
        {
            services.TryAddSingleton(x =>
            {
                var factory = new StdSchedulerFactory();
                var scheduler = factory
                    .GetScheduler()
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();
        
                scheduler.JobFactory =
                    new HareDuJobFactory<T>(
                        x.GetService<IDiagnosticScanner>(),
                        x.GetService<ISnapshotFactory>(),
                        x.GetService<ISnapshotWriter>(),
                        x.GetService<IDiagnosticWriter>());
        
                return scheduler;
            });
        
            services.TryAddSingleton<IHareDuScheduler, HareDuScheduler>();
        
            return services;
        }

        static void AddBrokerObjectFactory(this IServiceCollection services, string path)
        {
            services.TryAddSingleton<IBrokerObjectFactory>(x =>
            {
                var provider = x.GetService<IFileConfigurationProvider>();

                provider.TryGet($"{Directory.GetCurrentDirectory()}/haredu.yaml", out HareDuConfig config);

                var comm = x.GetService<IBrokerCommunication>();

                return new BrokerObjectFactory(comm.GetClient(config.Broker));
            });
        }

        static void AddBrokerObjectFactory(this IServiceCollection services)
        {
            services.TryAddSingleton<IBrokerObjectFactory>(x =>
            {
                var provider = x.GetService<IFileConfigurationProvider>();

                provider.TryGet($"{Directory.GetCurrentDirectory()}/haredu.yaml", out HareDuConfig config);

                var comm = x.GetService<IBrokerCommunication>();

                return new BrokerObjectFactory(comm.GetClient(config.Broker));
            });
        }

        static void AddDiagnostics(this IServiceCollection services, string path)
        {
            services.TryAddSingleton<IDiagnosticFactory>(x =>
            {
                var configProvider = x.GetService<IFileConfigurationProvider>();

                configProvider.TryGet(path, out HareDuConfig config);

                var knowledgeBaseProvider = x.GetService<IKnowledgeBaseProvider>();
                
                return new DiagnosticFactory(config.Diagnostics, knowledgeBaseProvider);
            });
        }
    }
}