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
namespace HareDu.Snapshotting.Snapshots
{
    using System;
    using System.Collections.Generic;
    using Core;

    class BaseSnapshot
    {
        protected readonly IList<IDisposable> _observers;
        protected readonly IResourceFactory _factory;

        protected BaseSnapshot(IResourceFactory factory)
        {
            _factory = factory;
            _observers = new List<IDisposable>();
        }

        protected void ConnectObservers(IList<IObserver<SnapshotContext>> observers/*, IList<IDiagnostic> diagnostic*/)
        {
            for (int i = 0; i < observers.Count; i++)
            {
                if (observers[i] != null)
                {
//                    for (int j = 0; j < diagnostic.Count; j++)
//                    {
//                        _observers.Add(diagnostic[j].Subscribe(observers[i]));
//                    }
                }
            }
        }
    }
}