using System;
using System.IO;

namespace DotNetCliPerf
{
    public class OrchardCore : WebCore
    {
        protected override string SourceDir => "OrchardCore";

        protected override string WebAppDir => Path.Combine("src", "OrchardCore.Cms.Web");

        // Building after source changes is currently unsupported, so change a meaningless file
        protected override string SourcePath => "README.md";

        // Do not verify output
        protected override string ExpectedOutput => String.Empty;
    }
}
