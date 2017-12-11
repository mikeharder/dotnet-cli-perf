using System.IO;

namespace DotNetCliPerf.Web
{
    public class WebSmallFramework : WebFramework
    {
        protected override string SourceDir => Path.Combine("web", "small", "framework");
    }
}
