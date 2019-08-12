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
namespace HareDu.Core
{
    using System;
    using System.Collections.Generic;

    public class FaultedResultList<T> :
        ResultList<T>
    {
        public FaultedResultList(List<Error> errors)
        {
            Errors = errors;
            Timestamp = DateTimeOffset.UtcNow;
        }

        public FaultedResultList(List<Error> errors, DebugInfo debugInfo)
        {
            DebugInfo = debugInfo;
            Errors = errors;
            Timestamp = DateTimeOffset.UtcNow;
        }

        public DateTimeOffset Timestamp { get; }
        public DebugInfo DebugInfo { get; }
        public IReadOnlyList<Error> Errors { get; }
        public bool HasFaulted => true;
        public IReadOnlyList<T> Data { get; }
        public bool HasData => false;
    }
}