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
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using Autofac;
    using AutofacIntegration;
    using CoreIntegration;
    using Diagnostics;
    using Diagnostics.Extensions;
    using Elasticsearch.Net;
    using Microsoft.Extensions.DependencyInjection;
    using Nest;
    using Prometheus;
    using PrometheusIntegration;
    using Quartz;
    using Quartz.Impl;
    using Quartz.Logging;
    using Scheduling;
    using Snapshotting;
    using Snapshotting.Model;
    using Snapshotting.Persistence;
    using Snapshotting.Registration;
    using ITrigger = Quartz.ITrigger;
    using LogLevel = Quartz.Logging.LogLevel;
    using Snapshot = Snapshotting.Snapshot;

    class Program
    {
        static async Task Main(string[] args)
        {
            var services = new ServiceCollection()
                .AddHareDuConfiguration($"{Directory.GetCurrentDirectory()}/haredu.yaml")
                .AddHareDu()
                .AddHareDuSnapshot()
                .AddHareDuDiagnostics()
                // .AddHareDuScheduling<BrokerConnectivity>();
                .AddHareDuScheduling<BrokerConnectivitySnapshot>();

            var provider = services.BuildServiceProvider();

            // IScheduler scheduler = provider.GetService<IScheduler>();
            // IHareDuScheduler hareDuScheduler = provider.GetService<IHareDuScheduler>();
            //
            // IDictionary<string, object> details = new Dictionary<string, object>();

            // details["path"] = $"{Directory.GetCurrentDirectory()}/snapshots";
            // details["path"] = $"{Directory.GetCurrentDirectory()}/diagnostics";

            // await hareDuScheduler.Schedule<PersistSnapshotJob<BrokerQueues>>("persist-snapshot", details);
            // await hareDuScheduler.Schedule<PersistDiagnosticsJob<BrokerConnectivity>>("persist-diagnostic", details);
            
            // await hareDuScheduler.Schedule<PersistDiagnosticsJob<BrokerConnectivitySnapshot>>("persist-diagnostic", details);
            //
            // Console.WriteLine("Starting");
            //
            // await scheduler.Start();
            //
            // Thread.Sleep(60000);
            //
            // await scheduler.Shutdown(true);
            //
            // Console.WriteLine("Stopped");
            
            var scanner = provider.GetService<IScanner>();
            
            var analyzer = provider.GetService<IScannerResultAnalyzer>()
                .RegisterObserver(new ScannerResultAnalyzerObserver());

            var result = provider.GetService<ISnapshotFactory>()
                .Lens<BrokerConnectivitySnapshot>()
                .TakeSnapshot();

            var metricServer = new KestrelMetricServer(port: 1234);
            metricServer.Start();
            
            PrometheusMetricsConfigurator.Configure();

            var report = scanner
                .Scan(result.Snapshot)
                .Analyze(analyzer, x => x.Id);
            
            // Thread.Sleep(TimeSpan.FromMinutes(1));
            
            metricServer.Stop();
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