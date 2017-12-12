using System.Collections.Generic;

namespace ScenarioGenerator
{
    public interface ISolution
    {
        IList<(string Name, IEnumerable<string> ProjectReferences)> Projects { get; }
        string MainProject { get; }
    }
}
