using BenchmarkDotNet.Attributes;
using Common;

namespace DotNetCliPerf
{
    public abstract class FrameworkApp : DotNetApp
    {
        [Params(true, false)]
        public bool Restore { get; set; }

        protected override void Build(bool first = false)
        {
            if (first || Restore)
            {
                // For Framework apps, restore via "nuget.exe restore" rather than "msbuild.exe /restore", for 2 reasons:
                // 1. Framework apps often use packages.config rather than PackageReference, which is only supported by nuget.exe
                // 2. Customers are more likely to use "nuget.exe restore" since it has existed for a lot longer
                NuGet("restore");
            }

            MSBuild("/t:build");
        }

        private void NuGet(string arguments)
        {
            Util.RunProcess("nuget", arguments, RootTempDir);
        }

        protected string VsTestConsole(string arguments, bool throwOnError = true)
        {
            return Util.RunProcess("vstest.console.exe", arguments, RootTempDir, throwOnError: throwOnError);
        }
    }
}
