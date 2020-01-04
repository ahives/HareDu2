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
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Quartz;

    public class HareDuScheduler :
        IHareDuScheduler
    {
        readonly IScheduler _scheduler;

        public HareDuScheduler(IScheduler scheduler)
        {
            _scheduler = scheduler;
        }

        public async Task Schedule<T>(string name, IDictionary<string, object> details)
            where T : IJob
        {
            IJobDetail jobDetails = JobBuilder.Create<T>()
                .WithIdentity($"{name}-job")
                .UsingJobData(new JobDataMap(details))
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity($"{name}-trigger")
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(5)
                    .RepeatForever()
                    .WithMisfireHandlingInstructionFireNow())
                .Build();
            
            await _scheduler.ScheduleJob(jobDetails, trigger);
        }
    }
}