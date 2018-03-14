using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.InProcess;
using BenchmarkDotNet.Validators;
using CommandLine;
using Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DotNetCliPerf
{
    class Options
    {
        [Option('t', "types", HelpText = "Comma-separated list of types to benchmark. Default is all types.", Separator = ',')]
        public IEnumerable<string> Types { get; set; }

        [Option('m', "methods", HelpText = "Comma-separated list of methods to benchmark. Default is all methods.", Separator = ',')]
        public IEnumerable<string> Methods { get; set; }

        [Option('p', "parameters", HelpText = "Comma-separated list of parameters to benchmark. Default is all parameters. Example: 'SdkVersion=1234|5678,SourceChanged=Leaf'",
            Separator = ',')]
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

            // Increase timeout from default 5 minutes to 30 minutes.  Required for OrchardCore, especially when using multiple iterations
            job = job.With(new InProcessToolchain(timeout: TimeSpan.FromMinutes(30), codegenMode: BenchmarkActionCodegen.ReflectionEmit, logOutput: true));

            var config = (IConfig)ManualConfig.Create(DefaultConfig.Instance);

            // Replace DefaultOrderProvider with NoOpOrderProvider to improve startup time (we don't care about order benchmarks are run)
            ((ManualConfig)config).Set(new NoOpOrderProvider());

            config = config.With(job);

            // Allow running debug build when debugger is attached
            if (Debugger.IsAttached)
            {
                ((List<IValidator>)config.GetValidators()).Remove(JitOptimizationsValidator.FailOnError);
                config = config.With(JitOptimizationsValidator.DontFailOnError);
            }

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
                    if (typeof(CoreApp).IsAssignableFrom(b.Target.Type))
                    {
                        return (bool)b.Parameters["Restore"];
                    }
                    else if (typeof(FrameworkApp).IsAssignableFrom(b.Target.Type))
                    {
                        return !(bool)b.Parameters["Restore"];
                    }
                    else
                    {
                        return true;
                    }
                });
            }

            // If not specified, default "PackageManagementFormat" to "PackagesConfig" to match typical ASP.NET customer usage.
            if (!parameters.ContainsKey("PackageManagementFormat"))
            {
                selectedBenchmarks = selectedBenchmarks.Where(b =>
                    (PackageManagementFormat?)b.Parameters["PackageManagementFormat"] == null ||
                    (PackageManagementFormat?)b.Parameters["PackageManagementFormat"] == PackageManagementFormat.PackagesConfig);
            }

            // If not specified, default "Parallel" to "true" to match typical customer usage.
            if (!parameters.ContainsKey("Parallel"))
            {
                selectedBenchmarks = selectedBenchmarks.Where(b => (bool?)b.Parameters["Parallel"] ?? true);
            }

            // If not specified, default "MSBuildFlavor" to "Core" for Core, to match typical customer usage.
            if (!parameters.ContainsKey("MSBuildFlavor"))
            {
                selectedBenchmarks = selectedBenchmarks.Where(b =>
                {
                    if (typeof(CoreApp).IsAssignableFrom(b.Target.Type))
                    {
                        return ((MSBuildFlavor)b.Parameters["MSBuildFlavor"]) == MSBuildFlavor.Core;
                    }
                    else
                    {
                        return true;
                    }
                });
            }

            // If MSBuildFlavor=Core, MSBuildVersion is irrelevant, so limit it to "NotApplicable"
            selectedBenchmarks = selectedBenchmarks.Where(b =>
                !(((MSBuildFlavor?)b.Parameters["MSBuildFlavor"]) == MSBuildFlavor.Core &&
                  !b.Parameters["MSBuildVersion"].ToString().Equals("NotApplicable", StringComparison.OrdinalIgnoreCase)));

            // If MSBuildFlavor=Framework or type is Framework, MSBuildVersion is required, so skip "NotApplicable"
            selectedBenchmarks = selectedBenchmarks.Where(b =>
                !((((MSBuildFlavor?)b.Parameters["MSBuildFlavor"]) == MSBuildFlavor.Framework ||
                   b.Target.Type.Name.IndexOf("Framework", StringComparison.OrdinalIgnoreCase) >= 0) &&
                  b.Parameters["MSBuildVersion"].ToString().Equals("NotApplicable", StringComparison.OrdinalIgnoreCase)));

            // If not specified, limit "MSBuildVersion" to "15.6" or "NotApplicable"
            if (!parameters.ContainsKey("MSBuildVersion"))
            {
                selectedBenchmarks = selectedBenchmarks.Where(b =>
                    b.Parameters["MSBuildVersion"] == null ||
                    ((string)b.Parameters["MSBuildVersion"]).StartsWith("15.6", StringComparison.OrdinalIgnoreCase) ||
                    ((string)b.Parameters["MSBuildVersion"]).Equals("NotApplicable", StringComparison.OrdinalIgnoreCase));
            }

            // If not specified, default "NodeReuse" to "true" to match typical customer usage.
            if (!parameters.ContainsKey("NodeReuse"))
            {
                selectedBenchmarks = selectedBenchmarks.Where(b => (bool?)b.Parameters["NodeReuse"] ?? true);
            }

            // Large apps and "SourceChanged" methods can choose from SourceChanged=Leaf/Root SourceChangeType=Implementation/Api.
            // All other apps and methods must use SourceChanged=NotApplicable and SourceChangeType=NotApplicable.
            selectedBenchmarks = selectedBenchmarks.Where(b =>
            {
                var sourceChanged = ((SourceChanged)b.Parameters["SourceChanged"]);
                var sourceChangeType = ((SourceChangeType)b.Parameters["SourceChangeType"]);

                if (b.Target.Type.Name.IndexOf("Large", StringComparison.OrdinalIgnoreCase) >= 0 &&
                    b.Target.Method.Name.IndexOf("SourceChanged", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return ((sourceChanged == SourceChanged.Leaf) || (sourceChanged == SourceChanged.Root)) &&
                           ((sourceChangeType == SourceChangeType.Implementation) || (sourceChangeType == SourceChangeType.Api));
                }
                else
                {
                    return sourceChanged == SourceChanged.NotApplicable && sourceChangeType == SourceChangeType.NotApplicable;
                }
            });

            // If not specified, remove SourceChanged=Root, since SourceChanged=Leaf is tested more often
            if (!parameters.ContainsKey("SourceChanged"))
            {
                selectedBenchmarks = selectedBenchmarks.Where(b => ((SourceChanged)b.Parameters["SourceChanged"]) != SourceChanged.Root);
            }

            // If not specified, remove SourceChangeType=Api, since SourceChangeType=Implementation is tested more often
            if (!parameters.ContainsKey("SourceChangeType"))
            {
                selectedBenchmarks = selectedBenchmarks.Where(b => ((SourceChangeType)b.Parameters["SourceChangeType"]) != SourceChangeType.Api);
            }

            // If not specified, default RazorCompileOnBuild to false
            if (!parameters.ContainsKey("RazorCompileOnBuild"))
            {
                selectedBenchmarks = selectedBenchmarks.Where(b => !(bool?)b.Parameters["RazorCompileOnBuild"] ?? true);
            }

            // If not specified, default TargetFramework to 2.0
            if (!parameters.ContainsKey("TargetFramework"))
            {
                selectedBenchmarks = selectedBenchmarks.Where(b =>
                    ((string)b.Parameters["TargetFramework"])?.Equals("2.0", StringComparison.OrdinalIgnoreCase) ?? true);
            }

            // NoBuild should be "false" or "true" for "Run" methods, else "null"
            selectedBenchmarks = selectedBenchmarks.Where(b =>
            {
                if (b.Target.Method.Name.StartsWith("run", StringComparison.OrdinalIgnoreCase))
                {
                    return (bool?)b.Parameters["NoBuild"] == false || (bool?)b.Parameters["NoBuild"] == true;
                }
                else
                {
                    return (bool?)b.Parameters["NoBuild"] == null;
                }
            });

            // If not specified, default NoBuild to null or false
            if (!parameters.ContainsKey("NoBuild"))
            {
                selectedBenchmarks = selectedBenchmarks.Where(b => (bool?)b.Parameters["NoBuild"] == false || (bool?)b.Parameters["NoBuild"] == null);
            }

            // If not specified, default TieredJit to null or false
            if (!parameters.ContainsKey("TieredJit"))
            {
                selectedBenchmarks = selectedBenchmarks.Where(b => (bool?)b.Parameters["TieredJit"] == false || (bool?)b.Parameters["TieredJit"] == null);
            }

            // If not specified, default ProduceReferenceAssembly to null or false
            if (!parameters.ContainsKey("ProduceReferenceAssembly"))
            {
                selectedBenchmarks = selectedBenchmarks.Where(b => (bool?)b.Parameters["ProduceReferenceAssembly"] == false || (bool?)b.Parameters["ProduceReferenceAssembly"] == null);
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

        private static IDictionary<string, IEnumerable<string>> ParametersToDictionary(IEnumerable<string> parameters)
        {
            var dict = new Dictionary<string, IEnumerable<string>>(parameters.Count(), StringComparer.OrdinalIgnoreCase);

            foreach (var p in parameters)
            {
                var parts = p.Split('=');
                var patterns = parts[1].Split('|');
                dict.Add(parts[0], patterns);
            }

            return dict;
        }

        private class NoOpOrderProvider : BenchmarkDotNet.Order.DefaultOrderProvider
        {
            // DefaultOrderProvider.GetExecutionOrder() is very slow.  Most time spent in String.Join().
            public override IEnumerable<Benchmark> GetExecutionOrder(Benchmark[] benchmarks)
            {
                return benchmarks;
            }
        }
    }
}
