using Common;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace DotNetCliPerf
{
    public abstract class GradleApp : App
    {
        protected override void Build(bool first=false)
        {
            GradleW("assemble --parallel");
        }

        protected string GradleW(string arguments, bool throwOnError = true)
        {
            string filename;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                filename = "cmd";
                arguments = $"/c \"gradlew.bat {arguments}\"";
            }
            else
            {
                filename = "sh";
                arguments = $"gradlew {arguments}";
            }

            return Util.RunProcess(filename, arguments, RootTempDir, throwOnError: throwOnError);
        }
    }
}
