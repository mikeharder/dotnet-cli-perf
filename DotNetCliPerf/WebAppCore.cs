using System.Collections.Generic;
using System.IO;

namespace DotNetCliPerf
{
    public class WebAppCore : CoreApp
    {       
        protected override string SourceDir => Path.Combine("web", "core");

        protected override string SourcePath => Path.Combine(RootTempDir, "mvc", "Controllers", "HomeController.cs");

        protected override string ExpectedOutput => $"<title>{NewValue}";

        protected override IEnumerable<string> CleanPaths => new string[]
        {
            Path.Combine("mvc", "bin"),
            Path.Combine("mvc", "obj"),
        };

        protected override string Run(bool first = false)
        {
            return DotNet("run -- --mode=singleRequest", restore: first || Restore, workingSubDirectory: "mvc", throwOnError: false);
        }
    }
}
