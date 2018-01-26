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
    using Extensions;

    public class FaultedResult<T> :
        Result<T>
    {
        public FaultedResult(IReadOnlyList<Error> errors)
        {
            Errors = errors;
        }

        public FaultedResult(DebugInfo debugInfo, IReadOnlyList<Error> errors)
        {
            DebugInfo = debugInfo;
            Errors = errors;
        }

        public T Data => throw new ArgumentNullException();
        public DateTimeOffset Timestamp { get; }
        public DebugInfo DebugInfo { get; }
        public IReadOnlyList<Error> Errors { get; }
        public bool HasResult => !Data.IsNull();
    }

    public class FaultedResult :
        Result
    {
        public FaultedResult(IReadOnlyList<Error> errors)
        {
            Errors = errors;
        }

        public FaultedResult(DebugInfo debugInfo, IReadOnlyList<Error> errors)
        {
            DebugInfo = debugInfo;
            Errors = errors;
        }

        public DateTimeOffset Timestamp { get; }
        public DebugInfo DebugInfo { get; }
        public IReadOnlyList<Error> Errors { get; }
    }
}