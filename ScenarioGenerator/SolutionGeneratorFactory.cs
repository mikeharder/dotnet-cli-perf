using System;
using System.Collections.Generic;
using System.Text;

namespace ScenarioGenerator
{
    static class SolutionGeneratorFactory
    {
        public static ISolutionGenerator GetInstance(Framework framework)
        {
            switch (framework)
            {
                case Framework.Core:
                    return new CoreSolutionGenerator();
                case Framework.Framework:
                    return new CoreSolutionGenerator();
                case Framework.Gradle:
                    return new GradleSolutionGenerator();
                default:
                    throw new ArgumentException();
            }
        }
    }
}
