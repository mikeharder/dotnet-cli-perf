using System;
using System.Collections.Generic;
using System.Text;

namespace ScenarioGenerator
{
    interface IScenario
    {
        IDictionary<string, IEnumerable<string>> ProjectReferences { get; }
        string MainProject { get; }
    }
}
