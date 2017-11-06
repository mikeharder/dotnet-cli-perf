using BenchmarkDotNet.Attributes;
using System;
using System.IO;

namespace DotNetCliPerf
{
    public class WebAppSpring : WebApp
    {
        private static readonly string sourceDir = Path.Combine(Util.RepoRoot, "scenarios", "web", "spring-boot");

        [GlobalSetup]
        public override void GlobalSetup()
        {
            base.GlobalSetup();

            // Copy app from scenarios folder
            Util.DirectoryCopy(sourceDir, RootTempDir, copySubDirs: true);

            Output = Run();
            VerifyOutput();
        }

        protected override void Build()
        {
            GradleW("assemble");
        }

        protected override string Run()
        {
            return GradleW("bootRun");
        }

        protected override void ChangeController()
        {
            NewTitle = Guid.NewGuid().ToString();
            Util.ReplaceInFile(Path.Combine(RootTempDir, "src", "main", "java", "hello", "HomeController.java"), OldTitle, NewTitle);
        }

        private string GradleW(string arguments)
        {
            return Util.RunProcess("cmd", $"/c \"gradlew.bat {arguments}\"", RootTempDir);
        }
    }
}
