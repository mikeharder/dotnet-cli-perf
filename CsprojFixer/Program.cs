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
            var projects = GetProjects(args[0]);
            foreach (var project in projects)
            {
                Console.WriteLine(project);
                EnsureProjectGuidContainsBraces(project);
                EnsureProjectGuidIsUpperCase(project);
            }
        }

        private static void EnsureProjectGuidContainsBraces(string path)
        {
            Util.RegexReplaceInFile(path, "<ProjectGuid>([^{]*)</ProjectGuid>", "<ProjectGuid>{$1}</ProjectGuid>");
        }

        private static void EnsureProjectGuidIsUpperCase(string path)
        {
            var group = Regex.Match(File.ReadAllText(path), "<ProjectGuid>{(.*)}</ProjectGuid>").Groups.Skip(1).SingleOrDefault();
            if (group != null)
            {
                var guid = group.ToString();
                var upperGuid = guid.ToUpperInvariant();
                if (guid != upperGuid)
                {
                    Util.ReplaceInFile(path, guid, upperGuid);
                }
            }
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
