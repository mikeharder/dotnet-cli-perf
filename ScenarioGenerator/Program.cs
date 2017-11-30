using CommandLine;
using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

                foreach (var (name, projectReferences) in template.Projects)
                {
                    Console.WriteLine($"Generating {name}");

                    if (name == template.MainProject)
                    {

                    }
                    else
                    {
                        var sourceDir = Path.Combine(Util.RepoRoot, "templates", "classlib", framework);
                        var destDir = Path.Combine(solutionDir, name);
                        Util.DirectoryCopy(sourceDir, destDir, copySubDirs: true);

                        // Rename csproj
                        File.Move(Path.Combine(destDir, "classlib.csproj"), Path.Combine(destDir, $"{name}.csproj"));

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
                        AddPropertyReferences(Path.Combine(destDir, $"Class001.cs"), projectReferences);
                    }
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

        private static void AddPropertyReferences(string path, IEnumerable<string> projectReferences)
        {
            foreach (var p in projectReferences)
            {
                Util.ReplaceInFile(path, ";", $" + \" \" + {p}.Class001.Property;");
            }
        }
    }
}
