using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ManyConsole;
using NuSight.Services.Interfaces;
using NuSightConsole.Interfaces;
using Serilog;

namespace NuSightConsole.Commands
{
    public class CloneCommand : ConsoleCommand, ICloneCommand
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

            var projects = _projectService.GetAllProjectFilesAsync(_sourcePath).Result;
            var commands = new List<string>();

            var packages = projects.SelectMany(x=>x.Packages).Distinct().ToList();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Printing install commands for cloning packages");
            Console.ResetColor();
            Console.WriteLine("-----------------------------------------");

            foreach (var p in packages)
            {
                var command = GenerateInstallCommand(_targetPath, p.Name, p.Version, _useLatestVersion);
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

        private string GenerateInstallCommand(string csproj, string package, string version, bool useLatestVersion)
        {
            if(useLatestVersion)
            {
                return $"dotnet add {csproj} package {package}";
            }

            return $"dotnet add {csproj} package {package} -v {version}";
        }

        private bool ValidateTargetProjectFile()
        {
            if(!_targetPath.EndsWith(".csproj") || !File.Exists(_targetPath))
                return false;
            return true;
        }
    }
}