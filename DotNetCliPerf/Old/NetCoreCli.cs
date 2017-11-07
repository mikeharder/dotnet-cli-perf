using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.IO;

namespace DotNetCliPerf
{
    class NetCoreCli : Cli
    {
        private IDictionary<string, string> _environment = new Dictionary<string, string>();

        [Params("console", "mvc")]
        public string Template { get; set; }

        [IterationSetup(Target = nameof(New))]
        public override void IterationSetupNew()
        {
            base.IterationSetupNew();
            _environment["NUGET_PACKAGES"] = Path.Combine(IterationTempDir, "nuget-packages");
            _environment["NUGET_HTTP_CACHE_PATH"] = Path.Combine(IterationTempDir, "nuget-http-cache");
        }

        [Benchmark]
        public override void New()
        {
            DotNet($"new {Template} --no-restore");
        }

        [IterationSetup(Target = nameof(RestoreInitial))]
        public void IterationSetupRestoreInitial()
        {
            IterationSetupNew();
            New();
            if (Template == "mvc")
            {
                TerminateAfterWebAppStarted();
            }
        }

        [Benchmark]
        public void RestoreInitial()
        {
            DotNet("restore");
        }

        [IterationSetup(Target = nameof(RestoreNoChanges))]
        public void IterationSetupRestoreNoChanges()
        {
            IterationSetupRestoreInitial();
            RestoreInitial();
        }

        [Benchmark]
        public void RestoreNoChanges()
        {
            DotNet("restore");
        }

        [IterationSetup(Target = nameof(AddPackage))]
        public void IterationSetupAddPackage()
        {
            IterationSetupRestoreInitial();
            RestoreInitial();
        }

        [Benchmark]
        public void AddPackage()
        {
            DotNet("add package NUnit --version 3.8.1 --no-restore");
        }

        [IterationSetup(Target = nameof(RestoreAfterAdd))]
        public void IterationSetupRestoreAfterAdd()
        {
            IterationSetupAddPackage();
            AddPackage();
        }

        [Benchmark]
        public void RestoreAfterAdd()
        {
            DotNet("restore");
        }

        [IterationSetup(Target = nameof(BuildInitial))]
        public override void IterationSetupBuildInitial()
        {
            IterationSetupRestoreInitial();
            RestoreInitial();
        }

        [Benchmark]
        public override void BuildInitial()
        {
            DotNet("build");
        }

        [Benchmark]
        public override void BuildNoChanges()
        {
            DotNet("build");
        }

        [Benchmark]
        public override void BuildAfterChange()
        {
            DotNet("build");
        }

        [Benchmark]
        public override void RunNoChanges()
        {
            DotNet("run");
        }

        [Benchmark]
        public override void RunAfterChange()
        {
            DotNet("run");
        }

        private void DotNet(string arguments)
        {
            Util.RunProcess("dotnet", arguments, IterationTempDir, environment: _environment);
        }

        protected override void ModifySource()
        {
            var replacements = new Dictionary<string, Tuple<string, string, string>>
            {
                { "mvc", Tuple.Create("Startup.cs", "default", "default2") },
                { "console", Tuple.Create("Program.cs", "Hello", "Hello2") },
            };

            var path = Path.Combine(IterationTempDir, replacements[Template].Item1);
            var oldValue = replacements[Template].Item2;
            var newValue = replacements[Template].Item3;

            File.WriteAllText(path, File.ReadAllText(path).Replace(oldValue, newValue));
        }

        private void TerminateAfterWebAppStarted()
        {
            var path = Path.Combine(IterationTempDir, "Program.cs");
            File.WriteAllText(path, File.ReadAllText(path).Replace("Run()", "RunAsync()"));
        }
    }
}
