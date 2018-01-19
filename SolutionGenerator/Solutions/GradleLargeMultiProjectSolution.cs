using System.Collections.Generic;
using System.Linq;

namespace ScenarioGenerator.Solutions
{
    // https://github.com/gradle/performance-comparisons/tree/master/large-multiproject
    class GradleLargeMultiProjectSolution : ISolution
    {
        public IList<(string Name, IEnumerable<string> ProjectReferences, IEnumerable<string> PackageReferences)> Projects { get; } =
            new List<(string Name, IEnumerable<string> ProjectReferences, IEnumerable<string> PackageReferences)>
        {
            ( "1", Enumerable.Empty<string>(), Enumerable.Empty<string>() ),

            ( "2", new string[] { "1" }, Enumerable.Empty<string>() ),

            ( "3", new string[] { "1", "2" }, Enumerable.Empty<string>() ),
            ( "4", new string[] { "1", "2" }, Enumerable.Empty<string>() ),

            ( "5", new string[] { "4", "2", "1" }, Enumerable.Empty<string>() ),

            ( "6", new string[] { "1", "2", "4", "5" }, Enumerable.Empty<string>() ),

            ( "7", new string[] { "2", "4", "5", "6" }, Enumerable.Empty<string>() ),

            ( "8", new string[] { "4", "5", "6", "7" }, Enumerable.Empty<string>() ),
            ( "9", new string[] { "5", "6", "7" }, Enumerable.Empty<string>() ),

            ( "10", new string[] { "6", "7", "8" }, Enumerable.Empty<string>() ),
            ( "11", new string[] { "6", "7", "8" }, Enumerable.Empty<string>() ),
            ( "12", new string[] { "7", "8", "9" }, Enumerable.Empty<string>() ),
            ( "13", new string[] { "7", "8", "9" }, Enumerable.Empty<string>() ),
            ( "14", new string[] { "8", "9" }, Enumerable.Empty<string>() ),
        };


        public string MainProject => "14";

        public Scenario Scenario => Scenario.ClassLib;
    }
}
