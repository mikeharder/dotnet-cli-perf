using BenchmarkDotNet.Attributes;
using Common;
using System;
using System.IO;

namespace DotNetCliPerf
{
    public abstract class CoreApp : DotNetApp
    {
        private const string _globalJson = @"{ ""sdk"": { ""version"": ""0.0.0"" } }";

        [Params("2.0.2", "2.1.3-preview-007232", "2.2.0-preview1-007736")]
        public string SdkVersion { get; set; }

        [Params(true, false)]
        public bool Restore { get; set; }

        // [Params(false, true)]
        public bool TieredJit { get; set; }

        [Params(MSBuildVersion.Desktop, MSBuildVersion.Core)]
        public MSBuildVersion MSBuildVersion { get; set; }

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

            if (MSBuildVersion == MSBuildVersion.Desktop)
            {
                // Verify version
                var output = MSBuild("/version");
                if (!output.Contains("15.5."))
                {
                    throw new InvalidOperationException($"Incorrect MSBuild version");
                }
            }
            else
            {
                // Verify version
                var output = DotNet("--info");
                if (!output.Contains($"Version:            {SdkVersion}"))
                {
                    throw new InvalidOperationException($"Incorrect SDK version");
                }
            }
        }

        protected override void Build(bool first = false)
        {
            if (MSBuildVersion == MSBuildVersion.Desktop)
            {
                MSBuild("/t:build", restore: first || Restore);
            }
            else
            {
                DotNet("build", restore: first || Restore);
            }
        }

        protected string DotNet(string dotnetArguments, string appArguments = null, string workingSubDirectory = "", bool restore = true, bool build = true, bool throwOnError = true)
        {
            if (build && NodeReuse)
            {
                throw new InvalidOperationException("Core MSBuild currently does not support NodeReuse");
            }

            var arguments = dotnetArguments +
                (Parallel ? "" : " /m:1") +
                (restore ? "" : " --no-restore") +
                (build ? "" : " --no-build") +
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
