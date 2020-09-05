using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ManyConsole;
using NuSight.Core.Attributes;
using NuSight.Services.Interfaces;
using NuSightConsole.Commands.Options;
using NuSightConsole.Interfaces;
using Serilog;
using NuSightConsole.Commands.Enums;

namespace NuSightConsole.Commands
{
    [AutofacRegistrationOrder(6)]
    public class CloneCommand : BaseConsoleCommand
    {
        private readonly IProjectService _projectService;
        private readonly ILogger _logger;

        private CloneCommandOption _option;

        public CloneCommand(IProjectService projectService, ILogger logger)
        {
            _projectService = projectService;
            _logger = logger;
            _option = new CloneCommandOption();

            this.IsCommand("clone", "copy nuget packages from selected solution & install to the target project.");
            this.HasOption("s|source=", "source project path", v => _option.SourcePath = v);
            this.HasOption("t|target=", "target .csproj path", v => _option.TargetPath = v);
            this.HasOption("d|display", "run install command to clone packages to your target csproj", v => _option.DisplayOnly = true);
            this.HasOption("l|latest", "using latest version", x => _option.UseLatestVersion = true);
        }

        public override int RunCommand()
        {
            if (string.IsNullOrEmpty(_option.TargetPath))
                _option.TargetPath = Environment.CurrentDirectory;

            if(!ValidateTargetProjectFile())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"The target project file is not exist.");
                Console.ResetColor();

                return (int)ExitCodes.UnknownError;
            }

            var packages = _projectService.GetAllProjectFilesAsync(_option.SourcePath).Result;
            var commands = new List<string>();

            var foundPackages = packages.Select(x => new { Package = x.Name, Version = x.Version }).Distinct().ToList();

            PrintTitleLine("Printing install commands for cloning packages");

            foreach (var p in foundPackages)
            {
                var command = GenerateUpdateCommand(_option.TargetPath, p.Package, _option.UseLatestVersion?null:p.Version);
                commands.Add(command);
                Console.WriteLine(command);
            }

            PrintSplitLine();

            if(!_option.DisplayOnly)
            {
                PrintTitleLine($"Installing {commands.Count} packages for {_option.TargetPath}");

                foreach(var c in commands)
                {
                    PrintSuccessLine($"Running command: {c}");
                    Console.WriteLine(BashCommand.Bash(c));
                }
            }

            return 0;
        }

        private bool ValidateTargetProjectFile()
        {
            if(!_option.TargetPath.EndsWith(".csproj") || !File.Exists(_option.TargetPath))
                return false;
            return true;
        }
    }
}