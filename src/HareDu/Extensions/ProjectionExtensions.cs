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
    using System.Threading.Tasks;

    public static class ProjectionExtensions
    {
        public static TResult Select<T, TResult>(this Task<Result<T>> source, Func<Result<T>, TResult> projector)
        {
            if (source.IsNull() || projector.IsNull())
                return default;
                
            Result<T> result = source.Unwrap();

            return result.HasResult ? projector(result) : default;
        }

        public static TResult Select<T, TResult>(this Result<T> source, Func<Result<T>, TResult> projector)
        {
            if (source.IsNull() || projector.IsNull())
                return default;

            return source.HasResult ? projector(source) : default;
        }

        public static IReadOnlyList<T> Select<T>(this Result<IReadOnlyList<T>> source,
            Func<Result<IReadOnlyList<T>>, IReadOnlyList<T>> projection)
        {
            if (source.IsNull() || projection.IsNull())
                return new List<T>();

            return source.HasResult ? projection(source) : new List<T>();
        }
    }
}