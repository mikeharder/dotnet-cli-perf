namespace DotNetCliPerf
{
    public abstract class FrameworkApp : App
    {
        protected override void Build()
        {
            NuGet("restore");
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
