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
namespace HareDu.Examples
{
    using Diagnostics;
    using Diagnostics.Persistence;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Quartz.Impl;
    using Scheduling;
    using Snapshotting;
    using Snapshotting.Persistence;
    using Snapshotting.Registration;

    public static class Extensions
    {
        public static IServiceCollection AddHareDuScheduling<T>(this IServiceCollection services)
            // where T : SnapshotLens<Snapshot>
            where T : Snapshot
        {
            services.TryAddSingleton(x =>
            {
                var factory = new StdSchedulerFactory();
                var scheduler = factory
                    .GetScheduler()
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();
        
                scheduler.JobFactory =
                    new HareDuJobFactory<T>(
                        x.GetService<IScanner>(),
                        x.GetService<ISnapshotFactory>(),
                        x.GetService<ISnapshotWriter>(),
                        x.GetService<IDiagnosticWriter>());
        
                return scheduler;
            });
        
            services.TryAddSingleton<IHareDuScheduler, HareDuScheduler>();
        
            return services;
        }
    }
}