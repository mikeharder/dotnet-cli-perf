using System.Collections.Generic;
using System.IO;

namespace DotNetCliPerf
{
    public class WebAppCore : CoreApp
    {       
        protected override string SourceDir => Path.Combine("web", "core");

        protected override string SourcePath => Path.Combine(RootTempDir, "Controllers", "HomeController.cs");

        protected override string ExpectedOutput => $"<title>{NewValue}";

        protected override IEnumerable<string> CleanPaths => new string[]
        {
            "bin",
            "obj",
        };
    }
}
