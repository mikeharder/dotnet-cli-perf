using BenchmarkDotNet.Attributes;
using System.IO;

namespace DotNetCliPerf
{
    public class GradleDaemon : TempDir
    {
        [GlobalSetup]
        public new void GlobalSetup()
        {
            base.GlobalSetup();
            
            // Ensure gradle daemon is running
            Gradle("--daemon");
        }

        [IterationSetup(Target = nameof(New))]
        public void IterationSetupNew()
        {
            IterationSetup();
        }

        [Benchmark]
        public void New()
        {
            Gradle($"init --type java-application");
        }

        [IterationSetup(Target = nameof(BuildInitial))]
        public void IterationSetupBuildInitial()
        {
            IterationSetupNew();
            New();
        }

        [Benchmark]
        public void BuildInitial()
        {
            GradleW("assemble");
        }

        [IterationSetup(Target = nameof(BuildNoChanges))]
        public void IterationSetupBuildNoChanges()
        {
            IterationSetupBuildInitial();
            BuildInitial();
        }

        [Benchmark]
        public void BuildNoChanges()
        {
            GradleW("assemble");
        }

        [IterationSetup(Target = nameof(BuildAfterChange))]
        public void IterationSetupBuildAfterChange()
        {
            IterationSetupBuildInitial();
            BuildInitial();
            ModifySource();
        }

        [Benchmark]
        public void BuildAfterChange()
        {
            GradleW("assemble");
        }

        [IterationSetup(Target = nameof(RunNoChanges))]
        public void IterationSetupRunNoChanges()
        {
            IterationSetupBuildInitial();
            BuildInitial();
        }

        [Benchmark]
        public void RunNoChanges()
        {
            GradleW("run");
        }

        [IterationSetup(Target = nameof(RunAfterChange))]
        public void IterationSetupRunAfterChange()
        {
            IterationSetupRunNoChanges();
            RunNoChanges();
            ModifySource();
        }

        [Benchmark]
        public void RunAfterChange()
        {
            GradleW("run");
        }

        private void Gradle(string arguments)
        {
            Util.RunProcess("gradle", arguments, IterationTempDir);
        }

        private void GradleW(string arguments)
        {
            Util.RunProcess("cmd", $"/c \"gradlew.bat {arguments}\"", IterationTempDir);
        }

        private void ModifySource()
        {
            var path = Path.Combine(IterationTempDir, "src", "main", "java", "App.java");
            var oldValue = "Hello";
            var newValue = "Hello2";

            File.WriteAllText(path, File.ReadAllText(path).Replace(oldValue, newValue));
        }
    }
}
