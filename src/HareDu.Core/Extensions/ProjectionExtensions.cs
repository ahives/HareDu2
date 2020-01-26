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
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public static class ProjectionExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="projector"></param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public static TResult Select<T, TResult>(this Task<Result<T>> source, Func<Result<T>, TResult> projector)
        {
            if (source.IsNull() || projector.IsNull())
                return default;
                
            Result<T> result = source.Unfold();

            return !result.IsNull() && result.HasData ? projector(result) : default;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="projector"></param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public static TResult Select<T, TResult>(this Result<T> source, Func<Result<T>, TResult> projector)
        {
            if (source.IsNull() || !source.HasData || projector.IsNull())
                return default;
            
            return !source.IsNull() && source.HasData ? projector(source) : default;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="projector"></param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public static TResult Select<T, TResult>(this Task<ResultList<T>> source, Func<ResultList<T>, TResult> projector)
        {
            if (source.IsNull() || projector.IsNull())
                return default;
                
            ResultList<T> result = source.Unfold();

            return !result.IsNull() && result.HasData ? projector(result) : default;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="projector"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IReadOnlyList<T> Select<T>(this ResultList<T> source, Func<ResultList<T>, IReadOnlyList<T>> projector)
        {
            if (source.IsNull() || !source.HasData || projector.IsNull())
                return Array.Empty<T>();

            return !source.IsNull() && source.HasData ? projector(source) : Array.Empty<T>();
        }
    }
}