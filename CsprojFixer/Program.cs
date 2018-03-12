using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace CsprojFixer
{
    class Program
    {
        static void Main(string[] args)
        {
            var projects = GetProjects(args[0]).ToArray();
            var guids = new Dictionary<string, string>();

            foreach (var project in projects)
            {
                Console.WriteLine(project);

                EnsureProjectGuidContainsBraces(project);
                EnsureProjectGuidIsUpperCase(project);

                // First pass to collect guids
                guids.Add(project, GetGuid(project));
            }

            // Second pass to update ProjectReference elements
            foreach (var project in projects)
            {
                EnsureProjectReferencesIncludeGuidAndName(project, guids);
            }
        }

        private static void EnsureProjectReferencesIncludeGuidAndName(string path, IReadOnlyDictionary<string, string> guids)
        {
            // OLD
            // <ProjectReference Include="..\ClassLib127\ClassLib127.csproj" />

            // NEW
            // <ProjectReference Include="..\ClassLib127\ClassLib127.csproj">
            //   <Project>{0A8C9BDD-4ED0-4F14-BC1B-31E8047CCF1F}</Project>
            //   <Name>ClassLib127</Name>
            // </ProjectReference>

            var text = File.ReadAllText(path);

            // SDK projects do not require GUID and Name in ProjectReference
            if (text.Contains("<Project Sdk=\"Microsoft.NET.Sdk\">"))
            {
                return;
            }

            var matches = Regex.Matches(text, "<ProjectReference Include=\"(.*)\" />");
            foreach (var match in matches.AsEnumerable<Match>())
            {
                var referencedProject = match.Groups.Skip(1).SingleOrDefault().ToString();
                var fullPath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(path), referencedProject));
                var guid = guids[fullPath];
                var name = Path.GetFileNameWithoutExtension(fullPath);

                var newReference = $"<ProjectReference Include=\"{referencedProject}\">" + Environment.NewLine +
                    $"      <Project>{{{guid}}}</Project>" + Environment.NewLine +
                    $"      <Name>{name}</Name>" + Environment.NewLine +
                    $"    </ProjectReference>";

                Util.ReplaceInFile(path, match.Value, newReference);
            }

        }

        private static void EnsureProjectGuidContainsBraces(string path)
        {
            Util.RegexReplaceInFile(path, "<ProjectGuid>([^{]*)</ProjectGuid>", "<ProjectGuid>{$1}</ProjectGuid>");
        }

        private static void EnsureProjectGuidIsUpperCase(string path)
        {
            var guid = GetGuid(path);
            if (guid != null)
            {
                var upperGuid = guid.ToUpperInvariant();
                if (guid != upperGuid)
                {
                    Util.ReplaceInFile(path, guid, upperGuid);
                }
            }
        }

        private static string GetGuid(string path)
        {
            var group = Regex.Match(File.ReadAllText(path), "<ProjectGuid>{(.*)}</ProjectGuid>").Groups.Skip(1).SingleOrDefault();
            return group?.ToString();
        }

        private static IEnumerable<string> GetProjects(string path)
        {
            foreach (var file in Directory.EnumerateFiles(path))
            {
                if (Path.GetExtension(file) == ".csproj")
                {
                    yield return file;
                }
            }

            foreach (var directory in Directory.EnumerateDirectories(path))
            {
                foreach (var file in GetProjects(directory))
                {
                    yield return file;
                }
            }
        }
    }
}
