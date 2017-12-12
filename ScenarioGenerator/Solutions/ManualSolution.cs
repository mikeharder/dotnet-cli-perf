using System.Collections.Generic;

namespace ScenarioGenerator.Solutions
{
    class ManualSolution : ISolution
    {
        public ManualSolution(IList<(string Name, IEnumerable<string> ProjectReferences)> projects, string mainProject)
        {
            Projects = projects;
            MainProject = mainProject;
        }

        public IList<(string Name, IEnumerable<string> ProjectReferences)> Projects { get; }

        public string MainProject { get; }
    }
}
