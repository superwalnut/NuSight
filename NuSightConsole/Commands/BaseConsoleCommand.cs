using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using ManyConsole;
using NuSight.Models.Models;
using NuSightConsole.Commands.Enums;
using NuSightConsole.Interfaces;

namespace NuSightConsole.Commands
{
    public abstract class BaseConsoleCommand : ConsoleCommand, IConsoleCommand
    {
        public override int Run(string[] remainingArguments)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            int errorCode = 0;
            try
            {
                errorCode = RunCommand();
            }
            catch(Exception ex)
            {
                errorCode = (int)ExitCodes.UnknownError;
            }
            finally{
                PrintSuccessLine($"Running for {sw.Elapsed.TotalSeconds} seconds");
                PrintJobCompleted(errorCode);
            }

            return errorCode;
        }

        public virtual int RunCommand()
        {
            throw new NotImplementedException("command is not implemented");
        }

        protected void PrintTitleLine(string title)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(title);
            Console.ResetColor();
        }

        protected void PrintSubTitleLine(string sub)
        {
            Console.WriteLine(sub);
            Console.WriteLine("---------------------------------------------");
        }

        protected void PrintSplitLine()
        {
            Console.WriteLine();
            Console.WriteLine("              **   **   **   **              ");
            Console.WriteLine();
        }

        protected void PrintSuccessLine(string line)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(line);
            Console.ResetColor();
        }

        protected void PrintErrorLine(string line)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(line);
            Console.ResetColor();
        }

        protected void PrintJobCompleted(int code)
        {
            Console.ForegroundColor = code!=0?ConsoleColor.Red:ConsoleColor.Green;
            Console.WriteLine($"Job completed with code {code} ...");
            Console.ResetColor();
        }

        protected void PrintLinesForList(List<string> list)
        {
            foreach (var item in list)
            {
                Console.WriteLine(item);
            }
        }

        protected void PrintLinesForPackages(List<PackageReference> packages)
        {
            foreach (var p in packages)
            {
                PrintErrorLine($"{p.Project.Project.PadRight(50, ' ')}: {p.Name.PadRight(80, ' ')} - {p.Version.PadRight(10, ' ')}");
            }
        }

        protected string GenerateUpdateCommand(string csproj, string package, string version)
        {
            if (string.IsNullOrEmpty(version))
            {
                return $"dotnet add {csproj} package {package}";
            }

            return $"dotnet add {csproj} package {package} -v {version}";
        }

        protected string GenerateRemoveCommand(string csproj, string package, string version)
        {
            if (string.IsNullOrEmpty(version))
            {
                return $"dotnet remove {csproj} package {package}";
            }
            return $"dotnet remove {csproj} package {package} -v {version}";
        }

    }
}
