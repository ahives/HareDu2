namespace HareDu.CoreIntegration
{
    using System;
    using Core.Configuration;
    using Core.Extensions;
    using Diagnostics;
    using Diagnostics.KnowledgeBase;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Snapshotting;

    public static class HareDuExtensions
    {
        /// <summary>
        /// Registers all the necessary components to use the low level HareDu Broker Object API.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddHareDu(this IServiceCollection services)
        {
            services.TryAddSingleton<IBrokerObjectFactory>(x =>
                new BrokerObjectFactory(x.GetService<HareDuConfig>()));
            
            return services;
        }
        
        /// <summary>
        /// Registers all the necessary components to use the low level HareDu Broker, Diagnostic, and Snapshotting APIs.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="settingsFile">The full path of where the configuration settings file is located.</param>
        /// <param name="configSection">The section found within the configuration file.</param>
        /// <returns></returns>
        public static IServiceCollection AddHareDu(this IServiceCollection services, string settingsFile = "appsettings.json", string configSection = "HareDuConfig")
        {
            HareDuConfig config = new HareDuConfigImpl();
            
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile(settingsFile, false)
                .Build();

            configuration.Bind(configSection, config);

            services.AddSingleton(config);
            services.AddSingleton<IBrokerObjectFactory, BrokerObjectFactory>();
            services.AddSingleton<IScanner, Scanner>();
            services.AddSingleton<IKnowledgeBaseProvider, KnowledgeBaseProvider>();
            services.AddSingleton<IScannerFactory, ScannerFactory>();
            services.AddSingleton<IScannerResultAnalyzer, ScannerResultAnalyzer>();
            services.AddSingleton<ISnapshotFactory>(x => new SnapshotFactory(x.GetService<IBrokerObjectFactory>()));
            
            return services;
        }

        /// <summary>
        /// Registers all the necessary components to use the low level HareDu Broker, Diagnostic, and Snapshotting APIs.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configurator">Configure Broker and Diagnostic APIs programmatically.</param>
        /// <returns></returns>
        public static IServiceCollection AddHareDu(this IServiceCollection services, Action<HareDuConfigurator> configurator)
        {
            HareDuConfig config = configurator.IsNull()
                ? ConfigCache.Default
                : new HareDuConfigProvider()
                    .Configure(configurator);

            services.AddSingleton(config);
            services.AddSingleton<IBrokerObjectFactory, BrokerObjectFactory>();
            services.AddSingleton<IScanner, Scanner>();
            services.AddSingleton<IKnowledgeBaseProvider, KnowledgeBaseProvider>();
            services.AddSingleton<IScannerFactory, ScannerFactory>();
            services.AddSingleton<IScannerResultAnalyzer, ScannerResultAnalyzer>();
            services.AddSingleton<ISnapshotFactory>(x => new SnapshotFactory(x.GetService<IBrokerObjectFactory>()));
            
            return services;
        }

        
        class HareDuConfigImpl :
            HareDuConfig
        {
            public BrokerConfig Broker { get; }
            public DiagnosticsConfig Diagnostics { get; }
        }
    }
}