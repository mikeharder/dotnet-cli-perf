using System.Collections.Generic;
using System.IO;

namespace DotNetCliPerf
{
    public class ClassLibGradle : GradleApp
    {
        protected override string SourceDir => Path.Combine("classlib", "gradle");

        protected override string SourcePath => Path.Combine("src", "main", "java", "Library.java");

        protected override string ExpectedOutput => $"was:<[{NewValue}]>";

        protected override string Run()
        {
            return GradleW("build", throwOnError: false);
        }
    }
}
