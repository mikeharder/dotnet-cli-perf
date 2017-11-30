﻿using System.Collections.Generic;
using System.Linq;

namespace ScenarioGenerator
{
    public class SmallWeb : ISolution
    {
        public IList<(string Name, IEnumerable<string> ProjectReferences)> Projects => new List<(string Name, IEnumerable<string> ProjectReferences)>
        {
            // Rank 0
            ( "ClassLib1", Enumerable.Empty<string>() ),
            ( "ClassLib2", Enumerable.Empty<string>() ),

            // Rank 1
            ( "ClassLib3", new string[] { "ClassLib1" } ),

            // Rank 2
            ( "WebApp1", new string[] { "ClassLib2", "ClassLib3" } ),
        };

        public string MainProject => "WebApp1";

        public Scenario Scenario => Scenario.WebApp;
    }
}
