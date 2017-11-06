using BenchmarkDotNet.Attributes;
using System;
using System.IO;

namespace DotNetCliPerf
{
    public class WebAppSpring : RootTemp
    {
        private static readonly string sourceDir = Path.Combine(Util.RepoRoot, "scenarios", "web", "spring-boot");

        private string _oldTitle = "Home Page";
        private string _newTitle;
        private string _output;

        [GlobalSetup]
        public override void GlobalSetup()
        {
            base.GlobalSetup();

            // Copy app from scenarios folder
            Util.DirectoryCopy(sourceDir, RootTempDir, copySubDirs: true);

            var output = GradleW("bootRun");

            // Verify response
            var expected = "<title>Home Page";
            if (!output.Contains(expected))
            {
                throw new InvalidOperationException($"Response missing '{expected}'");
            }
        }

        [IterationSetup(Target = nameof(BuildIncrementalControllerChanged))]
        public void IterationSetupBuildIncrementalControllerChanged()
        {
            ChangeController();
        }

        [Benchmark]
        public void BuildIncrementalControllerChanged()
        {
            GradleW("assemble");
        }

        [Benchmark]
        public void BuildIncrementalNoChange()
        {
            GradleW("assemble");
        }

        [IterationSetup(Target = nameof(RunIncrementalControllerChanged))]
        public void IterationSetupRunIncrementalControllerChanged()
        {
            ChangeController();
        }

        [Benchmark]
        public void RunIncrementalControllerChanged()
        {
            _output = GradleW("bootRun");
        }

        [IterationCleanup(Target = nameof(RunIncrementalControllerChanged))]
        public void IterationCleanupRunIncrementalControllerChanged()
        {
            VerifyOutput();
        }

        [Benchmark]
        public void RunIncrementalNoChange()
        {
            _output = GradleW("bootRun");
        }

        [IterationCleanup(Target = nameof(RunIncrementalNoChange))]
        public void IterationCleanupRunIncrementalNoChange()
        {
            VerifyOutput();
        }

        private void ChangeController()
        {
            _newTitle = Guid.NewGuid().ToString();
            Util.ReplaceInFile(Path.Combine(RootTempDir, "src", "main", "java", "hello", "HomeController.java"), _oldTitle, _newTitle);
        }

        private void VerifyOutput()
        {
            // Verify new title
            var expected = $"<title>{_newTitle}";
            if (!_output.Contains(expected))
            {
                throw new InvalidOperationException($"Response missing '{expected}'");
            }

            _oldTitle = _newTitle;
        }

        private string GradleW(string arguments)
        {
            return Util.RunProcess("cmd", $"/c \"gradlew.bat {arguments}\"", RootTempDir);
        }
    }
}
