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
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Core.Testing;
    using Model;

    public class FakeConnectionObject :
        Connection,
        HareDuTestingFake
    {
        public async Task<ResultList<ConnectionInfo>> GetAll(CancellationToken cancellationToken = default)
        {
            ConnectionInfo connection1 = new FakeConnectionInfo(1, 1);
            ConnectionInfo connection2 = new FakeConnectionInfo(2, 1);
            ConnectionInfo connection3 = new FakeConnectionInfo(3, 1);

            List<ConnectionInfo> data = new List<ConnectionInfo> {connection1, connection2, connection3};

            return new SuccessfulResultList<ConnectionInfo>(data, null);
        }
    }
}