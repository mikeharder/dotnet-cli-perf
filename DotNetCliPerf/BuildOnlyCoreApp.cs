using System;

namespace DotNetCliPerf
{
    public abstract class BuildOnlyCoreApp : CoreApp
    {
        // Building after source changes is currently unsupported, so change a meaningless file
        protected override string SourcePath => "README.md";

        // Do not verify output
        protected override string ExpectedOutput => String.Empty;

        // Running is not supported
        protected override string Run(bool first = false)
        {
            throw new NotImplementedException();
        }
    }
}
