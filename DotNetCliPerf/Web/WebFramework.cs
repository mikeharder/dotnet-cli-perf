using Common;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;

namespace DotNetCliPerf
{
    public abstract class WebFramework : FrameworkApp
    {
        private (Process Process, StringBuilder OutputBuilder, StringBuilder ErrorBuilder) _process;

        protected override string SourcePath => Path.Combine(RootTempDir, "mvc", "Controllers", "HomeController.cs");

        protected override string ExpectedOutput => $"<title>{NewValue}";

        protected override IEnumerable<string> CleanPaths => Enumerable.Concat(
            base.CleanPaths,
            new string[]
            {
                Path.Combine("mvc", "bin"),
                Path.Combine("mvc", "obj"),
            });

        protected override string Run(bool first = false)
        {
            Build(first);
            _process = StartIISExpress("mvc");
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

        protected override void RunCleanup()
        {
            base.RunCleanup();
            Util.StopProcess(_process.Process, _process.OutputBuilder, _process.ErrorBuilder, throwOnError: false);
        }

        protected (Process Process, StringBuilder OutputBuilder, StringBuilder ErrorBuilder) StartIISExpress(string relativePath, int port = 5000, bool systray = false)
        {
            var path = Path.Combine(RootTempDir, relativePath);
            return Util.StartProcess("iisexpress", $"/path:{path} /port:{port} /systray:{systray}", RootTempDir);
        }
    }
}
