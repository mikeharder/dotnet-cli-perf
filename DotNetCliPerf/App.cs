using BenchmarkDotNet.Attributes;
using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;

namespace DotNetCliPerf
{
    public abstract class App: RootTemp
    {
        protected static readonly HttpClient HttpClient = new HttpClient();

        protected readonly Dictionary<string, string> Environment = new Dictionary<string, string>();

        protected string OldValue { get; set; } = "InitialValue";
        protected string NewValue { get; set; }
        protected string Output { get; set; }

        protected abstract string SourceDir { get; }
        protected abstract string SourcePath { get; }
        protected abstract string ExpectedOutput { get; }
        protected abstract IEnumerable<string> CleanPaths { get; }

        protected abstract void Build(bool first = false);
        protected abstract string Run(bool first = false);

        protected virtual void RunCleanup() { }

        [GlobalSetup]
        public override void GlobalSetup()
        {
            base.GlobalSetup();
            CopyApp();
            ChangeSource();
            Output = Run(first: true);
            VerifyOutput();
        }

        protected virtual void CopyApp()
        {
            Util.DirectoryCopy(Path.Combine(Util.RepoRoot, "scenarios", SourceDir), RootTempDir, copySubDirs: true);
        }

        protected virtual void Clean()
        {
            foreach (var path in CleanPaths)
            {
                Util.DeleteDir(Path.Combine(RootTempDir, path));
            }
        }

        protected void ChangeSource()
        {
            NewValue = Guid.NewGuid().ToString();
            Util.ReplaceInFile(Path.Combine(RootTempDir, SourcePath), OldValue, NewValue);
        }

        private void VerifyOutput()
        {
            if (!Output.Contains(ExpectedOutput))
            {
                throw new InvalidOperationException($"Response missing '{ExpectedOutput}'");
            }

            OldValue = NewValue;

            RunCleanup();
        }

        [IterationSetup(Target = nameof(BuildIncrementalSourceChanged))]
        public void IterationSetupBuildIncrementalSourceChanged()
        {
            ChangeSource();
        }

        [Benchmark]
        public void BuildIncrementalSourceChanged()
        {
            Build();
        }

        [Benchmark]
        public void BuildIncrementalNoChange()
        {
            Build();
        }

        [IterationSetup(Target = nameof(BuildFull))]
        public void IterationSetupBuildFull()
        {
            Clean();
        }

        [Benchmark]
        public void BuildFull()
        {
            Build(first: true);
        }

        [IterationSetup(Target = nameof(RunIncrementalSourceChanged))]
        public void IterationSetupRunIncrementalSourceChanged()
        {
            ChangeSource();
        }

        [Benchmark]
        public void RunIncrementalSourceChanged()
        {
            Output = Run();
        }

        [IterationCleanup(Target = nameof(RunIncrementalSourceChanged))]
        public void IterationCleanupRunIncrementalSourceChanged()
        {
            VerifyOutput();
        }

        [Benchmark]
        public void RunIncrementalNoChange()
        {
            Output = Run();
        }

        [IterationCleanup(Target = nameof(RunIncrementalNoChange))]
        public void IterationCleanupRunIncrementalNoChange()
        {
            VerifyOutput();
        }

        [IterationSetup(Target = nameof(RunFull))]
        public void IterationSetupRunFull()
        {
            Clean();
        }

        [Benchmark]
        public void RunFull()
        {
            Output = Run(first: true);
        }

        [IterationCleanup(Target = nameof(RunFull))]
        public void IterationCleanupRunFull()
        {
            VerifyOutput();
        }
    }
}
