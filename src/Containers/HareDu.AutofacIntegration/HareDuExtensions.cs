namespace HareDu.AutofacIntegration
{
    using System;
    using Autofac;
    using Core;
    using Core.Configuration;
    using Core.Extensions;
    using Diagnostics;
    using Diagnostics.Formatting;
    using Diagnostics.KnowledgeBase;
    using Diagnostics.Persistence;
    using Microsoft.Extensions.Configuration;
    using Snapshotting;
    using Snapshotting.Persistence;

    public static class HareDuExtensions
    {
        /// <summary>
        /// Registers all the necessary components to use the low level HareDu Broker, Diagnostic, and Snapshotting APIs.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="settingsFile">The full path of where the configuration settings file is located.</param>
        /// <param name="configSection">The section found within the configuration file.</param>
        /// <returns></returns>
        public static ContainerBuilder AddHareDu(this ContainerBuilder builder, string settingsFile = "appsettings.json", string configSection = "HareDuConfig")
        {
            builder.Register(x =>
                {
                    HareDuConfig config = new HareDuConfigImpl();

                    IConfiguration configuration = new ConfigurationBuilder()
                        .AddJsonFile(settingsFile, false)
                        .Build();

                    configuration.Bind(configSection, config);

                    return config;
                })
                .SingleInstance();
            
            builder.RegisterType<BrokerObjectFactory>()
                .As<IBrokerObjectFactory>()
                .SingleInstance();
            
            builder.RegisterType<Scanner>()
                .As<IScanner>()
                .SingleInstance();
            
            builder.RegisterType<KnowledgeBaseProvider>()
                .As<IKnowledgeBaseProvider>()
                .SingleInstance();
            
            builder.RegisterType<ScannerFactory>()
                .As<IScannerFactory>()
                .SingleInstance();

            builder.RegisterType<ScannerResultAnalyzer>()
                .As<IScannerResultAnalyzer>()
                .SingleInstance();

            builder.RegisterType<SnapshotWriter>()
                .As<ISnapshotWriter>()
                .SingleInstance();

            builder.RegisterType<DiagnosticReportTextFormatter>()
                .As<IDiagnosticReportFormatter>()
                .SingleInstance();

            builder.RegisterType<DiagnosticWriter>()
                .As<IDiagnosticWriter>()
                .SingleInstance();

            builder.Register(x => new SnapshotFactory(x.Resolve<IBrokerObjectFactory>()))
                .As<ISnapshotFactory>()
                .SingleInstance();

            return builder;
        }

        public static ContainerBuilder AddHareDu(this ContainerBuilder builder, string settingsFile = "haredu.yaml")
        {
            builder.RegisterType<HareDuConfigProvider>()
                .As<IHareDuConfigProvider>()
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

            builder.Register(x =>
                {
                    var provider = x.Resolve<IFileConfigProvider>();

                    if (!provider.TryGet(settingsFile, out HareDuConfig config))
                        throw new HareDuConfigurationException($"Not able to get settings from {settingsFile}.");

                    var validator = x.Resolve<IConfigValidator>();

                    if (!validator.IsValid(config))
                        throw new HareDuConfigurationException($"Invalid settings in {settingsFile}.");

                    return config;
                })
                .As<HareDuConfig>()
                .SingleInstance();
            
            builder.RegisterType<BrokerObjectFactory>()
                .As<IBrokerObjectFactory>()
                .SingleInstance();
            
            builder.RegisterType<Scanner>()
                .As<IScanner>()
                .SingleInstance();
            
            builder.RegisterType<KnowledgeBaseProvider>()
                .As<IKnowledgeBaseProvider>()
                .SingleInstance();
            
            builder.RegisterType<ScannerFactory>()
                .As<IScannerFactory>()
                .SingleInstance();

            builder.RegisterType<ScannerResultAnalyzer>()
                .As<IScannerResultAnalyzer>()
                .SingleInstance();

            builder.RegisterType<SnapshotWriter>()
                .As<ISnapshotWriter>()
                .SingleInstance();

            builder.RegisterType<DiagnosticReportTextFormatter>()
                .As<IDiagnosticReportFormatter>()
                .SingleInstance();

            builder.RegisterType<DiagnosticWriter>()
                .As<IDiagnosticWriter>()
                .SingleInstance();

            builder.Register(x => new SnapshotFactory(x.Resolve<IBrokerObjectFactory>()))
                .As<ISnapshotFactory>()
                .SingleInstance();

            return builder;
        }

        /// <summary>
        /// Registers all the necessary components to use the low level HareDu Broker, Diagnostic, and Snapshotting APIs.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configurator">Configure Broker and Diagnostic APIs programmatically.</param>
        /// <returns></returns>
        public static ContainerBuilder AddHareDu(this ContainerBuilder builder, Action<HareDuConfigurator> configurator)
        {
            builder.Register(x =>
                {
                    HareDuConfig config = configurator.IsNull()
                        ? ConfigCache.Default
                        : new HareDuConfigProvider()
                            .Configure(configurator);

                    return config;
                })
                .SingleInstance();
            
            builder.RegisterType<BrokerObjectFactory>()
                .As<IBrokerObjectFactory>()
                .SingleInstance();
            
            builder.RegisterType<Scanner>()
                .As<IScanner>()
                .SingleInstance();
            
            builder.RegisterType<KnowledgeBaseProvider>()
                .As<IKnowledgeBaseProvider>()
                .SingleInstance();
            
            builder.RegisterType<ScannerFactory>()
                .As<IScannerFactory>()
                .SingleInstance();

            builder.RegisterType<ScannerResultAnalyzer>()
                .As<IScannerResultAnalyzer>()
                .SingleInstance();

            builder.RegisterType<SnapshotWriter>()
                .As<ISnapshotWriter>()
                .SingleInstance();

            builder.RegisterType<DiagnosticReportTextFormatter>()
                .As<IDiagnosticReportFormatter>()
                .SingleInstance();

            builder.RegisterType<DiagnosticWriter>()
                .As<IDiagnosticWriter>()
                .SingleInstance();

            builder.Register(x => new SnapshotFactory(x.Resolve<IBrokerObjectFactory>()))
                .As<ISnapshotFactory>()
                .SingleInstance();

            return builder;
        }

        
        class HareDuConfigImpl :
            HareDuConfig
        {
            public BrokerConfig Broker { get; }
            public DiagnosticsConfig Diagnostics { get; }
        }
    }
}