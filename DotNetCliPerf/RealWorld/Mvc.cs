namespace DotNetCliPerf
{
    public class Mvc : BuildOnlyCoreApp
    {
        protected override string SourceDir => "Mvc";

        public override string Solution => "Mvc.sln";
    }
}
