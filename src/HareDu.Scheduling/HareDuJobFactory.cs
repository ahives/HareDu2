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
namespace HareDu.Scheduling
{
    using System;
    using System.Collections.Generic;
    using Diagnostics;
    using Diagnostics.Persistence;
    using Quartz;
    using Quartz.Simpl;
    using Quartz.Spi;
    using Snapshotting;
    using Snapshotting.Persistence;
    using Snapshotting.Registration;

    public class HareDuJobFactory<T> :
        SimpleJobFactory
        where T : HareDuSnapshot<Snapshot>
    {
        readonly IDiagnosticScanner _scanner;
        readonly ISnapshotFactory _factory;
        readonly ISnapshotWriter _snapshotWriter;
        readonly IDiagnosticWriter _diagnosticWriter;

        public HareDuJobFactory(
            IDiagnosticScanner scanner,
            ISnapshotFactory factory,
            ISnapshotWriter snapshotWriter,
            IDiagnosticWriter diagnosticWriter)
        {
            _scanner = scanner;
            _factory = factory;
            _snapshotWriter = snapshotWriter;
            _diagnosticWriter = diagnosticWriter;
        }

        public override IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            try
            {
                if (bundle.JobDetail.JobType == typeof(PersistSnapshotJob<T>))
                    return new PersistSnapshotJob<T>(_factory, _snapshotWriter);

                if (bundle.JobDetail.JobType == typeof(PersistDiagnosticsJob<T>))
                    return new PersistDiagnosticsJob<T>(_factory, _scanner, _diagnosticWriter);

                return null;
            }
            catch (Exception e)
            {
                throw new SchedulerException(
                    $"Problem while instantiating job '{bundle.JobDetail.Key}' from HareDuJobFactory.");
            }
        }
    }

    public class HareDuJobFactory :
        SimpleJobFactory
    {
        readonly IDiagnosticScanner _scanner;
        readonly ISnapshotFactory _factory;
        readonly ISnapshotWriter _snapshotWriter;
        readonly IDiagnosticWriter _diagnosticWriter;

        public HareDuJobFactory(
            IDiagnosticScanner scanner,
            ISnapshotFactory factory,
            ISnapshotWriter snapshotWriter,
            IDiagnosticWriter diagnosticWriter)
        {
            _scanner = scanner;
            _factory = factory;
            _snapshotWriter = snapshotWriter;
            _diagnosticWriter = diagnosticWriter;
        }

        public override IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            try
            {
                if (bundle.JobDetail.JobType == typeof(PersistSnapshotJob<BrokerConnectivity>))
                    return new PersistSnapshotJob<BrokerConnectivity>(_factory, _snapshotWriter);

                if (bundle.JobDetail.JobType == typeof(PersistSnapshotJob<BrokerQueues>))
                    return new PersistSnapshotJob<BrokerQueues>(_factory, _snapshotWriter);

                if (bundle.JobDetail.JobType == typeof(PersistSnapshotJob<Cluster>))
                    return new PersistSnapshotJob<Cluster>(_factory, _snapshotWriter);

                if (bundle.JobDetail.JobType == typeof(PersistDiagnosticsJob<Cluster>))
                    return new PersistDiagnosticsJob<Cluster>(_factory, _scanner, _diagnosticWriter);

                return new DoNothingJob();
            }
            catch (Exception e)
            {
                throw new SchedulerException(
                    $"Problem while instantiating job '{bundle.JobDetail.Key}' from HareDuJobFactory.");
            }
        }
    }
}