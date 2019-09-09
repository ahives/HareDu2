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
namespace HareDu.Diagnostics.Configuration
{
    using System;
    using System.IO;
    using YamlDotNet.Serialization;
    using YamlDotNet.Serialization.NamingConventions;

    public class DiagnosticScannerConfigProvider :
        IDiagnosticScannerConfigProvider
    {
        public bool TryGet(out DiagnosticScannerConfig config)
        {
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(new HyphenatedNamingConvention())
                .WithTagMapping("sensor", typeof(DiagnosticSensorConfig))
                .Build();

            string path = "/Users/albert/Documents/Git/HareDu2/src/HareDu.Diagnostics.Tests/scanner.yaml";

            if (!File.Exists(path))
            {
                config = DiagnosticSensorConfigCache.Default;
                return true;
            }

            try
            {
                using (var stream = File.OpenRead(path))
                using (var reader = new StreamReader(stream))
                {
                    config = deserializer.Deserialize<DiagnosticScannerConfig>(reader);
                    return true;
                }
            }
            catch (Exception e)
            {
                config = DiagnosticSensorConfigCache.Default;
                return true;
            }
            // TODO: add code to pull from configuration file
        }
    }
}