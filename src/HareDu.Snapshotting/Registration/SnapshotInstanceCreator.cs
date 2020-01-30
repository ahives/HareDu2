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
namespace HareDu.Snapshotting.Registration
{
    using System;
    using Core.Extensions;
    using Internal;

    public class SnapshotInstanceCreator :
        ISnapshotInstanceCreator
    {
        readonly IBrokerObjectFactory _factory;

        public SnapshotInstanceCreator(IBrokerObjectFactory factory)
        {
            _factory = factory;
        }

        public object CreateInstance(Type type)
        {
            var instance = type.IsDerivedFrom(typeof(BaseSnapshot<>))
                ? Activator.CreateInstance(type, _factory)
                : Activator.CreateInstance(type);
            
            return instance;
        }
    }
}