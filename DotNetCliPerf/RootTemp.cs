using BenchmarkDotNet.Attributes;
using Common;

namespace DotNetCliPerf
{
    public abstract class RootTemp
    {
        protected string RootTempDir { get; private set; }

        public virtual void GlobalSetup()
        {
            RootTempDir = Util.GetTempDir();
        }

        [GlobalCleanup]
        public virtual void GlobalCleanup()
        {
            Util.DeleteDir(RootTempDir);
        }
    }
}
