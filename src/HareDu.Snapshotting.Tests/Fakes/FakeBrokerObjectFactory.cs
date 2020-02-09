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
namespace HareDu.Snapshotting.Tests.Fakes
{
    using Core;
    using Core.Testing;
    using HareDu.Registration;

    public class FakeBrokerObjectFactory :
        IBrokerObjectFactory,
        HareDuTestingFake
    {
        public T Object<T>()
            where T : BrokerObject
        {
            if (typeof(T) == typeof(SystemOverview))
            {
                SystemOverview obj = new FakeSystemOverviewObject();

                return (T) obj;
            }

            if (typeof(T) == typeof(Node))
            {
                Node obj = new FakeNodeObject();

                return (T) obj;
            }

            if (typeof(T) == typeof(Connection))
            {
                Connection obj = new FakeConnectionObject();

                return (T) obj;
            }

            if (typeof(T) == typeof(Channel))
            {
                Channel obj = new FakeChannelObject();

                return (T) obj;
            }

            if (typeof(T) == typeof(Queue))
            {
                Queue obj = new FakeQueueObject();

                return (T) obj;
            }

            return default;
        }

        public bool IsRegistered(string key) => throw new System.NotImplementedException();

        public void CancelPendingRequest()
        {
            throw new System.NotImplementedException();
        }
    }
}