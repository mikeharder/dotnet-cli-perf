using System.IO;

namespace DotNetCliPerf
{
    public class WebAppCore : CoreApp
    {       
        protected override string SourceDir => Path.Combine(Util.RepoRoot, "scenarios", "web", "core");

        protected override string SourcePath => Path.Combine(RootTempDir, "Controllers", "HomeController.cs");

        protected override string ExpectedOutput => $"<title>{NewValue}";
    }
}
