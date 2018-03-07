using System.Collections.Generic;

namespace SolutionGenerator.Solutions
{
    class ManualSolution : ISolution
    {
        public ManualSolution(
            IList<(string Name, IEnumerable<string> ProjectReferences, IEnumerable<(string Name, string Version)> PackageReferences)> projects,
            string mainProject)
        {
            Projects = projects;
            MainProject = mainProject;
        }

        public IList<(string Name, IEnumerable<string> ProjectReferences, IEnumerable<(string Name, string Version)> PackageReferences)> Projects { get; }

        public string MainProject { get; }
    }
}
