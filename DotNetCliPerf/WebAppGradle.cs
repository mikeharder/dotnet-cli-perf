using System.Collections.Generic;
using System.IO;

namespace DotNetCliPerf
{
    public class WebAppGradle : GradleApp
    {
        protected override string SourceDir => Path.Combine("web", "gradle");

        protected override string SourcePath => Path.Combine("src", "main", "java", "hello", "HomeController.java");

        protected override string ExpectedOutput => $"<title>{NewValue}";

        protected override string Run()
        {
            return GradleW("bootRun");
        }
    }
}
