using Common;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScenarioGenerator
{
    class GradleSolutionGenerator : ISolutionGenerator
    {
        public void GenerateSolution(string path, ISolution template, Scenario scenario)
        {
            Util.DirectoryCopy(Path.Combine(Util.RepoRoot, "templates", "gradle", "root"), path, copySubDirs: true);

            Parallel.ForEach(template.Projects, p => GenerateProject(
                path, p.Name, p.Name == template.MainProject, scenario, p.ProjectReferences));

            AddProjectsToSettings(path, template.Projects.Select(p => p.Name));
        }

        private static void GenerateProject(string path, string name, bool mainProject, Scenario scenario, IEnumerable<string> projectReferences)
        {
            var destDir = Path.Combine(path, name);

            if (mainProject)
            {
                if (scenario == Scenario.Web)
                {
                    var sourceDir = Path.Combine(Util.RepoRoot, "templates", "gradle", "mvc");
                    var viewsDir = Path.Combine(destDir, "src", "main", "resources", "templates");
                    var controllersDir = Path.Combine(destDir, "src", "main", "java", "hello");

                    // Copy template
                    Util.DirectoryCopy(sourceDir, destDir, copySubDirs: true);

                    // Create copies of HomeController.java and index.html
                    for (var i=2; i < Program.SourceFilesPerProject; i++)
                    {
                        var destName = "Home" + i.ToString("D3") + "Controller";
                        var destFile = Path.Combine(controllersDir, destName + ".java");

                        File.Copy(Path.Combine(controllersDir, "HomeController.java"), destFile);
                        Util.ReplaceInFile(destFile, "HomeController", destName);
                        Util.ReplaceInFile(destFile, @"@RequestMapping(""/"")", "@RequestMapping(\"/home" + i.ToString("D3") + "\")");
                        Util.ReplaceInFile(destFile, "\"index\"", "\"index" + i.ToString("D3") + "\"");

                        File.Copy(Path.Combine(viewsDir, "index.html"), Path.Combine(viewsDir, "index" + i.ToString("D3") + ".html"));
                    }

                    // Add references to build.gradle
                    AddProjectReferences(Path.Combine(destDir, "build.gradle"), projectReferences);

                    // Update HomeController with dependencies
                    AddPropertyReferences(Path.Combine(controllersDir, "HomeController.java"), "\"InitialValue\"", projectReferences);
                }
            }
            else
            {
                var sourceDir = Path.Combine(Util.RepoRoot, "templates", "gradle", "classlib");
                var classDir = Path.Combine(destDir, "src", "main", "java", name);

                // Copy template
                Util.DirectoryCopy(sourceDir, destDir, copySubDirs: true);

                // Rename "classlib" folder to $"{name}"
                Directory.Move(Path.Combine(destDir, "src", "main", "java", "classlib"), classDir);

                // Change package and string in Class001.java
                Util.ReplaceInFile(Path.Combine(classDir, "Class001.java"), "classlib", name);

                // Rename jar in build.gradle
                Util.ReplaceInFile(Path.Combine(destDir, "build.gradle"), "classlib", name);

                // Create copies of Class001.java
                for (var i = 2; i <= Program.SourceFilesPerProject; i++)
                {
                    var destName = "Class" + i.ToString("D3");
                    var destFile = Path.Combine(classDir, destName + ".java");
                    File.Copy(Path.Combine(classDir, "Class001.java"), destFile);
                    Util.ReplaceInFile(destFile, "Class001", destName);
                }

                // Add references to build.gradle
                AddProjectReferences(Path.Combine(destDir, "build.gradle"), projectReferences);

                // Update Class001.property() with dependencies
                AddPropertyReferences(Path.Combine(classDir, $"Class001.java"), $"\"{name}\"", projectReferences);
            }
        }

        private static void AddProjectReferences(string path, IEnumerable<string> projectReferences)
        {
            var sb = new StringBuilder();

            sb.AppendLine();
            sb.AppendLine("dependencies {");
            foreach (var projectReference in projectReferences)
            {
                sb.AppendLine($"    compile project(':{projectReference}')");
            }
            sb.AppendLine("}");

            File.AppendAllText(path, sb.ToString());
        }

        private static void AddProjectsToSettings(string path, IEnumerable<string> projects)
        {
            var sb = new StringBuilder();
            foreach (var project in projects)
            {
                sb.AppendLine($"include '{project}'");
            }
            File.WriteAllText(Path.Combine(path, "settings.gradle"), sb.ToString());
        }

        private static void AddPropertyReferences(string path, string initialValue, IEnumerable<string> projectReferences)
        {
            var sb = new StringBuilder();
            foreach (var p in projectReferences)
            {
                sb.Append($" + \" \" + {p}.Class001.property()");
            }
            Util.ReplaceInFile(path, initialValue, initialValue + sb.ToString());
        }

    }
}
