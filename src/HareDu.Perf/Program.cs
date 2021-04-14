namespace HareDu.Perf
{
    using BenchmarkDotNet.Running;

    class Program
    {
        static void Main(string[] args)
        {
            var run = BenchmarkRunner.Run(typeof(Program).Assembly);
        }
    }
}