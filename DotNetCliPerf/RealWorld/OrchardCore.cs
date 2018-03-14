using System;
using System.IO;

namespace DotNetCliPerf
{
    public class OrchardCore : WebCore, ISourceChanged
    {
        private static readonly string _leafSourcePath = Path.Combine("src", "OrchardCore.Cms.Web", "Program.cs");

        // OrchardCore.Environment.Extensions.Abstractions is transitively referenced the most times
        private static readonly string _rootSourcePath =
            Path.Combine("src", "OrchardCore", "OrchardCore.Environment.Extensions.Abstractions", "Utility", "DependencyOrderingUtility.cs");

        protected override string SourceDir => "OrchardCore";

        protected override string WebAppDir => Path.Combine("src", "OrchardCore.Cms.Web");

        protected override string SourcePath => (SourceChanged == SourceChanged.Leaf) ? _leafSourcePath : _rootSourcePath;

        // Do not verify output
        protected override string ExpectedOutput => String.Empty;

        public override void GlobalSetup()
        {
            if (SourceChanged == SourceChanged.Root)
            {
                // DependencyOrderingUtility.cs, Line 80
                // throw new ArgumentException("lowerIndex");
                OldValue = "lowerIndex";
            }

            base.GlobalSetup();
        }

        protected override void CopyApp()
        {
            base.CopyApp();

            if (SourceChanged == SourceChanged.Leaf)
            {
                // Add dummy class with a string that can be changed to trigger a rebuild
                File.AppendAllText(Path.Combine(RootTempDir, SourcePath), "public class Class001 { public static string Property => \"InitialValue\"; }");
            }
        }
    }
}
