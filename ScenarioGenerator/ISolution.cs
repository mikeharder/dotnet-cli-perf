using System.Collections.Generic;

namespace ScenarioGenerator
{
    interface ISolution
    {
        IList<(string Name, IEnumerable<string> ProjectReferences)> Projects { get; }
        string MainProject { get; }
    }
}
