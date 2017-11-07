using System.Collections.Generic;
using System.IO;

namespace DotNetCliPerf
{
    public class ClassLibCore : CoreApp
    {
        protected override string SourceDir => Path.Combine("classlib", "core");

        protected override string SourcePath => Path.Combine("classlib", "Class1.cs");

        protected override string ExpectedOutput => $"Actual:<{NewValue}>";

        protected override IEnumerable<string> CleanPaths => new string[]
        {
            Path.Combine("classlib", "bin"),
            Path.Combine("classlib", "obj"),
            Path.Combine("mstest", "bin"),
            Path.Combine("mstest", "obj"),
        };

        protected override string Run()
        {
            return DotNet("test mstest", throwOnError: false);
        }
    }
}
