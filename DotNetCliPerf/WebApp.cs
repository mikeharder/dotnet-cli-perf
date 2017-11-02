using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.IO;

namespace DotNetCliPerf
{
    public class WebApp : RootTemp
    {
        private static readonly string sourceDir = Path.Combine(Util.RepoRoot, "scenarios", "web", "core");

        private string _oldTitle = "Home Page";
        private string _newTitle;
        private string _output;

        [GlobalSetup]
        public override void GlobalSetup()
        {
            base.GlobalSetup();

            // Copy app from scenarios folder
            Util.DirectoryCopy(sourceDir, RootTempDir, copySubDirs: true);

            Npm("install");

            var output = DotNet("run");

            // Verify response
            var expected = "<title>Home Page";
            if (!output.Contains(expected))
            {
                throw new InvalidOperationException($"Response missing '{expected}'");
            }
        }

        [IterationSetup(Target = nameof(IncrementalControllerChanged))]
        public void IterationSetupIncrementalControllerChanged()
        {
            _newTitle = Guid.NewGuid().ToString();
            Util.ReplaceInFile(Path.Combine(RootTempDir, "Controllers", "HomeController.cs"), _oldTitle, _newTitle);
        }

        [Benchmark]
        public void IncrementalControllerChanged()
        {
            _output = DotNet("run");
        }

        [Benchmark]
        public void IncrementalNoChange()
        {
            _output = DotNet("run");
        }

        [IterationCleanup]
        public void IterationCleanup()
        {
            // Verify new title
            var expected = $"<title>{_newTitle}";
            if (!_output.Contains(expected))
            {
                throw new InvalidOperationException($"Response missing '{expected}'");
            }

            _oldTitle = _newTitle;
        }

        private string DotNet(string arguments)
        {
            return Util.RunProcess("dotnet", arguments, RootTempDir);
        }

        private void Npm(string arguments)
        {
            Util.RunProcess("cmd", $"/c \"npm.cmd {arguments}\"", RootTempDir);
        }
    }
}
