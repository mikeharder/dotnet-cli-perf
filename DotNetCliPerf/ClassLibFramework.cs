using System.Collections.Generic;
using System.IO;

namespace DotNetCliPerf
{
    public class ClassLibFramework : FrameworkApp
    {
        protected override string SourceDir => Path.Combine("classlib", "framework");

        protected override string SourcePath => Path.Combine("classlib", "Class1.cs");

        protected override string ExpectedOutput => $"Actual:<{NewValue}>";

        protected override IEnumerable<string> CleanPaths => new string[]
        {
            Path.Combine("classlib", "bin"),
            Path.Combine("classlib", "obj"),
            Path.Combine("mstest", "bin"),
            Path.Combine("mstest", "obj"),
            "packages",
            "TestResults",
        };

        protected override string Run(bool first = false)
        {
            Build(first);
            return VsTestConsole(Path.Combine("mstest", "bin", "Debug", "mstest.dll"), throwOnError: false);
        }
    }
}
