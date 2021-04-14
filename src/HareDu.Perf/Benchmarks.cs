namespace HareDu.Perf
{
    using System.Threading.Tasks;
    using BenchmarkDotNet.Attributes;
    using Microsoft.Extensions.DependencyInjection;

    [Config(typeof(BenchmarkConfig))]
    public class Benchmarks :
        HareDuPerformanceTesting
    {
        readonly ServiceProvider _services;

        public Benchmarks()
        {
            _services = GetContainerBuilder().BuildServiceProvider();
        }

        [Benchmark]
        public async Task QueueCreateBenchmark()
        {
            var result = await _services.GetService<IBrokerObjectFactory>()
                .Object<Queue>()
                .Create("TestQueue31", "HareDu", "Node1", x =>
                {
                    x.IsDurable();
                    x.AutoDeleteWhenNotInUse();
                    x.HasArguments(arg =>
                    {
                        arg.SetQueueExpiration(1000);
                        arg.SetPerQueuedMessageExpiration(2000);
                    });
                });
        }
    }
}