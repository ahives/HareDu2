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
namespace HareDu.Registration
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;

    public interface IBrokerObjectRegistrar
    {
        IDictionary<string, object> ObjectCache { get; }

        void RegisterAll(HttpClient client);

        void Register(Type type, HttpClient client);

        void Register<T>(HttpClient client);

        bool TryRegisterAll(HttpClient client);
        
        bool TryRegister(Type type, HttpClient client);

        bool TryRegister<T>(HttpClient client);
    }
}