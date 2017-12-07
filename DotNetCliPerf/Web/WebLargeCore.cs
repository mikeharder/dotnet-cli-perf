using System.Collections.Generic;
using System.IO;

namespace DotNetCliPerf
{
    public class WebLargeCore : WebCore
    {
        protected override string SourceDir => Path.Combine("web", "large", "core");

        protected override IEnumerable<string> CleanPaths
        {
            get
            {
                foreach (var path in base.CleanPaths)
                {
                    yield return path;
                }

                for (var i = 1; i <= 126; i++)
                {
                    yield return Path.Combine($"ClassLib{i.ToString("D3")}", "bin");
                    yield return Path.Combine($"ClassLib{i.ToString("D3")}", "obj");
                }
            }
        }
    }
}
