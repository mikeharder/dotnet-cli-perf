using BenchmarkDotNet.Parameters;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCliPerf
{
    static class ParameterInstancesExtensions
    {
        public static bool Match(this ParameterInstances parameters, IEnumerable<string> patterns)
        {
            foreach (var pattern in patterns)
            {
                var parts = pattern.Split('=');
                var key = parts[0];
                var value = parts[1];
                foreach (var parameter in parameters.Items)
                {
                    if (parameter.Name.Equals(key, StringComparison.OrdinalIgnoreCase) &&
                        parameter.Value.ToString().IndexOf(value, StringComparison.OrdinalIgnoreCase) < 0)
                        return false;
                }
            }

            return true;
        }
    }
}

