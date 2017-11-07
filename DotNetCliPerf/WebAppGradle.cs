using System.IO;

namespace DotNetCliPerf
{
    public class WebAppGradle : GradleApp
    {
        protected override string SourceDir => Path.Combine(Util.RepoRoot, "scenarios", "web", "gradle");

        protected override string SourcePath => Path.Combine(RootTempDir, "src", "main", "java", "hello", "HomeController.java");

        protected override string ExpectedOutput => $"<title>{NewValue}";

        protected override string Run()
        {
            return GradleW("bootRun");
        }
    }
}
