using System.Collections.Generic;
using BenchmarkDotNet.Attributes;

namespace DotNetCliPerf
{
    public abstract class FrameworkApp : DotNetApp
    {
        [Params(true, false)]
        public bool Restore { get; set; }

        protected override IEnumerable<string> CleanPaths => new string[]
        {
            "packages",
        };

        protected override void Build(bool first = false)
        {
            MSBuild("/t:build", restore: first || Restore);
        }

        protected string VsTestConsole(string arguments, bool throwOnError = true)
        {
            return Util.RunProcess("vstest.console.exe", arguments, RootTempDir, throwOnError: throwOnError);
        }
    }
}
