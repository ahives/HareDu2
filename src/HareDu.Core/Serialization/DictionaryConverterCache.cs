﻿// Copyright 2012-2016 Chris Patterson
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace HareDu.Core.Serialization
{
    using System;
    using System.Collections.Concurrent;

    /// <summary>
    /// Caches the type converter instances
    /// </summary>
    public class DictionaryConverterCache :
        IDictionaryConverterCache
    {
        readonly ConcurrentDictionary<Type, IDictionaryConverter> _cache;

        public DictionaryConverterCache()
        {
            _cache = new ConcurrentDictionary<Type, IDictionaryConverter>();
        }

        public IDictionaryConverter GetConverter(Type type)
        {
            return _cache.GetOrAdd(type, CreateMissingConverter);
        }

        IDictionaryConverter CreateMissingConverter(Type key)
        {
            var type = typeof(ObjectDictionaryConverter<>).MakeGenericType(key);

            return (IDictionaryConverter)Activator.CreateInstance(type, this);
        }
    }
}