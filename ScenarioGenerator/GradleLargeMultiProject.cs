using System.Collections.Generic;
using System.Linq;

namespace ScenarioGenerator
{
    // https://github.com/gradle/performance-comparisons/tree/master/large-multiproject
    public class GradleLargeMultiProject : ISolution
    {
        public IList<(string Name, IEnumerable<string> ProjectReferences)> Projects => new List<(string Name, IEnumerable<string> ProjectReferences)>
        {
            ( "1", Enumerable.Empty<string>() ),

            ( "2", new string[] { "1" } ),

            ( "3", new string[] { "1", "2" } ),
            ( "4", new string[] { "1", "2" } ),

            ( "5", new string[] { "4", "2", "1" } ),

            ( "6", new string[] { "1", "2", "4", "5" } ),

            ( "7", new string[] { "2", "4", "5", "6" } ),

            ( "8", new string[] { "4", "5", "6", "7" } ),
            ( "9", new string[] { "5", "6", "7" } ),

            ( "10", new string[] { "6", "7", "8" } ),
            ( "11", new string[] { "6", "7", "8" } ),
            ( "12", new string[] { "7", "8", "9" } ),
            ( "13", new string[] { "7", "8", "9" } ),
            ( "14", new string[] { "8", "9" } ),
        };


        public string MainProject => "14";

        public Scenario Scenario => Scenario.ClassLib;
    }
}
