using BenchmarkDotNet.Parameters;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCliPerf
{
    static class ParameterInstancesExtensions
    {
        public static bool Match(this ParameterInstances parameters, IDictionary<string, string> patterns)
        {
            foreach (var pattern in patterns)
            {
                foreach (var parameter in parameters.Items)
                {
                    if (parameter.Name.Equals(pattern.Key, StringComparison.OrdinalIgnoreCase) &&
                        pattern.Value != "*" &&
                        parameter.Value.ToString().IndexOf(pattern.Value, StringComparison.OrdinalIgnoreCase) < 0)
                        return false;
                }
            }

            return true;
        }
    }
}

