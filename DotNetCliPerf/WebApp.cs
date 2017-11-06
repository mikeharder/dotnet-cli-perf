using BenchmarkDotNet.Attributes;
using System;
using System.IO;

namespace DotNetCliPerf
{
    public abstract class WebApp : RootTemp
    {
        protected string OldTitle { get; set; } = "Home Page";
        protected string NewTitle { get; set; } = "Home Page";
        protected string Output { get; set; }

        protected abstract void Build();
        protected abstract string Run();
        protected abstract void ChangeController();

        [IterationSetup(Target = nameof(BuildIncrementalControllerChanged))]
        public void IterationSetupBuildIncrementalControllerChanged()
        {
            ChangeController();
        }

        [Benchmark]
        public void BuildIncrementalControllerChanged()
        {
            Build();
        }

        [Benchmark]
        public void BuildIncrementalNoChange()
        {
            Build();
        }

        [IterationSetup(Target = nameof(RunIncrementalControllerChanged))]
        public void IterationSetupRunIncrementalControllerChanged()
        {
            ChangeController();
        }

        [Benchmark]
        public void RunIncrementalControllerChanged()
        {
            Output = Run();
        }

        [IterationCleanup(Target = nameof(RunIncrementalControllerChanged))]
        public void IterationCleanupRunIncrementalControllerChanged()
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

        protected void VerifyOutput()
        {
            // Verify new title
            var expected = $"<title>{NewTitle}";
            if (!Output.Contains(expected))
            {
                throw new InvalidOperationException($"Response missing '{expected}'");
            }

            OldTitle = NewTitle;
        }
    }
}
