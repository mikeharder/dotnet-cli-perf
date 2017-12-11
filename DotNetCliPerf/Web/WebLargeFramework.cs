using System.Collections.Generic;
using System.IO;

namespace DotNetCliPerf.Web
{
    public class WebLargeFramework : WebFramework
    {
        // ClassLib007 is transitively referenced the most times
        private const string _rootProject = "ClassLib007";

        protected override string SourceDir => Path.Combine("web", "large", "core");

        protected override string SourcePath =>
            (SourceChanged == SourceChanged.Leaf) ?
            base.SourcePath :
            Path.Combine(RootTempDir, _rootProject, $"Class001.cs");

        protected override string ExpectedOutput => (SourceChanged == SourceChanged.Leaf) ? base.ExpectedOutput : NewValue;

        public override void GlobalSetup()
        {
            if (SourceChanged == SourceChanged.Root)
            {
                OldValue = _rootProject;
            }

            base.GlobalSetup();
        }

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
