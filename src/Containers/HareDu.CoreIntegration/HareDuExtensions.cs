namespace HareDu.CoreIntegration
{
    using System;
    using Core;
    using Core.Configuration;
    using Core.Configuration.Internal;
    using Core.Extensions;
    using Diagnostics;
    using Diagnostics.Formatting;
    using Diagnostics.KnowledgeBase;
    using Diagnostics.Persistence;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Snapshotting;
    using Snapshotting.Persistence;

    public static class HareDuExtensions
    {
        /// <summary>
        /// Registers all the necessary components to use the low level HareDu Broker, Diagnostic, and Snapshotting APIs.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="settingsFile">The full path of where the configuration settings file is located.</param>
        /// <param name="configSection">The section found within the configuration file.</param>
        /// <returns></returns>
        public static IServiceCollection AddHareDu(this IServiceCollection services, string settingsFile, string configSection)
        {
            var internalConfig = new InternalHareDuConfig();
            
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile(settingsFile, false)
                .Build();

            configuration.Bind(configSection, internalConfig);

            HareDuConfig config = ConfigMapper.Map(internalConfig);

            services.AddSingleton(config);
            services.AddSingleton<IBrokerObjectFactory, BrokerObjectFactory>();
            services.AddSingleton<IScanner, Scanner>();
            services.AddSingleton<IKnowledgeBaseProvider, KnowledgeBaseProvider>();
            services.AddSingleton<IScannerFactory, ScannerFactory>();
            services.AddSingleton<IScannerResultAnalyzer, ScannerResultAnalyzer>();
            services.AddSingleton<ISnapshotFactory>(x => new SnapshotFactory(x.GetService<IBrokerObjectFactory>()));
            services.AddSingleton<ISnapshotWriter, SnapshotWriter>();
            services.AddSingleton<IDiagnosticReportFormatter, DiagnosticReportTextFormatter>();
            services.AddSingleton<IDiagnosticWriter, DiagnosticWriter>();
            
            return services;
        }

        /// <summary>
        /// Registers all the necessary components to use the low level HareDu Broker, Diagnostic, and Snapshotting APIs.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="settingsFile"></param>
        /// <returns></returns>
        /// <exception cref="HareDuConfigurationException"></exception>
        [Obsolete("This method is deprecated, please use one of the overloaded method signatures instead.")]
        public static IServiceCollection AddHareDu(this IServiceCollection services, string settingsFile)
        {
            services.AddSingleton<IHareDuConfigProvider, HareDuConfigProvider>();
            services.AddSingleton<IFileConfigProvider, YamlFileConfigProvider>();
            services.AddSingleton<IConfigProvider, YamlConfigProvider>();
            services.AddSingleton<IConfigValidator, HareDuConfigValidator>();
            services.AddSingleton(x =>
            {
                var provider = x.GetService<IFileConfigProvider>();
            
                if (!provider.TryGet(settingsFile, out HareDuConfig config))
                    throw new HareDuConfigurationException($"Not able to get settings from {settingsFile}.");

                var validator = x.GetService<IConfigValidator>();

                if (!validator.IsValid(config))
                    throw new HareDuConfigurationException($"Invalid settings in {settingsFile}.");

                return config;
            });

            services.AddSingleton<IBrokerObjectFactory, BrokerObjectFactory>();
            services.AddSingleton<IScanner, Scanner>();
            services.AddSingleton<IKnowledgeBaseProvider, KnowledgeBaseProvider>();
            services.AddSingleton<IScannerFactory, ScannerFactory>();
            services.AddSingleton<IScannerResultAnalyzer, ScannerResultAnalyzer>();
            services.AddSingleton<ISnapshotFactory>(x => new SnapshotFactory(x.GetService<IBrokerObjectFactory>()));
            services.AddSingleton<ISnapshotWriter, SnapshotWriter>();
            services.AddSingleton<IDiagnosticReportFormatter, DiagnosticReportTextFormatter>();
            services.AddSingleton<IDiagnosticWriter, DiagnosticWriter>();
            
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
            services.AddSingleton<ISnapshotWriter, SnapshotWriter>();
            services.AddSingleton<IDiagnosticReportFormatter, DiagnosticReportTextFormatter>();
            services.AddSingleton<IDiagnosticWriter, DiagnosticWriter>();
            
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