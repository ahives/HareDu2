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
namespace HareDu.Core.Internal.Serialization
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public class ReadWritePropertyCache<T> :
        IReadWritePropertyCache<T>
    {
        readonly IDictionary<string, ReadWriteProperty<T>> _properties;

        public ReadWritePropertyCache()
        {
            _properties = CreatePropertyCache(false);
        }

        public ReadWritePropertyCache(bool includeNonPublic)
        {
            _properties = CreatePropertyCache(includeNonPublic);
        }

        public ReadWriteProperty<T> this[string name] => _properties[name];

        public IEnumerator<ReadWriteProperty<T>> GetEnumerator()
        {
            return _properties.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool TryGetValue(string key, out ReadWriteProperty<T> value)
        {
            try
            {
                return _properties.TryGetValue(key, out value);
            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException($"The read only property {key} was not found.");
            }
        }

        public bool TryGetProperty(string propertyName, out ReadWriteProperty<T> property)
        {
            return _properties.TryGetValue(propertyName, out property);
        }

        static IDictionary<string, ReadWriteProperty<T>> CreatePropertyCache(bool includeNonPublic)
        {
            return new Dictionary<string, ReadWriteProperty<T>>(typeof(T).GetAllProperties()
                .Where(x => x.CanRead && (includeNonPublic || x.CanWrite))
                .Where(x => x.SetMethod != null)
                .Select(x => new ReadWriteProperty<T>(x))
                .ToDictionary(x => x.Property.Name));
        }

        public void Set(Expression<Func<T, object>> propertyExpression, T instance, object value)
        {
            _properties[propertyExpression.GetMemberName()].Set(instance, value);
        }

        public object Get(Expression<Func<T, object>> propertyExpression, T instance)
        {
            return _properties[propertyExpression.GetMemberName()].Get(instance);
        }
    }
}