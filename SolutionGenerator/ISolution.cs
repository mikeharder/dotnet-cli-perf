using System.Collections.Generic;

namespace ScenarioGenerator
{
    public interface ISolution
    {
        IList<(string Name, IEnumerable<string> ProjectReferences, IEnumerable<string> PackageReferences)> Projects { get; }
        string MainProject { get; }
    }
}
