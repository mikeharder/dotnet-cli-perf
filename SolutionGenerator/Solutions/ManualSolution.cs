using System.Collections.Generic;

namespace ScenarioGenerator.Solutions
{
    class ManualSolution : ISolution
    {
        public ManualSolution(
            IList<(string Name, IEnumerable<string> ProjectReferences, IEnumerable<string> PackageReferences)> projects,
            string mainProject)
        {
            Projects = projects;
            MainProject = mainProject;
        }

        public IList<(string Name, IEnumerable<string> ProjectReferences, IEnumerable<string> PackageReferences)> Projects { get; }

        public string MainProject { get; }
    }
}
