using System.IO;

namespace DotNetCliPerf
{
    public class WebSmallCore : WebCore
    {
        protected override string SourceDir => Path.Combine("web", "small", "core", TargetFramework);
    }
}
