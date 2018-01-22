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
namespace HareDu
{
    using System;
    using System.Collections.Generic;

    public interface Result<out TResult>
    {
        TResult Data { get; }
        DateTimeOffset Timestamp { get; }
        DebugInfo DebugInfo { get; }
        IEnumerable<Error> Errors { get; }
        bool HasResult { get; }
    }

    public static class Result
    {
        public static Result<T> None<T>(DebugInfo debugInfo = default, IEnumerable<Error> errors = default)
        {
            return new ResultImpl<T>(debugInfo, errors);
        }

            
        class ResultImpl<TResult> :
            Result<TResult>
        {
            public ResultImpl(DebugInfo debugInfo, IEnumerable<Error> errors)
            {
                DebugInfo = debugInfo;
                Errors = errors;
                Timestamp = DateTimeOffset.UtcNow;
            }

            public TResult Data => throw new ArgumentNullException();
            public DateTimeOffset Timestamp { get; }
            public DebugInfo DebugInfo { get; }
            public IEnumerable<Error> Errors { get; }
            public bool HasResult => false;
        }
    }
}