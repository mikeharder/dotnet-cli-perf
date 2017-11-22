namespace DotNetCliPerf
{
    public abstract class DotNetApp : App
    {
        protected string MSBuild(string arguments, bool restore = false)
        {
            arguments = "/m " + arguments + (restore ? " /restore" : "");
            return Util.RunProcess("msbuild", arguments, RootTempDir);
        }
    }
}
