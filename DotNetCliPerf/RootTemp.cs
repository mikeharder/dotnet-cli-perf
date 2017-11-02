using BenchmarkDotNet.Attributes;

namespace DotNetCliPerf
{
    public abstract class RootTemp
    {
        protected string RootTempDir { get; private set; }

        [GlobalSetup]
        public virtual void GlobalSetup()
        {
            RootTempDir = Util.GetTempDir();
        }

        [GlobalCleanup]
        public void GlobalCleanup()
        {
            Util.DeleteDir(RootTempDir);
        }
    }
}
