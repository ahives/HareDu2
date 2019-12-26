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
namespace HareDu.Snapshotting.Extensions
{
    using System;
    using Core.Extensions;

    public static class ProjectionExtensions
    {
        /// <summary>
        /// Safely attempts to unwrap the specified object and returns the resultant value. If the object is NULL, then the default object value will be returned.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="projection"></param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <returns></returns>
        public static T Select<T, U>(this U obj, Func<U, T> projection)
            => obj.IsNull()
                ? default
                : projection(obj);
    }
}