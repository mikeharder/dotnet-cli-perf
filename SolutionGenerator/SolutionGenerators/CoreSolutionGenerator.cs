using Common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SolutionGenerator
{
    class CoreSolutionGenerator : DotNetSolutionGenerator, ISolutionGenerator
    {
        public void GenerateSolution(string path, ISolution template, Scenario scenario)
        {
            var mainProject = template.MainProject;

            Util.RunProcess("dotnet", $"new sln -n {mainProject}", path);

            var projectFiles = new ConcurrentBag<string>();
            Parallel.ForEach(template.Projects, p => projectFiles.Add(GenerateProject(
                path, p.Name, p.Name == mainProject, scenario, p.ProjectReferences, p.PackageReferences)));

            Console.WriteLine("Adding projects to solution");
            AddProjectsToSolution(path, $"{mainProject}.sln", projectFiles, mainProject);
        }

        private string GenerateProject(string path, string name, bool mainProject, Scenario scenario,
            IEnumerable<string> projectReferences, IEnumerable<(string Name, string Version)> packageReferences)
        {
            var destDir = Path.Combine(path, name);
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
                    for (var i = 2; i <= Program.SourceFilesPerProject; i++)
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

                    // Do not add PackageReferences, since the web app needs to use its own PackageReferences to ensure
                    // it builds and runs correctly

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
                for (var i = 2; i <= Program.SourceFilesPerProject; i++)
                {
                    var destName = "Class" + i.ToString("D3");
                    var destFile = Path.Combine(destDir, destName + ".cs");
                    File.Copy(Path.Combine(destDir, "Class001.cs"), destFile);
                    Util.ReplaceInFile(destFile, "Class001", destName);
                }

                // Add ProjectReference lines to csproj
                AddProjectReferences(destProj, projectReferences);

                // Add PackageReference lines to csproj
                AddPackageReferences(destProj, packageReferences);

                // Update Class001.Property with dependencies
                AddPropertyReferences(Path.Combine(destDir, $"Class001.cs"), $"\"{name}\"", projectReferences);
            }

            return destProj;
        }

        private void AddProjectsToSolution(string path, string solutionName, IEnumerable<string> projects, string mainProject)
        {
            Util.RunProcess("dotnet", $"sln {solutionName} add {String.Join(' ', projects)}", path);
        }
    }
}
