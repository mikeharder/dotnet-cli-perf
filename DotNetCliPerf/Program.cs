using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.InProcess;
using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetCliPerf
{
    class Options
    {
        [Option('t', "types", HelpText = "Comma-separated list of types to benchmark. Default is all types.", Separator = ',')]
        public IEnumerable<string> Types { get; set; }

        [Option('m', "methods", HelpText = "Comma-separated list of methods to benchmark. Default is all methods.", Separator = ',')]
        public IEnumerable<string> Methods { get; set; }

        [Option('p', "parameters", HelpText = "Comma-separated list of parameters to benchmark. Default is all parameters.", Separator = ',')]
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
            job = job.With(InProcessToolchain.Instance);

            var config = ManualConfig.Create(DefaultConfig.Instance).With(job);

            var allBenchmarks = new List<Benchmark>();
            foreach (var type in typeof(Program).Assembly.GetTypes().Where(t => !t.IsAbstract).Where(t => t.IsPublic))
            {
                allBenchmarks.AddRange(BenchmarkConverter.TypeToBenchmarks(type, config).Benchmarks);
            }

            var selectedBenchmarks = (IEnumerable<Benchmark>)allBenchmarks;
            var parameters = ParametersToDictionary(options.Parameters);

            // If not specified, default "Restore" to "true" for Core and "false" for Framework,
            // to match typical customer usage.
            if (!parameters.ContainsKey("Restore"))
            {
                selectedBenchmarks = selectedBenchmarks.Where(b =>
                {
                    if (b.Target.Type.Name.IndexOf("Core", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        return (bool)b.Parameters["Restore"];
                    }
                    else if (b.Target.Type.Name.IndexOf("Framework", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        return !(bool)b.Parameters["Restore"];
                    }
                    else
                    {
                        return true;
                    }
                });
            }

            // If not specified, default "Parallel" to "true" for Core, to match typical customer usage.
            if (!parameters.ContainsKey("Parallel"))
            {
                selectedBenchmarks = selectedBenchmarks.Where(b =>
                {
                    if (b.Target.Type.Name.IndexOf("Core", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        return (bool)b.Parameters["Parallel"];
                    }
                    else
                    {
                        return true;
                    }
                });
            }

            // If not specified, default "MSBuildVersion" to "Core" for Core, to match typical customer usage.
            if (!parameters.ContainsKey("MSBuildVersion"))
            {
                selectedBenchmarks = selectedBenchmarks.Where(b =>
                {
                    if (b.Target.Type.Name.IndexOf("Core", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        return ((MSBuildVersion)b.Parameters["MSBuildVersion"]) == MSBuildVersion.Core;
                    }
                    else
                    {
                        return true;
                    }
                });
            }

            // Skip benchmarks with MSBuildVersion=Core and NodeReuse=True, since Core MSBuild currently does
            // not support NodeReuse.
            selectedBenchmarks = selectedBenchmarks.Where(b =>
                !(((MSBuildVersion?)b.Parameters["MSBuildVersion"]) == MSBuildVersion.Core &&
                  ((bool)b.Parameters["NodeReuse"])));
                
            // If not specified, default "NodeReuse" to  "true" for Framework, to match typical customer usage.
            if (!parameters.ContainsKey("NodeReuse"))
            {
                selectedBenchmarks = selectedBenchmarks.Where(b =>
                {
                    if (b.Target.Type.Name.IndexOf("Framework", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        return (bool)b.Parameters["NodeReuse"];
                    }
                    else
                    {
                        return true;
                    }
                });
            }

            selectedBenchmarks = selectedBenchmarks.
                Where(b => !options.Types.Any() ||
                                       b.Target.Type.Name.ContainsAny(options.Types, StringComparison.OrdinalIgnoreCase)).
                            Where(b => !options.Methods.Any() ||
                                       b.Target.Method.Name.ContainsAny(options.Methods, StringComparison.OrdinalIgnoreCase)).
                            Where(b => b.Parameters.Match(parameters));

            BenchmarkRunner.Run(selectedBenchmarks.ToArray(), config);

            return 0;
        }

        private static Dictionary<string, string> ParametersToDictionary(IEnumerable<string> parameters)
        {
            var dict = new Dictionary<string, string>(parameters.Count(), StringComparer.OrdinalIgnoreCase);

            foreach (var p in parameters)
            {
                var parts = p.Split('=');
                dict.Add(parts[0], parts[1]);
            }

            return dict;
        }
    }
}
