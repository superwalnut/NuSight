using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ManyConsole;
using NuSight.Core.Attributes;
using NuSight.Services.Interfaces;
using NuSightConsole.Interfaces;
using Serilog;

namespace NuSightConsole.Commands
{
    [AutofacRegistrationOrder(6)]
    public class CloneCommand : BaseConsoleCommand
    {
        private readonly IProjectService _projectService;
        private readonly ILogger _logger;

        private string _sourcePath;

        private string _targetPath;

        private bool _doInstall;

        private bool _useLatestVersion;

        public CloneCommand(IProjectService projectService, ILogger logger)
        {
            _projectService = projectService;
            _logger = logger;

            this.IsCommand("clone", "copy nuget packages from selected solution & install to the target project.");
            this.HasOption("s|source=", "source project path", v => _sourcePath = v);
            this.HasOption("t|target=", "target .csproj path", v => _targetPath = v);
            this.HasOption("i|install", "run install command to clone packages to your target csproj", v => _doInstall = true);
            this.HasOption("v|latest", "using latest version", x => _useLatestVersion = true);
        }

        public override int Run(string[] remainingArguments)
        {
            if(!ValidateTargetProjectFile())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"The target project file is not exist.");
                Console.ResetColor();
            }

            var packages = _projectService.GetAllProjectFilesAsync(_sourcePath).Result;
            var commands = new List<string>();

            var foundPackages = packages.Select(x => new { Package = x.Name, Version = x.Version }).Distinct().ToList();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Printing install commands for cloning packages");
            Console.ResetColor();
            Console.WriteLine("-----------------------------------------");

            foreach (var p in foundPackages)
            {
                var command = GenerateUpdateCommand(_targetPath, p.Package, _useLatestVersion?null:p.Version);
                commands.Add(command);
                Console.WriteLine(command);
            }

            Console.WriteLine("*****************************************");

            if(_doInstall)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Installing {commands.Count} packages for {_targetPath}");
                Console.ResetColor();

                foreach(var c in commands)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Running command: {c}");
                    Console.ResetColor();
                    Console.WriteLine(BashCommand.Bash(c));
                }
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Job completed...");
            Console.ResetColor();

            return 0;
        }

        private bool ValidateTargetProjectFile()
        {
            if(!_targetPath.EndsWith(".csproj") || !File.Exists(_targetPath))
                return false;
            return true;
        }
    }
}