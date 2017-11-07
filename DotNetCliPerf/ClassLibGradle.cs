using System.IO;

namespace DotNetCliPerf
{
    public class ClassLibGradle : GradleApp
    {
        protected override string SourceDir => Path.Combine(Util.RepoRoot, "scenarios", "classlib", "gradle");

        protected override string SourcePath => Path.Combine(RootTempDir, "src", "main", "java", "Library.java");

        protected override string ExpectedOutput => $"was:<[{NewValue}]>";

        protected override string Run()
        {
            return GradleW("build", throwOnError: false);
        }
    }
}
