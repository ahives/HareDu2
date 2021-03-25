namespace HareDu.CoreIntegration
{
    using Diagnostics;
    using Diagnostics.Formatting;
    using Diagnostics.KnowledgeBase;
    using Diagnostics.Persistence;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;

    public static class HareDuDiagnosticsExtensions
    {
        /// <summary>
        /// Registers all the necessary components to use the HareDu Diagnostics API.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddHareDuDiagnostics(this IServiceCollection services)
        {
            services.TryAddSingleton<IDiagnosticReportFormatter, DiagnosticReportTextFormatter>();

            services.TryAddSingleton<IScanner, Scanner>();

            services.TryAddSingleton<IKnowledgeBaseProvider, KnowledgeBaseProvider>();
            
            services.TryAddSingleton<IScannerFactory, ScannerFactory>();
            
            services.TryAddSingleton<IScannerResultAnalyzer, ScannerResultAnalyzer>();

            services.TryAddSingleton<IDiagnosticWriter, DiagnosticWriter>();

            return services;
        }
    }
}