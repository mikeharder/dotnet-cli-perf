using CommandLine;
using Common;
using SolutionGenerator.Solutions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SolutionGenerator
{
    class Options
    {
        [Option('f', "framework", Required = true)]
        public Framework Framework { get; set; }

        [Option("simplifyNames")]
        public bool SimplifyNames { get; set; } = true;

        [Option('s', "solution", Required = true)]
        public string Solution { get; set; }

        [Option('o', "outputDir")]
        public string OutputDir { get; set; }

        [Option('c', "scenario", Required = true)]
        public Scenario Scenario { get; set; }

        [Option('p', "packageManagementFormat")]
        public PackageManagementFormat PackageManagementFormat { get; set; }
    }

    class Program
    {
        public static int SourceFilesPerProject => 100;

        private static Options _options;

        private static readonly ConcurrentDictionary<string, object> _solutionLocks = new ConcurrentDictionary<string, object>();

        static int Main(string[] args)
        {
            return Parser.Default.ParseArguments<Options>(args).MapResult(
                options => Run(options),
                _ => 1
            );
        }

        private static int Run(Options options)
        {
            _options = options;

            var tempDir = _options.OutputDir == null ? Util.GetTempDir() : Util.GetTempDir(_options.OutputDir);

            var type = Type.GetType($"SolutionGenerator.Solutions.{_options.Solution}Solution", throwOnError: true, ignoreCase: true);
            ISolution template = (ISolution)Activator.CreateInstance(type);

            if (template is IPackageManagementFormat packageManagementTemplate)
            {
                packageManagementTemplate.PackageManagementFormat = _options.PackageManagementFormat;
            }

            var threads = template.Projects.Count();
            ThreadPool.SetMaxThreads(threads, threads);
            ThreadPool.SetMinThreads(threads, threads);

            GenerateSolution(tempDir, template, _options.Framework);

            return 0;
        }

        private static void GenerateSolution(string tempDir, ISolution template, Framework framework)
        {
            template = _options.SimplifyNames ? SimplifyProjectNames(template, _options.Scenario) : template;
            SolutionGeneratorFactory.GetInstance(framework).GenerateSolution(tempDir, template, _options.Scenario);
        }

        private static ISolution SimplifyProjectNames(ISolution solution, Scenario scenario)
        {
            var newProjects = new List<(string Name, IEnumerable<string> ProjectReferences, IEnumerable<(string Name, string Version)> PackageReferences)>(solution.Projects.Count);
            var newNames = new Dictionary<string, string>(solution.Projects.Count);

            var index = 1;
            foreach (var p in solution.Projects)
            {
                string newName;
                if (p.Name == solution.MainProject)
                {
                    if (scenario == Scenario.Web)
                    {
                        newName = "mvc";
                    }
                    else
                    {
                        throw new InvalidOperationException();
                    }
                }
                else
                {
                    newName = $"ClassLib{index.ToString("D3")}";
                    index++;
                }

                newNames.Add(p.Name, newName);

                var newProjectReferences = new List<string>(p.ProjectReferences.Count());
                foreach (var pr in p.ProjectReferences)
                {
                    newProjectReferences.Add(newNames[pr]);
                }

                newProjects.Add((newName, newProjectReferences, p.PackageReferences));
            }

            return new ManualSolution(newProjects, newNames[solution.MainProject]);
        }
    }
}
