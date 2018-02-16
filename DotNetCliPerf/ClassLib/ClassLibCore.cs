﻿using System.IO;

namespace DotNetCliPerf
{
    public class ClassLibCore : CoreApp
    {
        protected override string SourceDir => Path.Combine("classlib", "core");

        protected override string SourcePath => Path.Combine("classlib", "Class1.cs");

        protected override string ExpectedOutput => $"Actual:<{NewValue}>";

        protected override string Run(bool first = false)
        {
            if (MSBuildFlavor == MSBuildFlavor.Framework)
            {
                Build(first);
                return Run(restore: false, build: false);
            }
            else
            {
                return Run(restore: first || Restore, build: first || (NoBuild != true));
            }
        }

        private string Run(bool restore, bool build)
        {
            return DotNet("test mstest", restore: restore, build: build, throwOnError: false);
        }

    }
}
