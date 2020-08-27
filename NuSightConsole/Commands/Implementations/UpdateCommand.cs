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

        private readonly ListCommandOption _option;

        public UpdateCommand(IProjectService projectService, ILogger logger)
        {
            _projectService = projectService;
            _logger = logger;
            _option = new ListCommandOption();

            this.IsCommand("list", "List nuget packages from selected solution & highlight the outdated packages");
            this.HasOption("u|update", "run updating scripts for outdated packages to the latest.", v => _option.RunUpdate = true);
            this.HasOption("s|solution=", "solution path", v => _option.SolutionPath = v);
        }

        public override int RunCommand()
        {
            var updates = new List<string>();
            var packages = _projectService.GetAllProjectFilesAsync(_option.SolutionPath).Result;

            // search solution/project for nuget packages
            var projects = packages.GroupBy(x => x.Project);
            foreach (var p in projects)
            {
                PrintTitleLine($"{p.Key.Project} - {p.Key.Framework}");
                PrintSubTitleLine($"{p.Key.Path}");

                foreach (var d in p.ToList())
                {
                    if (d.Summary.BehindCount > 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        updates.Add(GenerateUpdateCommand(p.Key.Path, d.Name, d.Summary.LatestVersion));
                    }
                    Console.WriteLine($"\n{d.Name.PadRight(80, ' ')} - {d.Version.PadRight(10, ' ')} - {d.Summary.BehindCount.ToString().PadRight(3, ' ')}");
                    Console.ResetColor();
                }

                PrintSplitLine();
            }

            // print update commands for projects with out of dated packages
            if (updates.Count > 0)
            {
                PrintTitleLine("Printing update commands for updating packages");
                PrintLinesForList(updates);
                PrintSplitLine();
            }

            // execute update commands to update nuget packages
            if (_option.RunUpdate && updates.Count > 0)
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
