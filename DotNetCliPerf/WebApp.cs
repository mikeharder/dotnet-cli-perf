using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.IO;

namespace DotNetCliPerf
{
    public class WebApp : RootTemp
    {
        private const string _oldMain = "BuildWebHost(args).Run();";

        private const string _newMain =
@"
var sw = System.Diagnostics.Stopwatch.StartNew();
BuildWebHost(args).RunAsync();
var response = (new System.Net.Http.HttpClient()).GetStringAsync(""http://localhost:5000"").Result;
sw.Stop();

Console.WriteLine(response);
Console.WriteLine(sw.Elapsed);
";

        private const string _viewData = @"ViewData[""Title""] = ""Home Page"";";
        private const string _returnView = @"return View();";

        private string _oldTitle = "Home Page";
        private string _newTitle;
        private string _output;

        [GlobalSetup]
        public override void GlobalSetup()
        {
            base.GlobalSetup();

            DotNet("new angular --no-restore");
            Npm("install");

            // Update Main() to execute a request after starting the app
            Util.ReplaceInFile(Path.Combine(RootTempDir, "Program.cs"), _oldMain, _newMain);

            // Move ViewData["Title"] from View to Controller, so it can be updated to trigger a recompile
            Util.ReplaceInFile(Path.Combine(RootTempDir, "Views", "Home", "Index.cshtml"),
                _viewData, string.Empty);
            Util.ReplaceInFile(Path.Combine(RootTempDir, "Controllers", "HomeController.cs"),
                _returnView, _viewData + _returnView);

            var output = DotNet("run");

            // Verify response
            var expected = "<title>Home Page";
            if (!output.Contains(expected))
            {
                throw new InvalidOperationException($"Response missing '{expected}'");
            }
        }

        [IterationSetup]
        public void IterationSetup()
        {
            _newTitle = Guid.NewGuid().ToString();
            Util.ReplaceInFile(Path.Combine(RootTempDir, "Controllers", "HomeController.cs"), _oldTitle, _newTitle);
        }

        [Benchmark]
        public void Incremental()
        {
            _output = DotNet("run");
        }

        [IterationCleanup]
        public void IterationCleanup()
        {
            // Verify new title
            var expected = $"<title>{_newTitle}";
            if (!_output.Contains(expected))
            {
                throw new InvalidOperationException($"Response missing '{expected}'");
            }

            _oldTitle = _newTitle;
        }

        private string DotNet(string arguments)
        {
            return Util.RunProcess("dotnet", arguments, RootTempDir);
        }

        private void Npm(string arguments)
        {
            Util.RunProcess("cmd", $"/c \"npm.cmd {arguments}\"", RootTempDir);
        }
    }
}
