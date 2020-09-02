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
    [AutofacRegistrationOrder(1)]
    public class ListCommand : BaseConsoleCommand
    {
        private readonly IProjectService _projectService;
        private readonly ILogger _logger;

        private readonly ListCommandOption _option;

        public ListCommand(IProjectService projectService, ILogger logger)
        {
            _projectService = projectService;
            _logger = logger;
            _option = new ListCommandOption();

            this.IsCommand("list", "List nuget packages from selected solution & highlight the outdated packages");
            this.HasOption("s|solution=", "solution path", v => _option.SolutionPath = v);
            this.HasOption("i|inconsistency", "check package with inconsistent versions, if true errors out", v => _option.CheckInconsistency = true);
            this.HasOption("o|outdated", "check any outdated packages, if true errors out.", v => _option.CheckOutdated = true);
            this.HasOption("p|prereleased", "check any pre-release packages, if true errors out.", v => _option.CheckPreReleased = true);
            this.HasOption("u|unpublished", "check any unpublished packages, if true errors out.", v => _option.CheckUnpublished = true);
            //this.HasOption("ic|incompatible", "check packages with incompatible frameworks, if true errors out", v => _option.CheckIncompatibleFramework = true);
        }

        public override int RunCommand()
        {
            if (string.IsNullOrEmpty(_option.SolutionPath))
                _option.SolutionPath = Environment.CurrentDirectory;

            var packages = _projectService.GetAllProjectFilesAsync(_option.SolutionPath).Result;

            if(packages.Count <= 0)
            {
                PrintTitleLine("Didn't find any nuget packages or csproj files.");
                PrintSplitLine();
                return (int)ExitCodes.Success;
            }

            // search solution/project for nuget packages
            var projects = packages.GroupBy(x=>x.Project);
            foreach(var p in projects)
            {
                PrintTitleLine($"{p.Key.Project} - {p.Key.Framework} - found {p.Count()} nuget packages");
                PrintSubTitleLine($"{p.Key.Path}");

                Console.WriteLine($"{"Package".PadRight(80, ' ')}   {"Version".PadRight(10, ' ')}");
                foreach (var d in p.ToList())
                {
                    Console.WriteLine($"\n{d.Name.PadRight(80, ' ')} - {d.Version.PadRight(10, ' ')}");
                }

                PrintSplitLine();
            }

            ExitCodes inconsistentResult = default, outdatedResult = default, prereleasedResult = default, unpublished = default;
            if(_option.CheckInconsistency)
            {
                inconsistentResult = CheckInconsistency(packages);
            }
            
            if(_option.CheckOutdated)
            {
                outdatedResult = CheckOutdated(packages);
            }

            if(_option.CheckPreReleased)
            {
                prereleasedResult = CheckPreReleased(packages);
            }

            if(_option.CheckUnpublished)
            {
                unpublished = CheckUnpublished(packages);
            }
            
            return (int)(inconsistentResult | outdatedResult | prereleasedResult | unpublished);
        }

        private ExitCodes CheckInconsistency(List<PackageReference> packages)
        {
            PrintTitleLine("Checking inconsistent packages");
            var inconsistentPackages = packages.GroupBy(x=>x.Name).Where(x=>x.Select(p=>p.Version).Distinct().Count() > 1);
            if(inconsistentPackages.Count()>0)
            {
                Console.WriteLine($"{"Package".PadRight(80, ' ')}   {"Version".PadRight(10, ' ')}");
                foreach (var p in inconsistentPackages.SelectMany(x => x))
                {
                    PrintErrorLine($"{(p.Project.Project + ":" + p.Name).PadRight(80, ' ')} - {p.Version.PadRight(10, ' ')}");
                }
                PrintSplitLine();
                return ExitCodes.InconsistentVersion;
            }
            PrintSuccessLine("No packages inconsistency in projects");
            PrintSplitLine();

            return ExitCodes.Success;
        }

        private ExitCodes CheckOutdated(List<PackageReference> packages)
        {
            PrintTitleLine("Checking outdated packages");
            var outdated = packages.Where(x=>x.Summary.BehindCount > 0);
            if(outdated.Count() > 0)
            {
                Console.WriteLine($"{"Package".PadRight(80, ' ')}   {"Version".PadRight(10, ' ')}   {"Latest".ToString().PadRight(10, ' ')}   {"Behind".PadRight(10, ' ')}");
                foreach(var p in outdated)
                {
                    PrintErrorLine($"{(p.Project.Project + ":" + p.Name).PadRight(80, ' ')} - {p.Version.PadRight(10, ' ')} - {(p.Summary.IsUnpublished?"Unpublished":p.Summary.LatestReleaseVersion).PadRight(10, ' ')} - {(!p.Summary.IsUnpublished?p.Summary.BehindCount.ToString():"").PadRight(10, ' ')}");
                }
                PrintSplitLine();
                return ExitCodes.OutdatedPackage;
            }
            PrintSuccessLine("No packages outdated in projects");
            PrintSplitLine();

            return ExitCodes.Success;
        }

        private ExitCodes CheckPreReleased(List<PackageReference> packages)
        {
            PrintTitleLine("Checking pre-releaed packages");
            var prereleased = packages.Where(x=>x.IsPreReleaseVersion);
            if(prereleased.Count() >0)
            {
                Console.WriteLine($"{"Package".PadRight(80, ' ')}   {"Version".PadRight(10, ' ')}");
                foreach (var p in prereleased)
                {
                    PrintErrorLine($"{(p.Project.Project + ":" + p.Name).PadRight(80, ' ')} - {p.Version.PadRight(10, ' ')}");
                }
                PrintSplitLine();
                return ExitCodes.PreReleasedPackage;
            }
            PrintSuccessLine("No pre-released packages in projects");
            PrintSplitLine();

            return ExitCodes.Success;
        }

        private ExitCodes CheckUnpublished(List<PackageReference> packages)
        {
            PrintTitleLine("Checking unpublished packages");
            var prereleased = packages.Where(x => x.Summary.IsUnpublished);
            if (prereleased.Count() > 0)
            {
                Console.WriteLine($"{"Package".PadRight(80, ' ')}   {"Version".PadRight(10, ' ')}    {"Publish Status".ToString().PadRight(10, ' ')}");
                foreach (var p in prereleased)
                {
                    PrintErrorLine($"{(p.Project.Project + ":" + p.Name).PadRight(80, ' ')} - {p.Version.PadRight(10, ' ')} -> {"Unpublished".PadRight(10, ' ')}");
                }
                PrintSplitLine();
                return ExitCodes.PreReleasedPackage;
            }
            PrintSuccessLine("No unpublished packages in projects");
            PrintSplitLine();

            return ExitCodes.Success;
        }
    }
}
