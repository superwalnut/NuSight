using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace NuSightConsole
{
    public static class BashCommand 
    {
        public static string Bash(this string cmd)
        {
            if(IsWindows())
            {
                return RunWindowCommand(cmd);
            }

            if(IsMacOS() || IsLinux())
            {
                return RunBash(cmd);
            }

            throw new Exception("Command is not supported for your operating system.");
        }

        public static string RunWindowCommand(string cmd)
        {
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.Start();

            process.StandardInput.WriteLine(cmd);
            process.StandardInput.Flush();
            process.StandardInput.Close();
            process.WaitForExit();            
            string result = process.StandardOutput.ReadToEnd();
            return result;
        }

        public static string RunBash(string cmd)
        {
            var escapedArgs = cmd.Replace("\"", "\\\"");

            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{escapedArgs}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return result;
        }

        public static bool IsWindows() =>
        RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        public static bool IsMacOS() =>
            RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

        public static bool IsLinux() =>
            RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
    }
}