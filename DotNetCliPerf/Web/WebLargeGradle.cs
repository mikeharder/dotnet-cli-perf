using System.IO;

namespace DotNetCliPerf
{
    public class WebLargeGradle : WebGradle
    {
        // ClassLib007 is transitively referenced the most times
        private const string _rootProject = "ClassLib007";

        protected override string SourceDir => Path.Combine("web", "large", "gradle");

        protected override string SourcePath =>
            (SourceChanged == SourceChanged.Leaf) ?
            Path.Combine("mvc", "src", "main", "java", "hello", "HomeController.java") :
            Path.Combine(_rootProject, "src", "main", "java", _rootProject, "Class001.java");

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
