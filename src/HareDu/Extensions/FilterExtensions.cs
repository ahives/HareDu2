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
namespace HareDu.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public static class FilterExtensions
    {
        /// <summary>
        /// Returns a filtered list of results meeting the specified predicate.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IReadOnlyList<T> Where<T>(this Result<IReadOnlyList<T>> source, Func<T, bool> predicate)
        {
            return !source.HasResult ? default : Filter(source.Data, predicate);
        }

        /// <summary>
        /// Returns a filtered list of results meeting the specified predicate.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IReadOnlyList<T> Where<T>(this Task<Result<IReadOnlyList<T>>> source, Func<T, bool> predicate)
        {
            if (!source.IsNull())
                return default;
            
            Result<IReadOnlyList<T>> result = source?.Result;

            return !result.HasResult ? default : Filter(result.Data, predicate);
        }

        static IReadOnlyList<T> Filter<T>(IReadOnlyList<T> list, Func<T, bool> predicate)
        {
            var internalList = new List<T>();

            for (int i = 0; i < list.Count; i++)
            {
                if (predicate(list[i]))
                    internalList.Add(list[i]);
            }
            
            return internalList;
        }
    }
}