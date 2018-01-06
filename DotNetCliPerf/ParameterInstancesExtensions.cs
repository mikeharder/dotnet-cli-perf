using BenchmarkDotNet.Parameters;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCliPerf
{
    static class ParameterInstancesExtensions
    {
        public static bool Match(this ParameterInstances parameters, IDictionary<string, IEnumerable<string>> patterns)
        {
            foreach (var pattern in patterns)
            {
                foreach (var parameter in parameters.Items)
                {
                    if (parameter.Name.Equals(pattern.Key, StringComparison.OrdinalIgnoreCase) &&
                        !Match(parameter.Value.ToString(), pattern.Value))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private static bool Match(string parameter, IEnumerable<string> patterns)
        {
            foreach (var pattern in patterns)
            {
                if (pattern == "*" || parameter.IndexOf(pattern, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return true;
                }
            }

            return false;
        }
    }
}

