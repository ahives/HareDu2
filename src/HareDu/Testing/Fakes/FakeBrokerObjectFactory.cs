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
namespace HareDu.Testing.Fakes
{
    using Core;

    public class FakeBrokerObjectFactory :
        IBrokerObjectFactory
    {
        public T Object<T>()
            where T : BrokerObject
        {
            if (typeof(T) == typeof(Cluster))
            {
                Cluster obj = new FakeClusterObject();

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

        public void CancelPendingRequest()
        {
            throw new System.NotImplementedException();
        }
    }
}