using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCliPerf
{
    static class EnumerableStringExtensions
    {
        public static bool ContainsAny(this IEnumerable<string> inputs, IEnumerable<string> patterns,
            StringComparison comparisonType = StringComparison.Ordinal)
        {
            foreach (var input in inputs)
            {
                if (input.ContainsAny(patterns, comparisonType))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
