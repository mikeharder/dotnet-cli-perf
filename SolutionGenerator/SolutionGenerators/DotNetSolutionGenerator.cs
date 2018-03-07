﻿using Common;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace SolutionGenerator
{
    abstract class DotNetSolutionGenerator
    {
        protected static void AddPackageReferences(string path, IEnumerable<(string Name, string Version)> packageReferences)
        {
            var root = XElement.Load(path);
            var itemGroup = root.Descendants(XName.Get("ItemGroup", root.Name.NamespaceName)).First();

            foreach (var p in packageReferences)
            {
                itemGroup.Add(new XElement(
                    XName.Get("PackageReference", root.Name.NamespaceName),
                    new XAttribute("Include", p.Name),
                    new XAttribute("Version", p.Version)));
            }

            root.Save(path);
        }

        protected static void AddProjectReferences(string path, IEnumerable<string> projectReferences)
        {
            var root = XElement.Load(path);
            var itemGroup = root.Descendants(XName.Get("ItemGroup", root.Name.NamespaceName)).First();

            foreach (var p in projectReferences)
            {
                itemGroup.Add(new XElement(
                    XName.Get("ProjectReference", root.Name.NamespaceName),
                    new XAttribute("Include", Path.Combine("..", p, $"{p}.csproj"))));
            }

            root.Save(path);
        }

        protected static void AddPropertyReferences(string path, string initialValue, IEnumerable<string> projectReferences)
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
