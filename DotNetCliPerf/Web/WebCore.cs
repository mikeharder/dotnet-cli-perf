using Common;
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

        protected override string SourcePath => Path.Combine("mvc", "Controllers", "HomeController.cs");

        protected override string ExpectedOutput => $"<title>{NewValue}";

        protected override string Run(bool first = false)
        {
            if (MSBuildFlavor == MSBuildFlavor.Framework)
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
            var argument = "run";
            if (RazorCompileOnBuild)
            {
                argument += " /p:RazorCompileOnBuild=true /p:UseRazorBuildServer=true";
            }

            return StartDotNet(
                argument,
                restore: restore,
                build: build,
                workingSubDirectory: WebAppDir);
        }

        protected override void RunCleanup()
        {
            base.RunCleanup();

            if (_process.Process != null)
            {
                Util.StopProcess(_process.Process, _process.OutputBuilder, _process.ErrorBuilder, throwOnError: false);
                _process.Process = null;
            }
        }
    }
}
