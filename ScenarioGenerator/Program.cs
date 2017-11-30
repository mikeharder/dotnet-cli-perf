using CommandLine;
using System;

namespace ScenarioGenerator
{
    class Options
    {
        [Option('s', "solution")]
        public string Solution { get; set; }
    }

    class Program
    {
        private static Options _options;

        static int Main(string[] args)
        {
            return Parser.Default.ParseArguments<Options>(args).MapResult(
                options => Run(options),
                _ => 1
            );
        }

        private static int Run(Options options)
        {
            _options = options;

            var type = Type.GetType($"ScenarioGenerator.{_options.Solution}", throwOnError: true, ignoreCase: true);
            ISolution template = (ISolution)Activator.CreateInstance(type);

            

            return 0;
        }

    }
}
