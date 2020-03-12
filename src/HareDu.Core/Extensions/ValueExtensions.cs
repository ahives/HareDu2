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
    using System.Threading.Tasks;

    public static class ValueExtensions
    {
        /// <summary>
        /// Returns true if the value is null, otherwise, returns true.
        /// </summary>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool IsNull<T>(this T value) => value == null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool TryGetValue<T>(this ResultList<T> source, int index, out T value)
        {
            if (source.IsNull() || !source.HasData || index < 0 || index >= source.Data.Count)
            {
                value = default;
                return false;
            }

            value = source.Data[index];
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool TryGetValue<T>(this Task<ResultList<T>> source, int index, out T value)
        {
            if (source.IsNull() || index < 0)
            {
                value = default;
                return false;
            }
            
            ResultList<T> result = source.GetResult();

            if (result.IsNull() || result.Data.IsNull() || !result.HasData || result.HasFaulted || index >= result.Data.Count)
            {
                value = default;
                return false;
            }

            value = result.Data[index];
            return true;
        }
    }
}