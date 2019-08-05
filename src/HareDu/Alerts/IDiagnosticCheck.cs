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
namespace HareDu.Alerts
{
    using System;
    using Model;

    public interface IDiagnosticCheck<T>
    {
        string Identifier { get; }
        
        DiagnosticResult Execute(T snapshot);
    }

    public class ChannelThroughputThrottling :
        IDiagnosticCheck<ChannelSnapshot>
    {
        public string Identifier => "ChannelThroughputThrottle";

        public DiagnosticResult Execute(ChannelSnapshot snapshot)
        {
            if (snapshot.UnacknowledgedMessages > snapshot.PrefetchCount)
                return new DiagnosticResultImpl(snapshot.Name, DiagnosticStatus.Red,
                    "Unacknowledged messages on channel exceeds prefetch count causing the RabbitMQ broker to deliver any more messages to consumers.",
                    "Acknowledged messages must be greater than or equal to the result of subtracting the number of unacknowledged messages from the prefetch count plus 1.");

            return new DiagnosticResultImpl(snapshot.Name, DiagnosticStatus.Green,
                "Unacknowledged messages on channel is less than prefetch count.", null);
        }
    }

    class DiagnosticResultImpl :
        DiagnosticResult
    {
        public DiagnosticResultImpl(string identifier, DiagnosticStatus diagnosticStatus, string reason, string remediation)
        {
            Identifier = identifier;
            Status = diagnosticStatus;
            Reason = reason;
            Remediation = remediation;
            Timestamp = DateTimeOffset.Now;
        }

        public string Identifier { get; }
        public DiagnosticStatus Status { get; }
        public string Reason { get; }
        public string Remediation { get; }
        public DateTimeOffset Timestamp { get; }
    }

    public interface DiagnosticResult
    {
        string Identifier { get; }
        DiagnosticStatus Status { get; }
        string Reason { get; }
        string Remediation { get; }
        DateTimeOffset Timestamp { get; }
    }

    public enum DiagnosticStatus
    {
        Red,
        Green
    }

    public enum AlertLevel
    {
        Critical,
        High,
        Warning,
        Low,
        None
    }

    public enum AlertStatus
    {
        Red,
        Yellow,
        Green
    }
}