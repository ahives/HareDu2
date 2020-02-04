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
namespace HareDu.Core.Extensions
{
    using System;

    public static class TypeExtensions
    {
        public static bool IsDerivedFrom(this Type type, Type fromType)
        {
            while (!type.IsNull() && type != typeof(object))
            {
                Type currentType = type.IsGenericType ? type.GetGenericTypeDefinition() : type;

                if (fromType == currentType)
                    return true;

                type = type.BaseType;
            }

            return false;
        }

        public static Type Find(this Type[] types, Predicate<Type> predicate) => Array.Find(types, predicate);
    }
}