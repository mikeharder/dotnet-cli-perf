using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetCliPerf
{
    class Options
    {
        [Option('t', "types", HelpText = "Comma-separated list of types to benchmark.", Separator = ',')]
        public IEnumerable<string> Types { get; set; }

        [Option('m', "methods", HelpText = "Comma-separated list of methods to benchmark.", Separator = ',')]
        public IEnumerable<string> Methods { get; set; }

        [Option('p', "parameters", HelpText = "Comma-separated list of parameters to benchmark.", Separator = ',')]
        public IEnumerable<string> Parameters { get; set; }

        [Option('c', "targetCount", Default = 1)]
        public int TargetCount { get; set; }

        [Option('w', "warmupCount", Default = 0)]
        public int WarmupCount { get; set; }
    }

    class Program
    {
        static int Main(string[] args)
        {
            return Parser.Default.ParseArguments<Options>(args).MapResult(
                options => Run(options),
                _ => 1
            );
        }

        private static int Run(Options options)
        {
            var job = new Job();
            job.Run.RunStrategy = RunStrategy.Monitoring;
            job.Run.LaunchCount = 1;
            job.Run.WarmupCount = options.WarmupCount;
            job.Run.TargetCount = options.TargetCount;

            var config = ManualConfig.Create(DefaultConfig.Instance).With(job);

            var allBenchmarks = new List<Benchmark>();
            allBenchmarks.AddRange(BenchmarkConverter.TypeToBenchmarks(typeof(NetCoreCli), config));

            var selectedBenchmarks = allBenchmarks.
                Where(b => !options.Types.Any() ||
                           options.Types.Contains(b.Target.Type.Name, StringComparer.OrdinalIgnoreCase)).
                Where(b => !options.Methods.Any() ||
                           options.Methods.Contains(b.Target.Method.Name, StringComparer.OrdinalIgnoreCase)).
                Where(b => !options.Parameters.Any() ||
                           options.Parameters.Contains((string)b.Parameters[0].Value, StringComparer.OrdinalIgnoreCase));

            BenchmarkRunner.Run(selectedBenchmarks.ToArray(), config);

            return 0;
        }
    }
}
