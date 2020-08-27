using System;
using System.Collections.Generic;
using System.Linq;
using ManyConsole;
using NuSight.Core.Attributes;
using NuSight.Services.Interfaces;
using NuSightConsole.Interfaces;
using Serilog;

namespace NuSightConsole.Commands
{
    [AutofacRegistrationOrder(4)]
    public class DeleteCommand : BaseConsoleCommand
    {
        private readonly IProjectService _projectService;
        private readonly ILogger _logger;

        private string _solutionPath;

        private string _packageName;

        private bool _doDisplay;

        public DeleteCommand(IProjectService projectService, ILogger logger)
        {
            _projectService = projectService;
            _logger = logger;

            this.IsCommand("remove", "remove specified nuget packages from selected solution.");
            this.HasOption("s|solution=", "solution path", v => _solutionPath = v);
            this.HasOption("p|package=", "select package name", v => _packageName = v);
            this.HasOption("d|display", "display remove commands only for selected package.", v => _doDisplay = true);
        }

        public override int Run(string[] remainingArguments)
        {
            var packages = _projectService.GetAllProjectFilesAsync(_solutionPath).Result;
            var commands = new List<string>();
            
            var selectedPackages = GetPackageNames(_packageName);

            foreach (var p in packages)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"{p.Project.Project}");
                Console.ResetColor();

                Console.WriteLine($"{p.Project.Path}");
                Console.WriteLine("-----------------------------------------");

                if(p.Name.Equals(_packageName))
                {
                    var command = GenerateRemoveCommand(p.Project.Path, p.Name, p.Version);
                    commands.Add(command);
                    Console.WriteLine(command);
                }

                Console.WriteLine("*****************************************");
                Console.WriteLine();
            }

            if (!_doDisplay && commands.Count > 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Removing selected packages");
                Console.ResetColor();
                Console.WriteLine("-----------------------------------------");
                foreach (var c in commands)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Running command: {c}");
                    Console.ResetColor();
                    Console.WriteLine(BashCommand.Bash(c));
                }

                Console.WriteLine("*****************************************");
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Job completed...");
            Console.ResetColor();

            return 0;
        }

        private string[] GetPackageNames(string packageName)
        {
            if(!string.IsNullOrEmpty(packageName))
            {
                return packageName.Split(new char[]{','}, StringSplitOptions.RemoveEmptyEntries);                
            }
            return null;
        }
    }
}