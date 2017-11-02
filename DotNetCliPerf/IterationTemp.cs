using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCliPerf
{
    public abstract class IterationTemp : RootTemp
    {
        protected string IterationTempDir { get; private set; }

        protected void IterationSetupTempDir()
        {
            IterationTempDir = Util.GetTempDir(RootTempDir);
        }
    }
}
