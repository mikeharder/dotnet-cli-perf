using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DotNetCliPerf
{
    public class ClassLibFramework : FrameworkApp
    {
        protected override string SourceDir => Path.Combine("classlib", "framework");

        protected override string SourcePath => Path.Combine("classlib", "Class1.cs");

        protected override string ExpectedOutput => $"Actual:<{NewValue}>";

        protected override string Run(bool first = false)
        {
            Build(first);
            return VsTestConsole(Path.Combine("mstest", "bin", "Debug", "mstest.dll"), throwOnError: false);
        }
    }
}
