using BenchmarkDotNet.Attributes;

namespace DotNetCliPerf
{
    public class NetCoreCli
    {
        private string _rootTempDir;
        private string _iterationTempDir;

        [GlobalSetup]
        public void GlobalSetup()
        {
            _rootTempDir = Util.GetTempDir();
        }

        [GlobalCleanup]
        public void GlobalCleanup()
        {
            Util.DeleteDir(_rootTempDir);
        }

        [IterationSetup(Target = nameof(NewMvc))]
        public void IterationSetupNewMvc()
        {
            _iterationTempDir = Util.GetTempDir(_rootTempDir);
        }

        [Benchmark]
        public void NewMvc()
        {
            DotNet("new mvc --no-restore");
        }

        [IterationSetup(Target = nameof(RestoreInitial))]
        public void IterationSetupRestoreInitial()
        {
            IterationSetupNewMvc();
            NewMvc();
        }

        [Benchmark]
        public void RestoreInitial()
        {
            DotNet("restore");
        }

        private void DotNet(string arguments)
        {
            Util.RunProcess("dotnet", arguments, _iterationTempDir);
        }
    }
}
