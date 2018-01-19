using BenchmarkDotNet.Attributes;
using Common;
using System;
using System.Collections.Generic;
using System.IO;

namespace DotNetCliPerf
{
    public abstract class DotNetApp : App
    {
        private static readonly Dictionary<string, string> _vsVersions = new Dictionary<string, string>
        {
            { "15.5.180.51428", "2017" },
            { "15.6.13.61168", "PubPreview" },
            { "15.6.54.9755", "Preview" },
        };


        [Params(true, false)]
        public bool Parallel { get; set; }

        [Params(true, false)]
        public bool NodeReuse { get; set; }

        [Params("NotApplicable", "15.5.180.51428", "15.6.13.61168", "15.6.54.9755")]
        public string MSBuildVersion { get; set; }

        private string GetMSBuildPath()
        {
            if (MSBuildVersion == "NotApplicable")
            {
                throw new InvalidOperationException($"Parameter 'MSBuildVersion' is set to 'NotApplicable'");
            }

            var vsPath = Path.Combine(
                System.Environment.GetEnvironmentVariable("ProgramFiles(x86)"),
                "Microsoft Visual Studio",
                _vsVersions[MSBuildVersion]);

            var buildToolsPath = Path.Combine(vsPath, "BuildTools");
            if (!Directory.Exists(buildToolsPath))
            {
                buildToolsPath = Path.Combine(vsPath, "Enterprise");
            }
            if (!Directory.Exists(buildToolsPath))
            {
                throw new InvalidOperationException($"Could not find MSBuild.exe under {vsPath}");
            }

            var msBuildPath = Path.Combine(
                buildToolsPath,
                "MSBuild",
                "15.0",
                "Bin",
                "MSBuild.exe");

            // Verify version
            var output = Util.RunProcess(msBuildPath, "/nologo /version", RootTempDir);
            if (!output.Trim().Equals(MSBuildVersion, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException($"Incorrect MSBuild version: {output}");
            }

            return msBuildPath;
        }

        protected string MSBuild(string arguments, bool restore = false)
        {
            arguments = arguments +
                " /v:minimal" +
                (Parallel ? " /m" : "") +
                $" /nr:{NodeReuse}" +
                (restore ? " /restore" : "");

            return Util.RunProcess(GetMSBuildPath(), arguments, RootTempDir);
        }
    }
}
