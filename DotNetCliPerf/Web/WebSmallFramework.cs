using System.IO;

namespace DotNetCliPerf
{
    public class WebSmallFramework : WebFramework
    {
        protected override string SourceDir => Path.Combine("web", "small", "framework");
    }
}
