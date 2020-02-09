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
namespace HareDu.Diagnostics.Registration
{
    using System;
    using System.Collections.Generic;
    using Probes;

    public interface IDiagnosticProbeRegistrar
    {
        IDictionary<string, IDiagnosticProbe> ObjectCache { get; }
        
        void RegisterAll();
        
        void Register(Type type);
        
        void Register<T>();

        bool TryRegister(Type type);

        bool TryRegister<T>();
    }
}