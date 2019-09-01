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

    public class FakeDiskSnapshot1 :
        DiskSnapshot
    {
        public FakeDiskSnapshot1(long available, bool alarmInEffect, decimal rate)
        {
            AlarmInEffect = alarmInEffect;
            Capacity = new DiskCapacityDetailsImpl(available, rate);
        }

        public DiskCapacityDetails Capacity { get; }
        public string FreeLimit { get; }
        public bool AlarmInEffect { get; }

        
        class DiskCapacityDetailsImpl :
            DiskCapacityDetails
        {
            public DiskCapacityDetailsImpl(long available, decimal rate)
            {
                Available = available;
                Rate = rate;
            }

            public long Available { get; }
            public decimal Rate { get; }
        }
    }
}