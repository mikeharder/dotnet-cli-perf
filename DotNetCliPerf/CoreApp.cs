using BenchmarkDotNet.Attributes;
using System;
using System.IO;

namespace DotNetCliPerf
{
    public abstract class CoreApp : App
    {
        private const string _globalJson = @"{ ""sdk"": { ""version"": ""0.0.0"" } }";

        [Params("2.0.2", "2.1.1-preview-007145", "2.2.0-preview1-007525", "2.2.0-preview1-007556")]
        public string SdkVersion { get; set; }

        [Params(true, false)]
        public bool Restore { get; set; }

        [Params(true, false)]
        public bool Parallel { get; set; }

        // [Params(false, true)]
        public bool TieredJit { get; set; }

        [Params("Desktop", "Core")]
        public bool MSBuildVersion { get; set; }

        [GlobalSetup]
        public override void GlobalSetup()
        {
            if (TieredJit)
            {
                Environment.Add("COMPLUS_EXPERIMENTAL_TieredCompilation", "1");
            }

            base.GlobalSetup();
        }

        protected override void CopyApp()
        {
            base.CopyApp();

            File.WriteAllText(Path.Combine(RootTempDir, "global.json"), _globalJson.Replace("0.0.0", SdkVersion));

            // Verify version
            var output = DotNet("--info");
            if (!output.Contains($"Version:            {SdkVersion}"))
            {
                throw new InvalidOperationException($"Incorrect SDK version");
            }
        }

        protected override void Build(bool first = false)
        {
            DotNet("build", restore: first || Restore);
        }

        protected string DotNet(string dotnetArguments, string appArguments = null, string workingSubDirectory = "", bool restore = true, bool throwOnError = true)
        {
            var arguments = dotnetArguments +
                (Parallel ? "" : " /m:1") +
                (restore ? "" : " --no-restore") +
                (appArguments == null ? "" : " -- " + appArguments);

            return Util.RunProcess(
                "dotnet",
                arguments,
                Path.Combine(RootTempDir, workingSubDirectory),
                throwOnError: throwOnError,
                environment: Environment
            );
        }
    }
}
