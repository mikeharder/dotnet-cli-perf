using BenchmarkDotNet.Attributes;

namespace DotNetCliPerf
{
    public abstract class DotNetApp : App
    {
        [Params(true, false)]
        public bool Parallel { get; set; }

        [Params(true, false)]
        public bool NodeReuse { get; set; }

        protected string MSBuild(string arguments, bool restore = false)
        {
            arguments = arguments +
                (Parallel ? " /m" : "") +
                $" /nr:{NodeReuse}" +
                (restore ? " /restore" : "");
            return Util.RunProcess("msbuild", arguments, RootTempDir);
        }
    }
}
