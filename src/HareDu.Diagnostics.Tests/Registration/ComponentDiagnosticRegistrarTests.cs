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
namespace HareDu.Diagnostics.Tests.Registration
{
    using System.Reflection;
    using Diagnostics.Registration;
    using NUnit.Framework;
    using Shouldly;

    [TestFixture]
    public class ComponentDiagnosticRegistrarTests
    {
        [Test, Explicit]
        public void Test1()
        {
            // string path = $"{TestContext.CurrentContext.TestDirectory}/config.yaml";
            //
            // var configProvider = new ConfigurationProvider();
            // configProvider.TryGet(path, out HareDuConfig config);
            
            // var knowledgeBaseProvider = new DefaultKnowledgeBaseProvider();
            // var diagnosticAnalyzerRegistry = new DiagnosticAnalyzerRegistry(config.Analyzer, knowledgeBaseProvider);
            // diagnosticAnalyzerRegistry.RegisterAll();
            IDiagnosticProbeRegistrar registrar = null;
            var registry = new ComponentDiagnosticRegistrar(registrar);

            Should.Throw<TargetInvocationException>(() => registry.RegisterAll());
        }

        [Test, Explicit]
        public void Test2()
        {
        }
    }
}