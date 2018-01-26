// Copyright 2013-2018 Albert L. Hives
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
        public static IReadOnlyList<T> Where<T>(this Result<IReadOnlyList<T>> source, Func<T, bool> predicate)
        {
            IReadOnlyList<T> list = source.Select(x => x.Data);

            return Filter(list, predicate);
        }

        public static IReadOnlyList<T> Where<T>(this Task<Result<IReadOnlyList<T>>> source, Func<T, bool> predicate)
        {
            Result<IReadOnlyList<T>> data = source?.Result;
            IReadOnlyList<T> list = data.Select(x => x.Data);

            return Filter(list, predicate);
        }

        static IReadOnlyList<T> Filter<T>(IReadOnlyList<T> list, Func<T, bool> predicate)
        {
            var internalList = new List<T>();

            for (int i = 0; i < list.Count; i++)
                if (predicate(list[i]))
                    internalList.Add(list[i]);

            return internalList;
        }
    }
}