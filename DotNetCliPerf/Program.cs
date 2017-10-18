using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

namespace DotNetCliPerf
{
    class Program
    {
        static void Main(string[] args)
        {
            var warmupCount = args.Length >= 1 ? int.Parse(args[0]) : 1;
            var targetCount = args.Length >= 2 ? int.Parse(args[1]) : 1;

            var job = new Job();
            job.Run.RunStrategy = RunStrategy.Monitoring;
            job.Run.LaunchCount = 1;
            job.Run.WarmupCount = warmupCount;
            job.Run.TargetCount = targetCount;

            var config = ManualConfig.Create(DefaultConfig.Instance).With(job);

            BenchmarkRunner.Run<NetCoreCli>(config);
        }
    }
}
