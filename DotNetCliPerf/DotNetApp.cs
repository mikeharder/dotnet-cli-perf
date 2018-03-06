using BenchmarkDotNet.Attributes;
using Common;
using System;
using System.IO;

namespace DotNetCliPerf
{
    public abstract class DotNetApp : App
    {
        [Params(true, false)]
        public bool Parallel { get; set; }

        [Params(true, false)]
        public bool NodeReuse { get; set; }

        [Params("NotApplicable", "14.0.25420.1", "15.5.180.51428", "15.6.82.30579")]
        public string MSBuildVersion { get; set; }

        public override void GlobalSetup()
        {
            if (!NodeReuse)
            {
                Environment.Add("MSBUILDDISABLENODEREUSE", "1");
            }

            base.GlobalSetup();
        }

        private string GetMSBuildPath()
        {
            if (MSBuildVersion == "NotApplicable")
            {
                throw new InvalidOperationException($"Parameter 'MSBuildVersion' is set to 'NotApplicable'");
            }

            string msBuildPath;

            if (MSBuildVersion.StartsWith("14", StringComparison.OrdinalIgnoreCase))
            {
                msBuildPath = Path.Combine(
                    System.Environment.GetEnvironmentVariable("ProgramFiles(x86)"),
                    "MSBuild",
                    "14.0",
                    "Bin",
                    "MSBuild.exe");
            }
            else if (MSBuildVersion.StartsWith("15", StringComparison.OrdinalIgnoreCase))
            {
                var vsPath = Path.Combine(
                    System.Environment.GetEnvironmentVariable("ProgramFiles(x86)"),
                    "Microsoft Visual Studio",
                    GetVSVersion(MSBuildVersion));

                var buildToolsPath = Path.Combine(vsPath, "BuildTools");
                if (!Directory.Exists(buildToolsPath))
                {
                    buildToolsPath = Path.Combine(vsPath, "Enterprise");
                }
                if (!Directory.Exists(buildToolsPath))
                {
                    throw new InvalidOperationException($"Could not find MSBuild.exe under {vsPath}");
                }

                msBuildPath = Path.Combine(
                    buildToolsPath,
                    "MSBuild",
                    "15.0",
                    "Bin",
                    "MSBuild.exe");
            }
            else
            {
                throw new InvalidOperationException($"Unknown MSBuild version: {MSBuildVersion}");
            }

            // Verify version
            var output = Util.RunProcess(msBuildPath, "/nologo /version", RootTempDir);
            if (!output.Trim().Equals(MSBuildVersion, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException($"Incorrect MSBuild version: {output}");
            }

            return msBuildPath;
        }

        private static string GetVSVersion(string msBuildVersion)
        {
            if (msBuildVersion.StartsWith("15.5", StringComparison.OrdinalIgnoreCase))
            {
                return "2017";
            }
            else if (msBuildVersion.StartsWith("15.6", StringComparison.OrdinalIgnoreCase))
            {
                return "Preview";
            }
            else
            {
                throw new InvalidOperationException($"Could not find VS version for MSBuild version {msBuildVersion}");
            }
        }

        protected string MSBuild(string arguments, bool restore = false)
        {
            arguments = arguments +
                " /v:minimal" +
                (Parallel ? " /m" : "") +
                (restore ? " /restore" : "");

            return Util.RunProcess(GetMSBuildPath(), arguments, RootTempDir, environment: Environment);
        }
    }
}
