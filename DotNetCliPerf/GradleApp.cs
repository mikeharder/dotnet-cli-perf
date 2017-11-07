using BenchmarkDotNet.Attributes;

namespace DotNetCliPerf
{
    public abstract class GradleApp : App
    {
        protected override void Build()
        {
            GradleW("assemble");
        }

        protected string GradleW(string arguments, bool throwOnError = true)
        {
            return Util.RunProcess("cmd", $"/c \"gradlew.bat {arguments}\"", RootTempDir, throwOnError: throwOnError);
        }
    }
}
