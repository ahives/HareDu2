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
    using Diagnostics;
    using Diagnostics.Formatting;
    using Diagnostics.KnowledgeBase;
    using Diagnostics.Persistence;
    using Diagnostics.Registration;
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