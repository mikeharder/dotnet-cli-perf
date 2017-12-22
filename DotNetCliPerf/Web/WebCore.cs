using Common;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace DotNetCliPerf
{
    public abstract class WebCore : CoreApp
    {
        private (Process Process, StringBuilder OutputBuilder, StringBuilder ErrorBuilder) _process;

        protected virtual string WebAppDir => "mvc";

        protected override string SourcePath => Path.Combine(RootTempDir, "mvc", "Controllers", "HomeController.cs");

        protected override string ExpectedOutput => $"<title>{NewValue}";

        protected override IEnumerable<string> CleanPaths => new string[]
        {
            Path.Combine("mvc", "bin"),
            Path.Combine("mvc", "obj"),
        };

        protected override string Run(bool first = false)
        {
            if (MSBuildVersion == MSBuildVersion.Desktop)
            {
                Build(first);
                _process = Run(restore: false, build: false);
            }
            else
            {
                _process = Run(restore: first || Restore);
            }

            while (true)
            {
                try
                {
                    return HttpClient.GetStringAsync("http://localhost:5000").Result;
                }
                catch
                {
                    Thread.Sleep(SleepBetweenHttpRequests);
                }
            }
        }

        private (Process Process, StringBuilder OutputBuilder, StringBuilder ErrorBuilder) Run(bool restore, bool build = true)
        {
            return StartDotNet(
                "run",
                restore: restore,
                build: build,
                workingSubDirectory: WebAppDir);
        }

        protected override void RunCleanup()
        {
            base.RunCleanup();
            Util.StopProcess(_process.Process, _process.OutputBuilder, _process.ErrorBuilder, throwOnError: false);
        }
    }
}
