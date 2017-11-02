using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.IO;

namespace DotNetCliPerf
{
    public class WebAppCore : RootTemp
    {
        private static readonly string sourceDir = Path.Combine(Util.RepoRoot, "scenarios", "web", "core");

        private string _oldTitle = "Home Page";
        private string _newTitle;
        private string _output;

        private const string _globalJson = @"{ ""sdk"": { ""version"": ""0.0.0"" } }";

        [Params("2.0.2", "2.1.1")]
        public string SdkVersion { get; set; }

        [GlobalSetup]
        public override void GlobalSetup()
        {
            base.GlobalSetup();

            // Copy app from scenarios folder
            Util.DirectoryCopy(sourceDir, RootTempDir, copySubDirs: true);

            File.WriteAllText(Path.Combine(RootTempDir, "global.json"), _globalJson.Replace("0.0.0", SdkVersion));

            // Verify version
            var output = DotNet("--info");
            if (!output.Contains($"Version:            {SdkVersion}"))
            {
                throw new InvalidOperationException($"Incorrect SDK version");
            }

            output = DotNet("run");

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
