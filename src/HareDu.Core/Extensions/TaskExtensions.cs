﻿// Copyright 2013-2020 Albert L. Hives
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
    using System.Threading;
    using System.Threading.Tasks;

    public static class TaskExtensions
    {
        /// <summary>
        /// Unwrap <see cref="Task{T}"/> and return T.
        /// </summary>
        /// <param name="result"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetResult<T>(this Task<T> result)
            => !result.IsNull() && !result.IsCanceled && !result.IsFaulted
                ? result.GetAwaiter().GetResult()
                : default;
        
        public static void RequestCanceled(this CancellationToken cancellationToken)
        {
            if (!cancellationToken.IsCancellationRequested)
                return;

            cancellationToken.ThrowIfCancellationRequested();
        }
    }
}