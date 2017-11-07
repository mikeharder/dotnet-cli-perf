using System.IO;

namespace DotNetCliPerf
{
    public class ClassLibCore : CoreApp
    {
        protected override string SourceDir => Path.Combine(Util.RepoRoot, "scenarios", "classlib", "core");

        protected override string SourcePath => Path.Combine(RootTempDir, "classlib", "Class1.cs");

        protected override string ExpectedOutput => $"Actual:<{NewValue}>";

        protected override string Run()
        {
            return DotNet("test mstest", throwOnError: false);
        }
    }
}
