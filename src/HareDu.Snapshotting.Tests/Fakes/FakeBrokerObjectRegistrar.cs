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
namespace HareDu.Snapshotting.Tests.Fakes
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using HareDu.Registration;

    public class FakeBrokerObjectRegistrar :
        IBrokerObjectRegistrar
    {
        readonly IDictionary<string, object> _cache;
        
        public IDictionary<string, object> ObjectCache => _cache;
        
        public FakeBrokerObjectRegistrar()
        {
            _cache = new Dictionary<string, object>();
        }

        public void RegisterAll(HttpClient client)
        {
            _cache.Add(typeof(FakeNodeObject).FullName, new FakeNodeObject());
            _cache.Add(typeof(FakeSystemOverviewObject).FullName, new FakeSystemOverviewObject());
        }

        public void Register(Type type, HttpClient client)
        {
            throw new NotImplementedException();
        }

        public void Register<T>(HttpClient client)
        {
            throw new NotImplementedException();
        }

        public bool TryRegisterAll(HttpClient client) => throw new NotImplementedException();

        public bool TryRegister(Type type, HttpClient client) => throw new NotImplementedException();

        public bool TryRegister<T>(HttpClient client) => throw new NotImplementedException();
    }
}