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
namespace HareDu.Core.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using Exceptions;

    public class ResourceFactory :
        IResourceFactory
    {
        HttpClient _client;
        IDictionary<string, object> _resourceCache;
        static IResourceFactory _instance = null;
        static readonly object _gate = new object();

//        public ResourceFactory(HttpClient client)
//        {
//            _client = client ?? throw new ArgumentNullException(nameof(client));
//            _resourceCache = new Dictionary<string, object>();
//
//            RegisterResources();
//        }

        public static IResourceFactory Instance
        {
            get
            {
                lock (_gate)
                {
                    return _instance ?? (_instance = new ResourceFactory());
                }
            }
        }

        public void Init(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _resourceCache = new Dictionary<string, object>();

            RegisterResources();
        }

        public TResource Resource<TResource>()
            where TResource : Resource
        {
            Type type = GetType()
                .Assembly
                .GetTypes()
                .FirstOrDefault(x => typeof(TResource).IsAssignableFrom(x) && !x.IsInterface);

            if (type == null)
                throw new HareDuResourceInitException($"Failed to find implementation class for interface {typeof(TResource)}");

            if (_resourceCache.ContainsKey(type.FullName))
                return (TResource)_resourceCache[type.FullName];
            
            var instance = (TResource)Activator.CreateInstance(type, _client);

            _resourceCache.Add(type.FullName, instance);
            
            return instance;
        }

        public void CancelPendingRequest()
        {
            _client.CancelPendingRequests();
        }

        void RegisterResources()
        {
            var resourceImplTypes = GetType()
                .Assembly
                .GetTypes()
                .Where(x => typeof(Resource).IsAssignableFrom(x) && !x.IsInterface);

            foreach (var type in resourceImplTypes)
            {
                var resource = Activator.CreateInstance(type, _client);
                
                _resourceCache.Add(type.FullName, resource);
            }
        }
    }
}