namespace HareDu.Scheduling
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Quartz;

    public interface IHareDuScheduler
    {
        Task Schedule<T>(string name, IDictionary<string, object> details)
            where T : IJob;
    }

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