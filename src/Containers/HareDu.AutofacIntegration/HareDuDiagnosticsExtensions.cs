namespace HareDu.AutofacIntegration
{
    using Autofac;
    using Diagnostics;
    using Diagnostics.Formatting;
    using Diagnostics.KnowledgeBase;

    /// <summary>
    /// Registers all the necessary components to use the HareDu Diagnostics API.
    /// </summary>
    public static class HareDuDiagnosticsExtensions
    {
        public static ContainerBuilder AddHareDuDiagnostics(this ContainerBuilder builder)
        {
            builder.RegisterType<ScannerFactory>()
                .As<IScannerFactory>()
                .SingleInstance();

            builder.RegisterType<ScannerResultAnalyzer>()
                .As<IScannerResultAnalyzer>()
                .SingleInstance();

            builder.RegisterType<Scanner>()
                .As<IScanner>()
                .SingleInstance();

            builder.RegisterType<DiagnosticReportTextFormatter>()
                .As<IDiagnosticReportFormatter>()
                .SingleInstance();

            builder.RegisterType<KnowledgeBaseProvider>()
                .As<IKnowledgeBaseProvider>()
                .SingleInstance();

            return builder;
        }
    }
}