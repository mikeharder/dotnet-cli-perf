using CommandLine;
using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ScenarioGenerator
{
    class Options
    {
        [Option('s', "solution")]
        public string Solution { get; set; }
    }

    class Program
    {
        private const int _sourceFilesPerProject = 100;

        private static Options _options;
        private static readonly IEnumerable<string> _frameworks = new string[] { "core" };

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

            var tempDir = Util.GetTempDir();

            var type = Type.GetType($"ScenarioGenerator.{_options.Solution}", throwOnError: true, ignoreCase: true);
            ISolution template = (ISolution)Activator.CreateInstance(type);

            foreach (var framework in _frameworks)
            {
                var solutionDir = Path.Combine(tempDir, framework);
                Directory.CreateDirectory(solutionDir);

                // Generate sln
                Util.RunProcess("dotnet", $"new sln -n {template.MainProject}", solutionDir);

                foreach (var (name, projectReferences) in template.Projects)
                {
                    Console.WriteLine($"Generating {name}");

                    var destDir = Path.Combine(solutionDir, name);
                    var destProj = Path.Combine(destDir, $"{name}.csproj");

                    if (name == template.MainProject)
                    {
                        if (template.Scenario == Scenario.WebApp)
                        {
                            var sourceDir = Path.Combine(Util.RepoRoot, "templates", "mvc", framework);
                            var viewsDir = Path.Combine(destDir, "Views");
                            var controllersDir = Path.Combine(destDir, "Controllers");

                            // Copy template
                            Util.DirectoryCopy(sourceDir, destDir, copySubDirs: true);

                            // Rename csproj
                            File.Move(Path.Combine(destDir, "mvc.csproj"), destProj);

                            // Create copies of HomeController.cs and "Views\Home" folder
                            for (var i=2; i <= _sourceFilesPerProject; i++)
                            {
                                File.Copy(Path.Combine(controllersDir, "HomeController.cs"),
                                    Path.Combine(controllersDir, "Home" + i.ToString("D3") + "Controller.cs"));

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
                        var sourceDir = Path.Combine(Util.RepoRoot, "templates", "classlib", framework);

                        // Copy template
                        Util.DirectoryCopy(sourceDir, destDir, copySubDirs: true);

                        // Rename csproj
                        File.Move(Path.Combine(destDir, "classlib.csproj"), destProj);

                        // Add to sln
                        Util.RunProcess("dotnet", $"sln {template.MainProject}.sln add {destProj}", solutionDir);

                        // Change namespace and string in Class001.cs
                        Util.ReplaceInFile(Path.Combine(destDir, "Class001.cs"), "classlib", name);

                        // Create copies of Class001.cs
                        for (var i=2; i <= _sourceFilesPerProject; i++)
                        {
                            File.Copy(Path.Combine(destDir, "Class001.cs"), Path.Combine(destDir, "Class" + i.ToString("D3") + ".cs"));
                        }

                        // Add ProjectReference lines to csproj
                        AddProjectReferences(Path.Combine(destDir, $"{name}.csproj"), projectReferences);

                        // Update Class001.Property with dependencies
                        AddPropertyReferences(Path.Combine(destDir, $"Class001.cs"), $"\"{name}\"", projectReferences);
                    }

                    // Add proj to sln
                    Util.RunProcess("dotnet", $"sln {template.MainProject}.sln add {destProj}", solutionDir);
                }
            }

            return 0;
        }
        
        private static void AddProjectReferences(string path, IEnumerable<string> projectReferences)
        {
            var root = XElement.Load(path);
            var itemGroup = root.Descendants("ItemGroup").Single();

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
