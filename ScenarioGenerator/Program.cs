using System;
using System.Collections.Generic;
using System.Linq;

namespace ScenarioGenerator
{
    class Program
    {
        // https://github.com/gradle/performance-comparisons/tree/master/large-multiproject
        private static readonly IDictionary<int, IEnumerable<int>> _dependencies = new Dictionary<int, IEnumerable<int>>
        {
            { 1, Enumerable.Empty<int>() },

            { 2, new int[] { 1 } },

            { 3, new int[] { 1, 2 } },
            { 4, new int[] { 1, 2 } },

            { 5, new int[] { 4, 2, 1 } },

            { 6, new int[] { 1, 2, 4, 5 } },

            { 7, new int[] { 2, 4, 5, 6 } },

            { 8, new int[] { 4, 5, 6, 7 } },
            { 9, new int[] { 5, 6, 7 } },

            { 10, new int[] { 6, 7, 8 } },
            { 11, new int[] { 6, 7, 8} },
            { 12, new int[] { 7, 8, 9 } },
            { 13, new int[] { 7, 8, 9 } },
            { 14, new int[] { 8, 9 } },
        };

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
