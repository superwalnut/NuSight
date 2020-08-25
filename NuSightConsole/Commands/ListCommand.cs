using System;
using System.Collections.Generic;
using ManyConsole;
using NuSight.Services.Interfaces;
using NuSightConsole.Interfaces;
using Serilog;

namespace NuSightConsole.Commands
{
    public class ListCommand : ConsoleCommand, IListCommand
    {
        private readonly IProjectService _projectService;
        private readonly ILogger _logger;

        private string _solutionPath;

        private bool _doUpdate;

        public ListCommand(IProjectService projectService, ILogger logger)
        {
            _projectService = projectService;
            _logger = logger;

            this.IsCommand("list", "List nuget packages from selected solution & highlight the outdated packages");
            this.HasOption("s|solution=", "solution path", v => _solutionPath = v);
            this.HasOption("u|update", "run updating scripts for outdated packages.", v => _doUpdate = true);
        }

        public override int Run(string[] remainingArguments)
        {
            var updates = new List<string>();
            var projects = _projectService.GetAllProjectFilesAsync(_solutionPath).Result;

            foreach(var p in projects)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"{p.Project} - {p.Framework}");
                Console.ResetColor();

                Console.WriteLine($"{p.Path}");
                Console.WriteLine("-----------------------------------------");
                foreach (var d in p.Packages)
                {
                    if (d.Summary.BehindCount > 0){
                        Console.ForegroundColor = ConsoleColor.Red;
                        updates.Add(GenerateUpdateCommand(p.Path, d.Name, d.Summary.LatestVersion));
                    }
                    Console.WriteLine($"\n{d.Name.PadRight(80, ' ')} - {d.Version.PadRight(10, ' ')} - {d.Summary.BehindCount.ToString().PadRight(3, ' ')}");
                    Console.ResetColor();
                }
                Console.WriteLine("*****************************************");
                Console.WriteLine();
            }

            // generate update commands for projects with out of dated packages
            if(updates.Count>0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Printing update commands for updating packages");
                Console.ResetColor();
                Console.WriteLine("-----------------------------------------");
                foreach(var u in updates)
                {
                    Console.WriteLine(u);
                }

                Console.WriteLine("*****************************************");
            }

            if(_doUpdate && updates.Count>0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Updating outdated packages");
                Console.ResetColor();
                Console.WriteLine("-----------------------------------------");
                foreach (var u in updates)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Running command: {u}");
                    Console.ResetColor();
                    Console.WriteLine(BashCommand.Bash(u));
                }

                Console.WriteLine("*****************************************");
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Job completed...");
            Console.ResetColor();

            return 0;
        }

        private string GenerateUpdateCommand(string csproj, string package, string version)
        {
            return $"dotnet add {csproj} package {package} -v {version}";
        }
    }
}
