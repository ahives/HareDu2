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
namespace HareDu.Registration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using Core;

    public class BrokerObjectRegistration :
        IBrokerObjectRegistration
    {
        readonly IDictionary<string, object> _cache;

        public IDictionary<string, object> Cache => _cache;

        public BrokerObjectRegistration()
        {
            _cache = new Dictionary<string, object>();
        }

        public void RegisterAll(HttpClient client)
        {
            var types = GetType()
                .Assembly
                .GetTypes()
                .Where(x => typeof(BrokerObject).IsAssignableFrom(x) && !x.IsInterface);

            foreach (var type in types)
            {
                var resource = Activator.CreateInstance(type, client);
                
                _cache.Add(type.FullName, resource);
            }
        }
    }
}