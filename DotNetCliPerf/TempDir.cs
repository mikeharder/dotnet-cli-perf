using BenchmarkDotNet.Attributes;

namespace DotNetCliPerf
{
    public abstract class TempDir
    {
        protected string RootTempDir { get; private set; }
        protected string IterationTempDir { get; private set; }

        [GlobalSetup]
        public void GlobalSetup()
        {
            RootTempDir = Util.GetTempDir();
        }

        [GlobalCleanup]
        public void GlobalCleanup()
        {
            Util.DeleteDir(RootTempDir);
        }

        protected void IterationSetup()
        {
            IterationTempDir = Util.GetTempDir(RootTempDir);
        }
    }
}
