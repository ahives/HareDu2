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
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading.Tasks;

    public static class LinqExtensions
    {
        public static T Single<T>(this Task<Result<IReadOnlyList<T>>> source)
        {
            return source.Select(x => x.Data).Single();
        }

        public static T SingleOrDefault<T>(this Task<Result<IReadOnlyList<T>>> source)
        {
            return source.Select(x => x.Data).SingleOrDefault();
        }

        public static T SingleOrDefault<T>(this Task<Result<IReadOnlyList<T>>> source, Func<T, bool> predicate)
        {
            return source.Select(x => x.Data).SingleOrDefault(predicate);
        }

        public static T FirstOrDefault<T>(this Task<Result<IReadOnlyList<T>>> source)
        {
            return source.Select(x => x.Data).FirstOrDefault();
        }

        public static T FirstOrDefault<T>(this Task<Result<IEnumerable<T>>> source, Func<T, bool> predicate)
        {
            return source.Select(x => x.Data).FirstOrDefault(predicate);
        }

        public static bool Any<T>(this Task<Result<IReadOnlyList<T>>> source)
        {
            return source.Select(x => x.Data).Any();
        }

        public static bool Any<T>(this Task<Result<IReadOnlyList<T>>> source, Func<T, bool> predicate)
        {
            return source.Select(x => x.Data).Any(predicate);
        }
    }
}