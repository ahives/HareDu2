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

    public class FakeOperatingSystemSnapshot1 :
        OperatingSystemSnapshot
    {
        public FakeOperatingSystemSnapshot1(long available, long used)
        {
            FileDescriptors = new FileDescriptorChurnMetricsImpl(available, used);
        }

        public string ProcessId { get; }
        public FileDescriptorChurnMetrics FileDescriptors { get; }
        public SocketChurnMetrics Sockets { get; }

        
        class FileDescriptorChurnMetricsImpl :
            FileDescriptorChurnMetrics
        {
            public FileDescriptorChurnMetricsImpl(long available, long used)
            {
                Available = available;
                Used = used;
            }

            public long Available { get; }
            public long Used { get; }
            public decimal UsageRate { get; }
            public long OpenAttempts { get; }
            public decimal OpenAttemptRate { get; }
            public decimal AvgTimePerOpenAttempt { get; }
            public decimal AvgTimeRatePerOpenAttempt { get; }
        }
    }
}