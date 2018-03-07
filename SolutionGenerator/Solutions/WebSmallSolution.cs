using System.Collections.Generic;
using System.Linq;

namespace SolutionGenerator.Solutions
{
    class WebSmallSolution : ISolution
    {
        public IList<(string Name, IEnumerable<string> ProjectReferences, IEnumerable<(string Name, string Version)> PackageReferences)> Projects { get; } =
            new List<(string Name, IEnumerable<string> ProjectReferences, IEnumerable<(string Name, string Version)> PackageReferences)>
        {
            // Rank 0
            ( "ClassLib1", Enumerable.Empty<string>(), Enumerable.Empty<(string Name, string Version)>() ),
            ( "ClassLib2", Enumerable.Empty<string>(), Enumerable.Empty<(string Name, string Version)>() ),

            // Rank 1
            ( "ClassLib3", new string[] { "ClassLib1" }, Enumerable.Empty<(string Name, string Version)>() ),

            // Rank 2
            ( "WebApp1", new string[] { "ClassLib2", "ClassLib3" }, Enumerable.Empty<(string Name, string Version)>() ),
        };

        public string MainProject => "WebApp1";
    }
}
