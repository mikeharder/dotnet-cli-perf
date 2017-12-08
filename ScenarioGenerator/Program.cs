using CommandLine;
using Common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ScenarioGenerator
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
    }

    class Program
    {
        private const int _sourceFilesPerProject = 100;

        private static Options _options;
        private static readonly IEnumerable<string> _frameworks = new string[] { "core" };

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

            var type = Type.GetType($"ScenarioGenerator.{_options.Solution}", throwOnError: true, ignoreCase: true);
            ISolution template = (ISolution)Activator.CreateInstance(type);

            var threads = _frameworks.Count() * template.Projects.Count();
            ThreadPool.SetMaxThreads(threads, threads);
            ThreadPool.SetMinThreads(threads, threads);

            GenerateSolution(tempDir, template, _options.Framework);

            return 0;
        }

        private static void GenerateSolution(string tempDir, ISolution template, Framework framework)
        {
            var frameworkPath = framework.ToString().ToLowerInvariant();

            var solutionDir = Path.Combine(tempDir, frameworkPath);
            Directory.CreateDirectory(solutionDir);

            var (projects, mainProject) = _options.SimplifyNames ?
                SimplifyProjectNames(template.Projects, template.MainProject, template.Scenario) :
                (template.Projects, template.MainProject);

            // Generate sln
            if (framework == Framework.Core)
            {
                Util.RunProcess("dotnet", $"new sln -n {mainProject}", solutionDir);
            }
            else if (framework == Framework.Framework)
            {
                if (template.Scenario == Scenario.Web)
                {
                    File.Copy(Path.Combine(Util.RepoRoot, "templates", "framework", "mvc.sln"), Path.Combine(solutionDir, $"{mainProject}.sln"));
                }
            }

            var projectFiles = new ConcurrentBag<string>();
            Parallel.ForEach(projects, p => projectFiles.Add(GenerateProject(
                p.Name, p.Name == mainProject, template.Scenario, p.ProjectReferences, framework, solutionDir)));

            Console.WriteLine("Adding projects to solution");
            AddProjectsToSolution($"{mainProject}.sln", projectFiles, solutionDir);
        }

        private static (IList<(string Name, IEnumerable<string> ProjectReferences)> projects, string mainProject) SimplifyProjectNames(
            IList<(string Name, IEnumerable<string> ProjectReferences)> projects, string mainProject, Scenario scenario)
        {
            var newProjects = new List<(string Name, IEnumerable<string> ProjectReferences)>(projects.Count);
            var newNames = new Dictionary<string, string>(projects.Count);

            var index = 1;
            foreach (var p in projects)
            {
                string newName;
                if (p.Name == mainProject)
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

                newProjects.Add((newName, newProjectReferences));
            }

            return (newProjects, newNames[mainProject]);
        }

        private static string GenerateProject(string name, bool mainProject, Scenario scenario,
            IEnumerable<string> projectReferences, Framework framework, string solutionDir)
        {
            Console.WriteLine($"Generating {name}");

            switch (framework)
            {
                case Framework.Core:
                    return GenerateProjectCore(name, mainProject, scenario, projectReferences, solutionDir);
                case Framework.Framework:
                    return GenerateProjectFramework(name, mainProject, scenario, projectReferences, solutionDir);
                default:
                    throw new InvalidOperationException();
            }
        }

        private static string GenerateProjectGradle(string name, bool mainProject, Scenario scenario,
            IEnumerable<string> projectReferences, string frameworkPath, string solutionDir)
        {
            return null;
        }

        private static string GenerateProjectFramework(string name, bool mainProject, Scenario scenario,
            IEnumerable<string> projectReferences, string solutionDir)
        {
            var destDir = Path.Combine(solutionDir, name);
            var destProj = Path.Combine(destDir, $"{name}.csproj");

            if (mainProject)
            {
                if (scenario == Scenario.Web)
                {
                    var sourceDir = Path.Combine(Util.RepoRoot, "templates", "framework", "mvc");
                    var viewsDir = Path.Combine(destDir, "Views");
                    var controllersDir = Path.Combine(destDir, "Controllers");

                    // Copy template
                    Util.DirectoryCopy(sourceDir, destDir, copySubDirs: true);

                    // Rename csproj
                    File.Move(Path.Combine(destDir, "mvc.csproj"), destProj);

                    // Create copies of HomeController.cs and "Views\Home" folder
                    for (var i = 2; i <= _sourceFilesPerProject; i++)
                    {
                        var destName = "Home" + i.ToString("D3") + "Controller";
                        var destFile = Path.Combine(controllersDir, destName + ".cs");

                        File.Copy(Path.Combine(controllersDir, "HomeController.cs"), destFile);
                        Util.ReplaceInFile(destFile, "HomeController", destName);

                        Util.DirectoryCopy(Path.Combine(viewsDir, "Home"),
                            Path.Combine(viewsDir, "Home" + i.ToString("D3")),
                            copySubDirs: true);
                    }

                    // Add ProjectReference lines to csproj
                    AddProjectReferences(Path.Combine(destDir, $"{name}.csproj"), projectReferences);

                    // Update HomeController with dependencies
                    AddPropertyReferences(Path.Combine(controllersDir, "HomeController.cs"), "\"InitialValue\"", projectReferences);
                }
            }
            else
            {
                var sourceDir = Path.Combine(Util.RepoRoot, "templates", "framework", "classlib");

                // Copy template
                Util.DirectoryCopy(sourceDir, destDir, copySubDirs: true);

                // Rename csproj
                File.Move(Path.Combine(destDir, "classlib.csproj"), destProj);

                // Change namespace and string in Class001.cs
                Util.ReplaceInFile(Path.Combine(destDir, "Class001.cs"), "classlib", name);

                // Create copies of Class001.cs
                for (var i = 2; i <= _sourceFilesPerProject; i++)
                {
                    var destName = "Class" + i.ToString("D3");
                    var destFile = Path.Combine(destDir, destName + ".cs");
                    File.Copy(Path.Combine(destDir, "Class001.cs"), destFile);
                    Util.ReplaceInFile(destFile, "Class001", destName);
                }

                // Add ProjectReference lines to csproj
                AddProjectReferences(Path.Combine(destDir, $"{name}.csproj"), projectReferences);

                // Update Class001.Property with dependencies
                AddPropertyReferences(Path.Combine(destDir, $"Class001.cs"), $"\"{name}\"", projectReferences);
            }

            return destProj;
        }

        private static string GenerateProjectCore(string name, bool mainProject, Scenario scenario,
            IEnumerable<string> projectReferences, string solutionDir)
        {
            var destDir = Path.Combine(solutionDir, name);
            var destProj = Path.Combine(destDir, $"{name}.csproj");

            if (mainProject)
            {
                if (scenario == Scenario.Web)
                {
                    var sourceDir = Path.Combine(Util.RepoRoot, "templates", "core", "mvc");
                    var viewsDir = Path.Combine(destDir, "Views");
                    var controllersDir = Path.Combine(destDir, "Controllers");

                    // Copy template
                    Util.DirectoryCopy(sourceDir, destDir, copySubDirs: true);

                    // Rename csproj
                    File.Move(Path.Combine(destDir, "mvc.csproj"), destProj);

                    // Create copies of HomeController.cs and "Views\Home" folder
                    for (var i = 2; i <= _sourceFilesPerProject; i++)
                    {
                        var destName = "Home" + i.ToString("D3") + "Controller";
                        var destFile = Path.Combine(controllersDir, destName + ".cs");

                        File.Copy(Path.Combine(controllersDir, "HomeController.cs"), destFile);
                        Util.ReplaceInFile(destFile, "HomeController", destName);

                        Util.DirectoryCopy(Path.Combine(viewsDir, "Home"),
                            Path.Combine(viewsDir, "Home" + i.ToString("D3")),
                            copySubDirs: true);
                    }

                    // Add ProjectReference lines to csproj
                    AddProjectReferences(Path.Combine(destDir, $"{name}.csproj"), projectReferences);

                    // Update HomeController with dependencies
                    AddPropertyReferences(Path.Combine(controllersDir, "HomeController.cs"), "\"InitialValue\"", projectReferences);
                }
            }
            else
            {
                var sourceDir = Path.Combine(Util.RepoRoot, "templates", "core", "classlib");

                // Copy template
                Util.DirectoryCopy(sourceDir, destDir, copySubDirs: true);

                // Rename csproj
                File.Move(Path.Combine(destDir, "classlib.csproj"), destProj);

                // Change namespace and string in Class001.cs
                Util.ReplaceInFile(Path.Combine(destDir, "Class001.cs"), "classlib", name);

                // Create copies of Class001.cs
                for (var i = 2; i <= _sourceFilesPerProject; i++)
                {
                    var destName = "Class" + i.ToString("D3");
                    var destFile = Path.Combine(destDir, destName + ".cs");
                    File.Copy(Path.Combine(destDir, "Class001.cs"), destFile);
                    Util.ReplaceInFile(destFile, "Class001", destName);
                }

                // Add ProjectReference lines to csproj
                AddProjectReferences(Path.Combine(destDir, $"{name}.csproj"), projectReferences);

                // Update Class001.Property with dependencies
                AddPropertyReferences(Path.Combine(destDir, $"Class001.cs"), $"\"{name}\"", projectReferences);
            }

            return destProj;
        }

        private static void AddProjectsToSolution(string solution, IEnumerable<string> projects, string workingDirectory)
        {
            // Add proj to sln
            Util.RunProcess("dotnet", $"sln {solution} add {String.Join(' ', projects)}", workingDirectory);
        }

        private static void AddProjectReferences(string path, IEnumerable<string> projectReferences)
        {
            var root = XElement.Load(path);
            var itemGroup = root.Descendants(XName.Get("ItemGroup", root.Name.NamespaceName)).First();

            foreach (var p in projectReferences)
            {
                itemGroup.Add(new XElement("ProjectReference", new XAttribute("Include", Path.Combine("..", p, $"{p}.csproj"))));
            }

            root.Save(path);
        }

        private static void AddPropertyReferences(string path, string initialValue, IEnumerable<string> projectReferences)
        {
            var sb = new StringBuilder();
            foreach (var p in projectReferences)
            {
                sb.Append($" + \" \" + {p}.Class001.Property");
            }
            Util.ReplaceInFile(path, initialValue, initialValue + sb.ToString());
        }
    }
}
