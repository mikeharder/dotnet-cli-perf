using System.IO;

namespace DotNetCliPerf
{
    public class WebSmallGradle : WebGradle
    {
        protected override string SourceDir => Path.Combine("web", "small", "gradle");

        protected override string SourcePath => Path.Combine("src", "main", "java", "hello", "HomeController.java");
    }
}
