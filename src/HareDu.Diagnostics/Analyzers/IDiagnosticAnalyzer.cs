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
namespace HareDu.Diagnostics.Analyzers
{
    using System;

    public interface IDiagnosticAnalyzer :
        IObservable<DiagnosticAnalyzerContext>
    {
        string Identifier { get; }
        
        string Name { get; }
        
        string Description { get; }
        
        ComponentType ComponentType { get; }
        
        DiagnosticAnalyzerCategory Category { get; }
        
        DiagnosticAnalyzerStatus Status { get; }
        
        DiagnosticAnalyzerResult Execute<T>(T snapshot);
    }
}