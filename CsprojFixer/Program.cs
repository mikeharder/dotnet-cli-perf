﻿using Common;
using System;
using System.Collections.Generic;
using System.IO;

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
            }
        }

        private static void EnsureProjectGuidContainsBraces(string path)
        {
            Util.RegexReplaceInFile(path, "<ProjectGuid>([^{]*)</ProjectGuid>", "<ProjectGuid>{$1}</ProjectGuid>");
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
