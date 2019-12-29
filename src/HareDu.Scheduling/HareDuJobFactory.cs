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
namespace HareDu.Scheduling
{
    using System;
    using System.Collections.Generic;
    using Quartz;
    using Quartz.Simpl;
    using Quartz.Spi;
    using Snapshotting;

    public class HareDuJobFactory<T> :
        SimpleJobFactory
        where T : HareDuSnapshot<Snapshot>
    {
        readonly ISnapshotFactory _factory;
        readonly ISnapshotWriter _writer;

        public HareDuJobFactory(ISnapshotFactory factory, ISnapshotWriter writer)
        {
            _factory = factory;
            _writer = writer;
        }

        public override IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            try
            {
                if (bundle.JobDetail.JobType == typeof(PersistSnapshotJob<T>))
                    return new PersistSnapshotJob<T>(_factory, _writer);

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
        readonly ISnapshotFactory _factory;
        readonly ISnapshotWriter _writer;

        public HareDuJobFactory(ISnapshotFactory factory, ISnapshotWriter writer)
        {
            _factory = factory;
            _writer = writer;
        }

        public override IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            try
            {
                if (bundle.JobDetail.JobType == typeof(PersistSnapshotJob<BrokerConnection>))
                    return new PersistSnapshotJob<BrokerConnection>(_factory, _writer);

                if (bundle.JobDetail.JobType == typeof(PersistSnapshotJob<BrokerQueues>))
                    return new PersistSnapshotJob<BrokerQueues>(_factory, _writer);

                if (bundle.JobDetail.JobType == typeof(PersistSnapshotJob<Cluster>))
                    return new PersistSnapshotJob<Cluster>(_factory, _writer);

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