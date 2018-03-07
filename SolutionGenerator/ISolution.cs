using System.Collections.Generic;

namespace SolutionGenerator
{
    public interface ISolution
    {
        IList<(string Name, IEnumerable<string> ProjectReferences, IEnumerable<(string Name, string Version)> PackageReferences)> Projects { get; }
        string MainProject { get; }
    }
}
