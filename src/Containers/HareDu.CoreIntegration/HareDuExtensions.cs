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
    using Diagnostics.KnowledgeBase;
    using Diagnostics.Registration;
    using Diagnostics.Scanning;
    using Microsoft.Extensions.DependencyInjection;
    using Registration;
    using Snapshotting;
    using Snapshotting.Registration;

    public static class HareDuExtensions
    {
        public static IServiceCollection AddHareDu(this IServiceCollection services)
        {
            services.AddSingleton<IBrokerCommunication, BrokerCommunication>();
            
            services.AddSingleton<IBrokerConfigProvider, BrokerConfigProvider>();
            
            services.AddSingleton<IConfigurationProvider, ConfigurationProvider>();
            
            services.AddSingleton<IBrokerObjectFactory>(x =>
            {
                var settingsProvider = x.GetService<IBrokerConfigProvider>();
                var comm = x.GetService<IBrokerCommunication>();

                if (!settingsProvider.TryGet(out BrokerConfig config))
                    throw new HareDuClientConfigurationException(
                        "Settings cannot be null and should at least have user credentials, RabbitMQ server URL and port.");

                return new BrokerObjectFactory(comm.GetClient(config));
            });

            return services;
        }

        public static IServiceCollection AddHareDu(this IServiceCollection services, string path)
        {
            services.AddSingleton<IBrokerCommunication, BrokerCommunication>();
            
            services.AddSingleton<IBrokerConfigProvider, BrokerConfigProvider>();
            
            services.AddSingleton<IConfigurationProvider, ConfigurationProvider>();
            
            services.AddSingleton<IBrokerObjectFactory>(x =>
            {
                var settingsProvider = x.GetService<IBrokerConfigProvider>();
                var comm = x.GetService<IBrokerCommunication>();

                if (!settingsProvider.TryGet(path, out BrokerConfig config))
                    throw new HareDuClientConfigurationException(
                        "Settings cannot be null and should at least have user credentials, RabbitMQ server URL and port.");

                return new BrokerObjectFactory(comm.GetClient(config));
            });

            return services;
        }

        public static IServiceCollection AddHareDuSnapshotting(this IServiceCollection services)
        {
            services.AddSingleton<ISnapshotObjectRegistry, SnapshotObjectRegistry>();

            services.AddSingleton<IBrokerObjectRegistry, BrokerObjectRegistry>();
            
            services.AddSingleton<IBrokerCommunication, BrokerCommunication>();
            
            services.AddSingleton<IBrokerConfigProvider, BrokerConfigProvider>();
            
            services.AddSingleton<IConfigurationProvider, ConfigurationProvider>();

            services.AddSingleton<IBrokerObjectFactory>(x =>
            {
                var settingsProvider = x.GetService<IBrokerConfigProvider>();
                var comm = x.GetService<IBrokerCommunication>();

                if (!settingsProvider.TryGet(out BrokerConfig config))
                    throw new HareDuClientConfigurationException(
                        "Settings cannot be null and should at least have user credentials, RabbitMQ server URL and port.");

                return new BrokerObjectFactory(comm.GetClient(config));
            });

            services.AddSingleton<ISnapshotFactory>(x =>
            {
                var registry = x.GetService<ISnapshotObjectRegistry>();
                var factory = x.GetService<IBrokerObjectFactory>();

                registry.RegisterAll();

                return new SnapshotFactory(factory, registry.ObjectCache);
            });

            return services;
        }

        public static IServiceCollection AddHareDuSnapshotting(this IServiceCollection services, string path)
        {
            services.AddSingleton<ISnapshotObjectRegistry, SnapshotObjectRegistry>();

            services.AddSingleton<IBrokerObjectRegistry, BrokerObjectRegistry>();
            
            services.AddSingleton<IBrokerCommunication, BrokerCommunication>();
            
            services.AddSingleton<IBrokerConfigProvider, BrokerConfigProvider>();
            
            services.AddSingleton<IConfigurationProvider, ConfigurationProvider>();

            services.AddSingleton<IBrokerObjectFactory>(x =>
            {
                var settingsProvider = x.GetService<IBrokerConfigProvider>();
                var comm = x.GetService<IBrokerCommunication>();

                if (!settingsProvider.TryGet(path, out BrokerConfig config))
                    throw new HareDuClientConfigurationException(
                        "Settings cannot be null and should at least have user credentials, RabbitMQ server URL and port.");

                return new BrokerObjectFactory(comm.GetClient(config));
            });

            services.AddSingleton<ISnapshotFactory>(x =>
            {
                var registry = x.GetService<ISnapshotObjectRegistry>();
                var factory = x.GetService<IBrokerObjectFactory>();

                registry.RegisterAll();

                return new SnapshotFactory(factory, registry.ObjectCache);
            });

            return services;
        }

        public static IServiceCollection AddHareDuDiagnostics(this IServiceCollection services)
        {
            services.AddSingleton<ISnapshotObjectRegistry, SnapshotObjectRegistry>();

            services.AddSingleton<IBrokerObjectRegistry, BrokerObjectRegistry>();
            
            services.AddSingleton<IBrokerCommunication, BrokerCommunication>();
            
            services.AddSingleton<IBrokerConfigProvider, BrokerConfigProvider>();
            
            services.AddSingleton<IConfigurationProvider, ConfigurationProvider>();

            services.AddSingleton<IBrokerObjectFactory>(x =>
            {
                var settingsProvider = x.GetService<IBrokerConfigProvider>();
                var comm = x.GetService<IBrokerCommunication>();

                if (!settingsProvider.TryGet(out BrokerConfig config))
                    throw new HareDuClientConfigurationException(
                        "Settings cannot be null and should at least have user credentials, RabbitMQ server URL and port.");

                return new BrokerObjectFactory(comm.GetClient(config));
            });

            services.AddSingleton<ISnapshotFactory>(x =>
            {
                var registry = x.GetService<ISnapshotObjectRegistry>();
                var factory = x.GetService<IBrokerObjectFactory>();

                registry.RegisterAll();

                return new SnapshotFactory(factory, registry.ObjectCache);
            });

            services.AddSingleton<IComponentDiagnosticFactory>(x =>
            {
                var analyzerRegistry = x.GetService<IDiagnosticAnalyzerRegistry>();
                    
                analyzerRegistry.RegisterAll();

                var diagnosticRegistry = x.GetService<IComponentDiagnosticRegistry>();
                    
                diagnosticRegistry.RegisterAll();

                return new ComponentDiagnosticFactory(diagnosticRegistry.ObjectCache, diagnosticRegistry.Types, analyzerRegistry.ObjectCache);
            });

            services.AddSingleton<IDiagnosticAnalyzerRegistry>(x =>
            {
                var configProvider = x.GetService<IConfigurationProvider>();
                string path = $"{Directory.GetCurrentDirectory()}/config.yaml";

                configProvider.TryGet(path, out HareDuConfig config);

                var knowledgeBaseProvider = x.GetService<IKnowledgeBaseProvider>();
                    
                return new DiagnosticAnalyzerRegistry(config.Analyzer, knowledgeBaseProvider);
            });

            services.AddSingleton<IComponentDiagnosticRegistry>(x =>
            {
                var analyzerRegistry = x.GetService<IDiagnosticAnalyzerRegistry>();

                analyzerRegistry.RegisterAll();

                return new ComponentDiagnosticRegistry(analyzerRegistry.ObjectCache);
            });

            return services;
        }

        public static IServiceCollection AddHareDuDiagnostics(this IServiceCollection services, string path)
        {
            services.AddSingleton<ISnapshotObjectRegistry, SnapshotObjectRegistry>();

            services.AddSingleton<IBrokerObjectRegistry, BrokerObjectRegistry>();
            
            services.AddSingleton<IBrokerCommunication, BrokerCommunication>();
            
            services.AddSingleton<IBrokerConfigProvider, BrokerConfigProvider>();
            
            services.AddSingleton<IConfigurationProvider, ConfigurationProvider>();

            services.AddSingleton<IBrokerObjectFactory>(x =>
            {
                var settingsProvider = x.GetService<IBrokerConfigProvider>();
                var comm = x.GetService<IBrokerCommunication>();

                if (!settingsProvider.TryGet(path, out BrokerConfig config))
                    throw new HareDuClientConfigurationException(
                        "Settings cannot be null and should at least have user credentials, RabbitMQ server URL and port.");

                return new BrokerObjectFactory(comm.GetClient(config));
            });

            services.AddSingleton<ISnapshotFactory>(x =>
            {
                var registry = x.GetService<ISnapshotObjectRegistry>();
                var factory = x.GetService<IBrokerObjectFactory>();

                registry.RegisterAll();

                return new SnapshotFactory(factory, registry.ObjectCache);
            });

            services.AddSingleton<IComponentDiagnosticFactory>(x =>
            {
                var analyzerRegistry = x.GetService<IDiagnosticAnalyzerRegistry>();
                    
                analyzerRegistry.RegisterAll();

                var diagnosticRegistry = x.GetService<IComponentDiagnosticRegistry>();
                    
                diagnosticRegistry.RegisterAll();

                return new ComponentDiagnosticFactory(diagnosticRegistry.ObjectCache, diagnosticRegistry.Types, analyzerRegistry.ObjectCache);
            });

            services.AddSingleton<IDiagnosticAnalyzerRegistry>(x =>
            {
                var configProvider = x.GetService<IConfigurationProvider>();

                configProvider.TryGet(path, out HareDuConfig config);

                var knowledgeBaseProvider = x.GetService<IKnowledgeBaseProvider>();
                    
                return new DiagnosticAnalyzerRegistry(config.Analyzer, knowledgeBaseProvider);
            });

            services.AddSingleton<IComponentDiagnosticRegistry>(x =>
            {
                var analyzerRegistry = x.GetService<IDiagnosticAnalyzerRegistry>();

                analyzerRegistry.RegisterAll();

                return new ComponentDiagnosticRegistry(analyzerRegistry.ObjectCache);
            });

            return services;
        }
    }
}