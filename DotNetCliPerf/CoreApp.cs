using BenchmarkDotNet.Attributes;
using Common;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace DotNetCliPerf
{
    public abstract class CoreApp : DotNetApp
    {
        private const string _globalJson = @"{ ""sdk"": { ""version"": ""0.0.0"" } }";

        [Params("2.1.400", "2.1.401", "2.1.402", "2.2.100-preview1")]
        public string SdkVersion { get; set; }

        [Params(true, false)]
        public bool Restore { get; set; }

        [Params(false, true)]
        public bool TieredJit { get; set; }

        [Params(MSBuildFlavor.Framework, MSBuildFlavor.Core)]
        public MSBuildFlavor MSBuildFlavor { get; set; }

        [Params("2.0", "2.1")]
        public string TargetFramework { get; set; }

        public override void GlobalSetup()
        {
            // The environment of persistent build processes may be modified, so they should be killed in both
            // setup and cleanup to ensure consistent results both inside and outside the benchmarking app.
            KillPersistentBuildProcesses();

            if (TieredJit)
            {
                Environment.Add("COMPLUS_EXPERIMENTAL_TieredCompilation", "1");
            }

            base.GlobalSetup();
        }

        public override void GlobalCleanup()
        {
            // The environment of persistent build processes may be modified, so they should be killed in both
            // setup and cleanup to ensure consistent results both inside and outside the benchmarking app.
            KillPersistentBuildProcesses();

            base.GlobalCleanup();
        }

        private void KillPersistentBuildProcesses()
        {
            Util.KillAllDotnet("MSBuild.dll");
            Util.KillAllDotnet("VBCSCompiler.dll");
        }

        protected override void CopyApp()
        {
            base.CopyApp();

            File.WriteAllText(Path.Combine(RootTempDir, "global.json"), _globalJson.Replace("0.0.0", SdkVersion));

            if (MSBuildFlavor == MSBuildFlavor.Core)
            {
                // Verify version
                var output = DotNet("--info");
                if (!Regex.IsMatch(output, $@"Version:\W*{SdkVersion}"))
                {
                    throw new InvalidOperationException($"Incorrect SDK version");
                }
            }
        }

        protected override void Build(bool first = false)
        {
            if (first || NoBuild != true)
            {
                if (MSBuildFlavor == MSBuildFlavor.Framework)
                {
                    MSBuild($"/t:build {Solution}", restore: first || Restore);
                }
                else
                {
                    DotNet($"build {Solution}", restore: first || Restore);
                }
            }
        }

        protected string DotNet(
            string dotnetArguments,
            string appArguments = null,
            string workingSubDirectory = "",
            bool restore = true,
            bool build = true,
            bool throwOnError = true)
        {
            var p = StartDotNet(dotnetArguments, appArguments, workingSubDirectory, restore, build);
            return Util.WaitForExit(p.Process, p.OutputBuilder, p.ErrorBuilder, throwOnError: throwOnError);
        }

        protected (Process Process, StringBuilder OutputBuilder, StringBuilder ErrorBuilder) StartDotNet(
            string dotnetArguments,
            string appArguments = null,
            string workingSubDirectory = "",
            bool restore = true,
            bool build = true)
        {
            var arguments = dotnetArguments +
                (Parallel ? "" : " /m:1") +
                (ProduceReferenceAssembly.HasValue ? 
                    $" /p:ProduceReferenceAssembly={ProduceReferenceAssembly.ToString().ToLower()}" : "") +
                (RestoreUseSkipNonexistentTargets.HasValue ?
                    $" /p:RestoreUseSkipNonexistentTargets={RestoreUseSkipNonexistentTargets.ToString().ToLower()}" : "") +
                (restore ? "" : " --no-restore") +
                (build ? "" : " --no-build") +
                (appArguments == null ? "" : " -- " + appArguments);

            return Util.StartProcess(
                "dotnet",
                arguments,
                Path.Combine(RootTempDir, workingSubDirectory),
                environment: Environment
            );
        }

    }
}
