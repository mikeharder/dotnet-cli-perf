using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;

namespace DotNetCliPerf
{
    public abstract class App: RootTemp
    {
        protected readonly Dictionary<string, string> Environment = new Dictionary<string, string>();

        protected string OldValue { get; set; } = "InitialValue";
        protected string NewValue { get; set; }
        protected string Output { get; set; }

        protected abstract string SourceDir { get; }
        protected abstract string SourcePath { get; }
        protected abstract string ExpectedOutput { get; }

        protected abstract void Build();
        protected abstract string Run();

        [GlobalSetup]
        public override void GlobalSetup()
        {
            base.GlobalSetup();
            CopyApp();
            ChangeSource();
            Output = Run();
            VerifyOutput();
        }

        protected virtual void CopyApp()
        {
            Util.DirectoryCopy(SourceDir, RootTempDir, copySubDirs: true);
        }

        protected void ChangeSource()
        {
            NewValue = Guid.NewGuid().ToString();
            Util.ReplaceInFile(SourcePath, OldValue, NewValue);
        }

        protected void VerifyOutput()
        {
            if (!Output.Contains(ExpectedOutput))
            {
                throw new InvalidOperationException($"Response missing '{ExpectedOutput}'");
            }

            OldValue = NewValue;
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
    }
}
