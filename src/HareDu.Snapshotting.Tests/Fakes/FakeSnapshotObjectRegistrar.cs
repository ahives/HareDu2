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
    using Registration;

    public class FakeSnapshotObjectRegistrar :
        ISnapshotObjectRegistrar
    {
        readonly IDictionary<string, object> _cache;
        
        public IDictionary<string, object> ObjectCache => _cache;
        
        public FakeSnapshotObjectRegistrar()
        {
            _cache = new Dictionary<string, object>();
        }

        public void RegisterAll()
        {
            _cache.Add(typeof(FakeCluster).FullName, new FakeCluster());
        }

        public void Register(Type type)
        {
            throw new NotImplementedException();
        }

        public void Register<T>()
        {
            throw new NotImplementedException();
        }

        public bool TryRegisterAll() => throw new NotImplementedException();

        public bool TryRegister(Type type) => throw new NotImplementedException();

        public bool TryRegister<T>() => throw new NotImplementedException();
    }
}