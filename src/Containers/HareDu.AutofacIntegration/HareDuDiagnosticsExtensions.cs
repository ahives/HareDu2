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
namespace HareDu.AutofacIntegration
{
    using System.IO;
    using Autofac;
    using Core;
    using Core.Configuration;
    using Diagnostics;
    using Diagnostics.Formatting;
    using Diagnostics.KnowledgeBase;
    using Diagnostics.Registration;

    public static class HareDuDiagnosticsExtensions
    {
        public static ContainerBuilder AddHareDuDiagnostics(this ContainerBuilder builder, string file)
        {
            HareDuConfig config = GetConfig(file);
            
            builder.Register(config);
            
            return builder;
        }

        public static ContainerBuilder AddHareDuDiagnostics(this ContainerBuilder builder)
        {
            HareDuConfig config = GetConfig($"{Directory.GetCurrentDirectory()}/haredu.yaml");
            
            builder.Register(config);

            return builder;
        }

        static void Register(this ContainerBuilder builder, HareDuConfig config)
        {
            builder.Register(x =>
                    new ScannerFactory(config.Diagnostics, x.Resolve<IKnowledgeBaseProvider>()))
                .As<IScannerFactory>()
                .SingleInstance();

            builder.RegisterType<ScannerResultAnalyzer>()
                .As<IScannerResultAnalyzer>()
                .SingleInstance();

            builder.RegisterType<DiagnosticsConfigProvider>()
                .As<IDiagnosticsConfigProvider>()
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