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
namespace HareDu.Diagnostics.Formatting
{
    using System.Text;
    using Core.Extensions;

    public class DiagnosticReportTextFormatter :
        IDiagnosticReportFormatter
    {
        public string Format(ScannerResult report)
        {
            var builder = new StringBuilder();

            builder.AppendLine($"Report Identifier: {report.Identifier.ToString()}");
            builder.AppendLine($"Timestamp: {report.Timestamp.ToString()}");
            builder.AppendLine();

            builder.AppendLine("Results");
            
            foreach (var result in report.Results)
            {
                Format(result, ref builder);
            }

            return builder.ToString();
        }
        
        void Format(DiagnosticAnalyzerData data, ref StringBuilder builder) =>
            builder.AppendLine($"\t\t{data.PropertyName} => {data.PropertyValue}");

        void Format(DiagnosticAnalyzerResult result, ref StringBuilder builder)
        {
            builder.AppendLine($"\tTimestamp: {result.Timestamp.ToString()}");
            builder.AppendLine($"\tComponent Identifier: {result.ComponentIdentifier}");
            builder.AppendLine($"\tComponent Type: {result.ComponentType.ToString()}");
            builder.AppendLine($"\tAnalyzer: {result.AnalyzerIdentifier}");
            builder.AppendLine($"\tStatus: {result.Status.ToString()}");
            builder.AppendLine("\tAnalyzer Data");
            
            foreach (var data in result.AnalyzerData)
            {
                if (data.IsNull())
                    continue;
                
                Format(data, ref builder);
            }

            builder.AppendLine($"\tReason: {result.KnowledgeBaseArticle?.Reason}");
            builder.AppendLine($"\tRemediation: {result.KnowledgeBaseArticle?.Remediation}");
            builder.AppendLine();
        }
    }
}