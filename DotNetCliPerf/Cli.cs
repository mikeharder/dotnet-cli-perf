using BenchmarkDotNet.Attributes;

namespace DotNetCliPerf
{
    public abstract class Cli : IterationTemp
    {
        [IterationSetup(Target = nameof(New))]
        public virtual void IterationSetupNew()
        {
            IterationSetupTempDir();
        }

        public abstract void New();

        [IterationSetup(Target = nameof(BuildInitial))]
        public virtual void IterationSetupBuildInitial()
        {
            IterationSetupNew();
            New();
        }

        public virtual void BuildInitial() { }

        [IterationSetup(Target = nameof(BuildNoChanges))]
        public virtual void IterationSetupBuildNoChanges()
        {
            IterationSetupBuildInitial();
            BuildInitial();
        }

        public virtual void BuildNoChanges() { }

        [IterationSetup(Target = nameof(BuildAfterChange))]
        public virtual void IterationSetupBuildAfterChange()
        {
            IterationSetupBuildInitial();
            BuildInitial();
            ModifySource();
        }

        public virtual void BuildAfterChange() { }

        [IterationSetup(Target = nameof(RunNoChanges))]
        public void IterationSetupRunNoChanges()
        {
            IterationSetupBuildInitial();
            BuildInitial();
        }

        public abstract void RunNoChanges();

        [IterationSetup(Target = nameof(RunAfterChange))]
        public void IterationSetupRunAfterChange()
        {
            IterationSetupRunNoChanges();
            RunNoChanges();
            ModifySource();
        }

        public virtual void RunAfterChange() { }

        protected abstract void ModifySource();
    }
}
