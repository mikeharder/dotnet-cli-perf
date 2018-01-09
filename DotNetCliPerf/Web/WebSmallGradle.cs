using System.IO;

namespace DotNetCliPerf
{
    public class WebSmallGradle : GradleApp
    {
        protected override string SourceDir => Path.Combine("web", "small", "gradle");

        protected override string SourcePath => Path.Combine("src", "main", "java", "hello", "HomeController.java");

        protected override string ExpectedOutput => $"<title>{NewValue}";

        // We still need to use the "singleRequest" model for Gradle, since killing the process also kills the Gradle daemon.
        protected override string Run(bool first = false)
        {
            return GradleW("bootRun -PappArgs=\"['--mode=singleRequest']\"");
        }
    }
}
