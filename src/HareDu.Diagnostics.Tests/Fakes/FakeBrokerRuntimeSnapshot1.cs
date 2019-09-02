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
namespace HareDu.Diagnostics.Tests.Fakes
{
    using Snapshotting.Model;

    public class FakeBrokerRuntimeSnapshot1 :
        BrokerRuntimeSnapshot
    {
        public FakeBrokerRuntimeSnapshot1(long availableCores, long limit, long used, decimal usageRate)
        {
            AvailableCores = availableCores;
            Processes = new RuntimeProcessChurnMetricsImpl(limit, used, usageRate);
        }

        public string Version { get; }
        public long AvailableCores { get; }
        public RuntimeProcessChurnMetrics Processes { get; }

        
        class RuntimeProcessChurnMetricsImpl :
            RuntimeProcessChurnMetrics
        {
            public RuntimeProcessChurnMetricsImpl(long limit, long used, decimal usageRate)
            {
                Limit = limit;
                Used = used;
                UsageRate = usageRate;
            }

            public long Limit { get; }
            public long Used { get; }
            public decimal UsageRate { get; }
        }
    }
}