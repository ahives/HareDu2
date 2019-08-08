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
namespace HareDu.Diagnostics.Checks
{
    using Internal.Diagnostics;
    using Model;

    class ChannelThroughputThrottling :
        BaseDiagnosticCheck,
        IDiagnosticCheck
    {
        public string Identifier => "ChannelThroughputThrottling";
        public SnapshotType SnapshotType => SnapshotType.Channel;
        public DiagnosticCheckCategory DiagnosticCheckCategory => DiagnosticCheckCategory.Throughput;

        public DiagnosticResult Execute<T>(T snapshot)
        {
            ChannelSnapshot temp = snapshot as ChannelSnapshot;
            DiagnosticResult result = temp.UnacknowledgedMessages > temp.PrefetchCount
                ? new DiagnosticResultImpl(temp.Name, DiagnosticStatus.Red,
                    "Unacknowledged messages on channel exceeds prefetch count causing the RabbitMQ broker to stop delivering messages to consumers.",
                    "Acknowledged messages must be greater than or equal to the result of subtracting the number of unacknowledged messages from the prefetch count plus 1. Temporarily increase the number of consumers or prefetch count.")
                : new DiagnosticResultImpl(temp.Name, DiagnosticStatus.Green,
                    "Unacknowledged messages on channel is less than prefetch count.", null);

            NotifyObservers(result);
                
            return result;
        }
    }
}