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
    using System.Collections.Generic;
    using System.Linq;

    public static class ValueExtensions
    {
        public static bool IsNull<T>(this T value)
        {
            return value == null;
        }

        public static bool TryGetValue<T>(this Result<IEnumerable<T>> source, int index, out T value)
        {
            if (source.IsNull() || source.Data.IsNull() || !source.Data.Any() || index < 0 || index >= source.Data.Count())
            {
                value = default;
                return false;
            }

            value = source.Data.ElementAt(index);
            return true;
        }
    }
}