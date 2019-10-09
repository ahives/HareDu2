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
    using System.Threading.Tasks;
    using Autofac;
    using AutofacIntegration;
    using Quartz;
    using Quartz.Impl;
    using Quartz.Logging;
    using Snapshotting;

    class Program
    {
        static async Task Main(string[] args)
        {
            var container = GetContainer();
            
//            IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            IScheduler scheduler = container.Resolve<IScheduler>();
            
            IJobDetail job = JobBuilder.Create<CustomSnapshotJob>()
                .WithIdentity("myJob", "group1")
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("myTrigger", "group1")
                .WithSimpleSchedule(x => x
                    .WithRepeatCount(2)
                    .WithIntervalInSeconds(10))
                .StartNow()
                .Build();
	  
            await scheduler.ScheduleJob(job, trigger);
            
            Console.WriteLine("Starting");

            await scheduler.Start();

            await scheduler.Shutdown(true);
            
            Console.WriteLine("Stopped");
        }

        static IContainer GetContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<HareDuSnapshottingModule>();

            builder.Register(x =>
                {
                    var factory = new StdSchedulerFactory();
                    var scheduler = factory.GetScheduler().Result;

                    scheduler.JobFactory = new CustomJobFactory(x.Resolve<ISnapshotFactory>());

                    return scheduler;
                })
                .As<IScheduler>()
                .SingleInstance();

            var container = builder.Build();

            return container;
        }
        
        class ConsoleLogProvider : ILogProvider
        {
            public Logger GetLogger(string name)
            {
                return (level, func, exception, parameters) =>
                {
                    if (level >= LogLevel.Info && func != null)
                    {
                        Console.WriteLine("[" + DateTime.Now.ToLongTimeString() + "] [" + level + "] " + func(), parameters);
                    }
                    return true;
                };
            }

            public IDisposable OpenNestedContext(string message)
            {
                throw new NotImplementedException();
            }

            public IDisposable OpenMappedContext(string key, string value)
            {
                throw new NotImplementedException();
            }
        }
    }
}