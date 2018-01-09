namespace DotNetCliPerf
{
    public abstract class WebGradle : GradleApp
    {
        protected override string ExpectedOutput => $"<title>{NewValue}";

        // We still need to use the "singleRequest" model for Gradle, since killing the process also kills the Gradle daemon.
        protected override string Run(bool first = false)
        {
            return GradleW("bootRun -PappArgs=\"['--mode=singleRequest']\"");
        }
    }
}
