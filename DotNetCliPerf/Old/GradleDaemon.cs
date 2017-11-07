using BenchmarkDotNet.Attributes;
using System.IO;

namespace DotNetCliPerf
{
    class GradleDaemon : Cli
    {
        [GlobalSetup]
        public override void GlobalSetup()
        {
            base.GlobalSetup();
            
            // Ensure gradle daemon is running
            Gradle("--daemon");
        }

        [Benchmark]
        public override void New()
        {
            Gradle($"init --type java-application");
        }

        [Benchmark]
        public override void BuildInitial()
        {
            GradleW("assemble");
        }

        [Benchmark]
        public override void BuildNoChanges()
        {
            GradleW("assemble");
        }

        [Benchmark]
        public override void BuildAfterChange()
        {
            GradleW("assemble");
        }

        [Benchmark]
        public override void RunNoChanges()
        {
            GradleW("run");
        }

        [Benchmark]
        public override void RunAfterChange()
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

        protected override void ModifySource()
        {
            var path = Path.Combine(IterationTempDir, "src", "main", "java", "App.java");
            var oldValue = "Hello";
            var newValue = "Hello2";

            File.WriteAllText(path, File.ReadAllText(path).Replace(oldValue, newValue));
        }
    }
}
