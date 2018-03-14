using System.IO;

namespace DotNetCliPerf
{
    public class WebLargeCore : WebCore, ISourceChanged
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
    }
}
