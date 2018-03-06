using Common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ScenarioGenerator
{
    class FrameworkSolutionGenerator : DotNetSolutionGenerator, ISolutionGenerator
    {
        public void GenerateSolution(string path, ISolution template, Scenario scenario)
        {
            var mainProject = template.MainProject;

            if (scenario == Scenario.Web)
            {
                File.Copy(Path.Combine(Util.RepoRoot, "templates", "framework", "mvc.sln"), Path.Combine(path, $"{template.MainProject}.sln"));
            }

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
                    var sourceDir = Path.Combine(Util.RepoRoot, "templates", "framework", "mvc");
                    var viewsDir = Path.Combine(destDir, "Views");
                    var controllersDir = Path.Combine(destDir, "Controllers");

                    // Copy template
                    Util.DirectoryCopy(sourceDir, destDir, copySubDirs: true);

                    // Rename csproj
                    File.Move(Path.Combine(destDir, "mvc.csproj"), destProj);

                    var newFiles = new List<string>();
                    // Create copies of HomeController.cs and "Views\Home" folder
                    for (var i = 2; i <= Program.SourceFilesPerProject; i++)
                    {
                        var destName = "Home" + i.ToString("D3") + "Controller";
                        var destFile = Path.Combine(controllersDir, destName + ".cs");

                        File.Copy(Path.Combine(controllersDir, "HomeController.cs"), destFile);
                        Util.ReplaceInFile(destFile, "HomeController", destName);
                        newFiles.Add(Path.Combine("Controllers", destName + ".cs"));

                        Util.DirectoryCopy(Path.Combine(viewsDir, "Home"),
                            Path.Combine(viewsDir, "Home" + i.ToString("D3")),
                            copySubDirs: true);
                        newFiles.Add(Path.Combine("Views", "Home" + i.ToString("D3"), "About.cshtml"));
                        newFiles.Add(Path.Combine("Views", "Home" + i.ToString("D3"), "Contact.cshtml"));
                        newFiles.Add(Path.Combine("Views", "Home" + i.ToString("D3"), "Index.cshtml"));
                    }
                    AddFilesToProject(destProj, newFiles);

                    // Add ProjectReference lines to csproj
                    AddProjectReferences(destProj, projectReferences);

                    // Do not add PackageReferences, since the web app needs to use its own PackageReferences to ensure
                    // it builds and runs correctly

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

                var newFiles = new List<string>();
                // Create copies of Class001.cs
                for (var i = 2; i <= Program.SourceFilesPerProject; i++)
                {
                    var destName = "Class" + i.ToString("D3");
                    var destFile = Path.Combine(destDir, destName + ".cs");
                    File.Copy(Path.Combine(destDir, "Class001.cs"), destFile);
                    Util.ReplaceInFile(destFile, "Class001", destName);
                    newFiles.Add(destName + ".cs");
                }
                AddFilesToProject(destProj, newFiles);

                // Add ProjectReference lines to csproj
                AddProjectReferences(destProj, projectReferences);

                // Add PackageReferences to packages.config
                AddPackageReferences(destProj, packageReferences);

                // Update Class001.Property with dependencies
                AddPropertyReferences(Path.Combine(destDir, $"Class001.cs"), $"\"{name}\"", projectReferences);

                // Change GUID in csproj
                Util.ReplaceInFile(destProj, "a2806542-3ef7-40c1-9b10-ca07b164b420",
                    Guid.NewGuid().ToString().ToUpperInvariant());

                // Update RootNamespace and AssemblyName
                Util.ReplaceInFile(destProj, ">classlib<", $">{name}<");
            }

            return destProj;
        }

        private static void AddFilesToProject(string path, IEnumerable<string> files)
        {
            var root = XElement.Load(path);
            var itemGroup = root.Descendants(XName.Get("ItemGroup", root.Name.NamespaceName)).First();

            foreach (var file in files)
            {
                itemGroup.Add(new XElement(
                    XName.Get((Path.GetExtension(file) == ".cs") ? "Compile" : "Content", root.Name.NamespaceName),
                    new XAttribute("Include", file)));
            }

            root.Save(path);
        }

        private void AddProjectsToSolution(string path, string solutionName, IEnumerable<string> projects, string mainProject)
        {
            var projectRefs = new StringBuilder();
            var configs = new StringBuilder();

            foreach (var project in projects)
            {
                var name = Path.GetFileNameWithoutExtension(project);

                if (name.Equals(mainProject, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                var root = XElement.Load(Path.Combine(path, project));
                var guid = root.Descendants(XName.Get("ProjectGuid", root.Name.NamespaceName)).Single().Value;

                // Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "classlib", "classlib\classlib.csproj", "{6F6148E3-3C37-4B65-B3F0-8D58571761A8}"
                // EndProject
                projectRefs.Append("Project(\"{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}\") = ");
                projectRefs.AppendLine($"\"{name}\", \"{name}\\{name}.csproj\", \"{{{guid}}}\"");
                projectRefs.AppendLine("EndProject");

                // {6F6148E3-3C37-4B65-B3F0-8D58571761A8}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
                // {6F6148E3-3C37-4B65-B3F0-8D58571761A8}.Debug|Any CPU.Build.0 = Debug|Any CPU
                // {6F6148E3-3C37-4B65-B3F0-8D58571761A8}.Release|Any CPU.ActiveCfg = Release|Any CPU
                // {6F6148E3-3C37-4B65-B3F0-8D58571761A8}.Release|Any CPU.Build.0 = Release|Any CPU
                configs.AppendLine($"\t\t{{{guid.ToUpperInvariant()}}}.Debug|Any CPU.ActiveCfg = Debug|Any CPU");
                configs.AppendLine($"\t\t{{{guid.ToUpperInvariant()}}}.Debug|Any CPU.Build.0 = Debug|Any CPU");
                configs.AppendLine($"\t\t{{{guid.ToUpperInvariant()}}}.Release|Any CPU.ActiveCfg = Debug|Any CPU");
                configs.AppendLine($"\t\t{{{guid.ToUpperInvariant()}}}.Release|Any CPU.Build.0 = Debug|Any CPU");
            }

            Util.InsertInFileAfter(
                Path.Combine(path, solutionName),
                "EndProject" + Environment.NewLine,
                projectRefs.ToString());

            Util.InsertInFileAfter(
                Path.Combine(path, solutionName),
                "{F9625F4A-BB92-465C-A8A4-2D13A8A99086}.Release|Any CPU.Build.0 = Release|Any CPU" + Environment.NewLine,
                configs.ToString());
        }
    }
}
