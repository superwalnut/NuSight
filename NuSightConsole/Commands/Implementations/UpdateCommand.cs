using System;
using System.Collections.Generic;
using ManyConsole;
using NuSight.Core.Attributes;
using NuSight.Models.Models;
using NuSight.Services.Interfaces;
using NuSightConsole.Commands.Options;
using NuSightConsole.Interfaces;
using Serilog;
using System.Linq;
using NuSightConsole.Commands.Enums;

namespace NuSightConsole.Commands
{
    [AutofacRegistrationOrder(5)]
    public class UpdateCommand : BaseConsoleCommand
    {
        private readonly IProjectService _projectService;
        private readonly ILogger _logger;

        private readonly UpdateCommandOption _option;

        public UpdateCommand(IProjectService projectService, ILogger logger)
        {
            _projectService = projectService;
            _logger = logger;
            _option = new UpdateCommandOption();

            this.IsCommand("update", "Update nuget packages from selected solution & highlight the outdated packages");
            this.HasOption("s|solution=", "solution path", v => _option.SolutionPath = v);
            this.HasOption("d|display", "display 'update commands' only for selected package.", v => _option.DisplayOnly = true);
        }

        public override int RunCommand()
        {
            if (string.IsNullOrEmpty(_option.SolutionPath))
                _option.SolutionPath = Environment.CurrentDirectory;

            var packages = _projectService.GetAllProjectFilesAsync(_option.SolutionPath).Result;

            PrintProjectGroups(packages);

            var updates = packages.Select(x=> (GenerateUpdateCommand(x.Project.Path, x.Name, x.Summary.LatestReleaseVersion))).ToList();

            // print update commands
            if (updates.Count > 0)
            {
                PrintTitleLine("Printing update commands for updating packages");
                PrintLinesForList(updates);
                PrintSplitLine();
            }

            // execute update commands to update nuget packages
            if (!_option.DisplayOnly && updates.Count > 0)
            {
                RunUpdate(updates);
            }

            return 0;
        }

        private void RunUpdate(List<string> updates)
        {
            PrintTitleLine("Updating outdated packages");
            foreach (var u in updates)
            {
                PrintSuccessLine($"Running command: {u}");
                Console.WriteLine(BashCommand.Bash(u));
            }
            PrintSplitLine();
        }

    }
}
