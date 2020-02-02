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
    using System.Net.Http;
    using Core;
    using Core.Extensions;

    public class BrokerObjectInstanceCreator :
        IBrokerObjectInstanceCreator
    {
        readonly HttpClient _client;

        public BrokerObjectInstanceCreator(HttpClient client)
        {
            _client = client;
        }

        public object CreateInstance(Type type)
        {
            var instance = type.IsDerivedFrom(typeof(BaseBrokerObject))
                ? Activator.CreateInstance(type, _client)
                : Activator.CreateInstance(type);

            return instance;
        }

        public object CreateInstance(Type type, HttpClient client)
        {
            var instance = Activator.CreateInstance(type, client);

            return instance;
        }
    }
}