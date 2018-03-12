﻿using Microsoft.Extensions.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Common
{
    public static class Util
    {
        public static readonly Lazy<string> _repoRoot = new Lazy<string>(() =>
        {
            var currentDir = string.Empty;
            while (true)
            {
                if (File.Exists(Path.Combine(currentDir, "DotNetCliPerf.sln")))
                {
                    return currentDir;
                }
                else
                {
                    currentDir = Path.Combine(currentDir, "..");
                }
            }
        });

        public static string RepoRoot => _repoRoot.Value;

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
            // If delete fails, retry once every second up to 20 times.
            for (var i = 0; i < 20; i++)
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
                catch (FileNotFoundException)
                {
                    Util.WriteLine("Nothing to do");
                    break;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error deleting directory: {e.ToString()}");

                    if (i < 19)
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

        public static (Process Process, StringBuilder OutputBuilder, StringBuilder ErrorBuilder) StartProcess(
            string filename, string arguments, string workingDirectory = null, IDictionary<string, string> environment = null)
        {
            if (String.IsNullOrEmpty(workingDirectory))
            {
                workingDirectory = Directory.GetCurrentDirectory();
            }

            Util.WriteLine($"{filename} {arguments}");

            var process = new Process()
            {
                StartInfo =
                {
                    FileName = filename,
                    Arguments = arguments,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    WorkingDirectory = workingDirectory,
                },
            };

            if (environment != null)
            {
                foreach (var kvp in environment)
                {
                    process.StartInfo.Environment.Add(kvp);
                }
            }

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

            return (process, outputBuilder, errorBuilder);
        }

        public static string StopProcess(Process process, StringBuilder outputBuilder, StringBuilder errorBuilder,
            bool throwOnError = true)
        {
            if (!process.HasExited)
            {
                process.KillTree();
            }

            return WaitForExit(process, outputBuilder, errorBuilder, throwOnError: throwOnError);
        }

        public static string WaitForExit(Process process, StringBuilder outputBuilder, StringBuilder errorBuilder,
            bool throwOnError = true)
        {
            // Workaround issue where WaitForExit() blocks until child processes are killed, which is problematic
            // for the dotnet.exe NodeReuse child processes.  I'm not sure why this is problematic for dotnet.exe child processes
            // but not for MSBuild.exe child processes.  The workaround is to specify a large timeout.
            // https://stackoverflow.com/a/37983587/102052
            process.WaitForExit(int.MaxValue);

            if (throwOnError && process.ExitCode != 0)
            {
                throw new InvalidOperationException(
                    $"Command {process.StartInfo.FileName} {process.StartInfo.Arguments} returned exit code {process.ExitCode}");
            }

            return outputBuilder.ToString();
        }

        public static string RunProcess(string filename, string arguments, string workingDirectory = null,
            bool throwOnError = true, IDictionary<string, string> environment = null)
        {
            var p = StartProcess(filename, arguments, workingDirectory, environment: environment);
            return WaitForExit(p.Process, p.OutputBuilder, p.ErrorBuilder, throwOnError: throwOnError);
        }

        // Replace contents in a file without changing encoding
        private static void ReplaceInFile(string path, Func<string, string> replacer)
        {
            string contents;
            Encoding encoding;
            using (var reader = new StreamReader(File.OpenRead(path)))
            {
                contents = reader.ReadToEnd();
                encoding = reader.CurrentEncoding;
            }

            contents = replacer(contents);

            File.WriteAllText(path, contents, encoding);
        }

        public static void ReplaceInFile(string path, string oldValue, string newValue)
        {
            ReplaceInFile(path, s => s.Replace(oldValue, newValue));
        }

        public static void RegexReplaceInFile(string path, string pattern, string replacement)
        {
            ReplaceInFile(path, s => Regex.Replace(s, pattern, replacement));
        }

        public static void InsertInFileBefore(string path, string insertBefore, string value)
        {
            ReplaceInFile(path, insertBefore, value + insertBefore);
        }

        public static void InsertInFileAfter(string path, string insertAfter, string value)
        {
            ReplaceInFile(path, insertAfter, insertAfter + value);
        }

        // https://docs.microsoft.com/en-us/dotnet/standard/io/how-to-copy-directories
        public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            Parallel.ForEach(dir.EnumerateFiles(), file =>
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            });

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                Parallel.ForEach(dir.EnumerateDirectories(), subdir =>
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                });
            }
        }
    }
}
