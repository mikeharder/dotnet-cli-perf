using System.Collections.Generic;
using BenchmarkDotNet.Attributes;

namespace DotNetCliPerf
{
    public abstract class FrameworkApp : App
    {
        [Params(true, false)]
        public bool Restore { get; set; }

        protected override IEnumerable<string> CleanPaths => new string[]
        {
            "packages",
        };

        protected override void Build(bool first = false)
        {
            var restore = first || Restore;

            if (restore)
            {
                NuGet("restore");
            }

            MSBuild("/t:build");
        }

        protected string VsTestConsole(string arguments, bool throwOnError = true)
        {
            return Util.RunProcess("vstest.console.exe", arguments, RootTempDir, throwOnError: throwOnError);
        }

        protected void NuGet(string arguments)
        {
            Util.RunProcess("nuget", arguments, RootTempDir);
        }

        protected void MSBuild(string arguments)
        {
            Util.RunProcess("msbuild", $"/m {arguments}", RootTempDir);
        }
    }
}
