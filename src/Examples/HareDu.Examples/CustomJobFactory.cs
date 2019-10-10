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
namespace HareDu.Examples
{
    using System;
    using Quartz;
    using Quartz.Simpl;
    using Quartz.Spi;
    using Snapshotting;
    using Snapshotting.Model;

    public class CustomJobFactory<T> :
        SimpleJobFactory
        where T : ResourceSnapshot<Snapshot>
    {
        readonly T _resource;

        public CustomJobFactory(T resource)
        {
            _resource = resource;
        }

        public override IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            try
            {
                return new CustomSnapshotJob<T>(_resource);
            }
            catch (Exception e)
            {
                throw new SchedulerException($"Problem while instantiating job '{bundle.JobDetail.Key}' from MyJobFactory.");
            }
        }
    }
}