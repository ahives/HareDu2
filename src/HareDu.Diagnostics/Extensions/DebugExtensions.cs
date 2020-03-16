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
namespace HareDu.Diagnostics.Extensions
{
    using System;
    using System.Collections.Generic;

    public static class DebugExtensions
    {
        public static ScannerResult ScreenDump(this ScannerResult result)
        {
            Console.WriteLine($"Id: {result.Id}");
            Console.WriteLine($"Scanner Id: {result.ScannerId}");
            // Console.WriteLine($"{result}");
            
            for (int i = 0; i < result.Results.Count; i++)
            {
                Console.WriteLine($"Id: {result.Results[i].Id}");
                Console.WriteLine($"Name: {result.Results[i].Name}");
                Console.WriteLine($"Status: {result.Results[i].Status}");
                Console.WriteLine($"Component Id: {result.Results[i].ComponentId}");
                Console.WriteLine($"Component Type: {result.Results[i].ComponentType}");
                Console.WriteLine($"Parent Component Id: {result.Results[i].ParentComponentId}");
                Console.WriteLine();
                // Console.WriteLine($"{result.Results[i]}");
            }

            return result;
        }

        public static IReadOnlyList<AnalyzerSummary> ScreenDump(this IReadOnlyList<AnalyzerSummary> result)
        {
            for (int i = 0; i < result.Count; i++)
            {
                Console.WriteLine(result[i].Id);
                Console.WriteLine($"\t{result[i].Healthy.Percentage}% healthy ({result[i].Healthy.Total})");
                Console.WriteLine($"\t{result[i].Unhealthy.Percentage}% unhealthy ({result[i].Unhealthy.Total})");
                Console.WriteLine($"\t{result[i].Warning.Percentage}% warning ({result[i].Warning.Total})");
                Console.WriteLine($"\t{result[i].Inconclusive.Percentage}% inconclusive ({result[i].Inconclusive.Total})");
            }

            return result;
        }
    }
}