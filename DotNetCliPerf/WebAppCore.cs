using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.IO;

namespace DotNetCliPerf
{
    public class WebAppCore : WebApp
    {
        private static readonly string sourceDir = Path.Combine(Util.RepoRoot, "scenarios", "web", "core");

        private const string _globalJson = @"{ ""sdk"": { ""version"": ""0.0.0"" } }";

        private readonly Dictionary<string, string> _environment = new Dictionary<string, string>();

        [Params("2.0.2", "2.1.1")]
        public string SdkVersion { get; set; }

        // [Params(false, true)]
        public bool TieredJit { get; set; }

        [GlobalSetup]
        public override void GlobalSetup()
        {
            base.GlobalSetup();

            if (TieredJit)
            {
                _environment.Add("COMPLUS_EXPERIMENTAL_TieredCompilation", "1");
            }

            // Copy app from scenarios folder
            Util.DirectoryCopy(sourceDir, RootTempDir, copySubDirs: true);

            File.WriteAllText(Path.Combine(RootTempDir, "global.json"), _globalJson.Replace("0.0.0", SdkVersion));

            // Verify version
            var output = DotNet("--info");
            if (!output.Contains($"Version:            {SdkVersion}"))
            {
                throw new InvalidOperationException($"Incorrect SDK version");
            }

            Output = Run();
            VerifyOutput();
        }

        protected override void Build()
        {
            DotNet("build");
        }

        protected override string Run()
        {
            return DotNet("run");
        }

        protected override void ChangeController()
        {
            NewTitle = Guid.NewGuid().ToString();
            Util.ReplaceInFile(Path.Combine(RootTempDir, "Controllers", "HomeController.cs"), OldTitle, NewTitle);
        }

        private string DotNet(string arguments)
        {
            return Util.RunProcess("dotnet", arguments, RootTempDir, environment: _environment);
        }

        private void Npm(string arguments)
        {
            Util.RunProcess("cmd", $"/c \"npm.cmd {arguments}\"", RootTempDir);
        }
    }
}
