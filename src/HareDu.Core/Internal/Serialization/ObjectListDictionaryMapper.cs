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
    using System.Collections.Generic;

    public class ObjectListDictionaryMapper<T, TElement> :
        IDictionaryMapper<T>
    {
        readonly IDictionaryConverter _elementConverter;
        readonly ReadOnlyProperty<T> _property;

        public ObjectListDictionaryMapper(ReadOnlyProperty<T> property, IDictionaryConverter elementConverter)
        {
            _property = property;
            _elementConverter = elementConverter;
        }

        public void WritePropertyToDictionary(IDictionary<string, object> dictionary, T obj)
        {
            var value = _property.Get(obj);

            var values = value as IList<TElement>;
            if (values == null)
                return;

            var elements = new object[values.Count];
            for (var i = 0; i < values.Count; i++)
                elements[i] = _elementConverter.GetDictionary(values[i]);

            dictionary.Add(_property.Property.Name, elements);
        }
    }
}