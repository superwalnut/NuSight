using System;
using System.Collections.Generic;
using System.Linq;
using ManyConsole;
using NuSight.Core.Attributes;
using NuSight.Services.Interfaces;
using NuSightConsole.Commands.Options;
using NuSightConsole.Interfaces;
using Serilog;

namespace NuSightConsole.Commands
{
    [AutofacRegistrationOrder(4)]
    public class DeleteCommand : BaseConsoleCommand
    {
        private readonly IProjectService _projectService;
        private readonly ILogger _logger;

        private DeleteCommandOption _option;

        public DeleteCommand(IProjectService projectService, ILogger logger)
        {
            _projectService = projectService;
            _logger = logger;
            _option = new DeleteCommandOption();

            this.IsCommand("remove", "remove specified nuget packages from selected solution.");
            this.HasOption("s|solution=", "solution path", v => _option.SolutionPath = v);
            this.HasOption("p|package=", "select package name", v => _option.PackageName = v);
            this.HasOption("d|display", "display 'remove commands' only for selected package.", v => _option.DisplayOnly = true);
        }

        public override int RunCommand()
        {
            if (string.IsNullOrEmpty(_option.SolutionPath))
                _option.SolutionPath = Environment.CurrentDirectory;

            var packages = _projectService.GetAllProjectFilesAsync(_option.SolutionPath).Result;
            
            var selectedPackages = GetPackageNames(_option.PackageName).ToList();

            PrintProjectGroups(packages);

            var deletes = packages.Where(x=> selectedPackages.Contains(x.Name)).Select(x => (GenerateRemoveCommand(x.Project.Path, x.Name))).ToList();

            // print update commands
            if (deletes.Count > 0)
            {
                PrintTitleLine("Printing remove commands for selected packages");
                PrintLinesForList(deletes);
                PrintSplitLine();
            }

            if (!_option.DisplayOnly && deletes.Count > 0)
            {
                PrintSubTitleLine("Removing selected packages");
                foreach (var c in deletes)
                {
                    PrintSuccessLine($"Running command: {c}");
                    Console.WriteLine(BashCommand.Bash(c));
                }

                PrintSplitLine();
            }

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