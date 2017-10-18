using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace DotNetCliPerf
{
    static class Util
    {
        // BenchmarkDotNet requires output log lines to start with "//"
        public static void WriteLine(string value) => Console.WriteLine("// " + value);

        public static string GetTempDir()
        {
            var temp = Path.GetTempFileName();
            File.Delete(temp);
            Directory.CreateDirectory(temp);
            Util.WriteLine($"Created temp directory '{temp}'");
            return temp;
        }

        public static string GetTempDir(string path)
        {
            var temp = Path.Combine(path, Path.GetRandomFileName());
            if (Directory.Exists(temp))
            {
                // Retry
                return GetTempDir();
            }
            else
            {
                Directory.CreateDirectory(temp);
                Util.WriteLine($"Created temp directory '{temp}'");
                return temp;
            }
        }

        public static void DeleteDir(string path)
        {
            Console.WriteLine($"// Deleting directory '{path}'");

            // Delete occasionally fails with the following exception:
            //
            // System.UnauthorizedAccessException: Access to the path 'Benchmarks.dll' is denied.
            //
            // If delete fails, retry once every second up to 10 times.
            for (var i = 0; i < 10; i++)
            {
                try
                {
                    var dir = new DirectoryInfo(path) { Attributes = FileAttributes.Normal };
                    foreach (var info in dir.GetFileSystemInfos("*", SearchOption.AllDirectories))
                    {
                        info.Attributes = FileAttributes.Normal;
                    }
                    dir.Delete(recursive: true);
                    Util.WriteLine("SUCCESS");
                    break;
                }
                catch (DirectoryNotFoundException)
                {
                    Util.WriteLine("Nothing to do");
                    break;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error deleting directory: {e.ToString()}");

                    if (i < 9)
                    {
                        Util.WriteLine("RETRYING");
                        Thread.Sleep(TimeSpan.FromSeconds(1));
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        public static void RunProcess(string filename, string arguments, string workingDirectory)
        {
            Util.WriteLine($"{filename} {arguments}");

            var process = new Process()
            {
                StartInfo =
                {
                    FileName = filename,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    WorkingDirectory = workingDirectory,
                },
            };

            var outputBuilder = new StringBuilder();
            process.OutputDataReceived += (_, e) =>
            {
                outputBuilder.AppendLine(e.Data);
                Util.WriteLine(e.Data);
            };

            var errorBuilder = new StringBuilder();
            process.ErrorDataReceived += (_, e) =>
            {
                errorBuilder.AppendLine(e.Data);
                Util.WriteLine(e.Data);
            };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                throw new InvalidOperationException($"Command {filename} {arguments} returned exit code {process.ExitCode}");
            }
        }
    }
}
