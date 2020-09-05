using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ManyConsole;
using Newtonsoft.Json;
using NuSight.Core.Attributes;
using NuSight.Models.Models;
using NuSight.Services.Interfaces;
using NuSightConsole.Commands.Enums;
using NuSightConsole.Commands.Options;
using NuSightConsole.Interfaces;
using Serilog;

namespace NuSightConsole.Commands
{
    [AutofacRegistrationOrder(2)]
    public class ImportCommand : BaseConsoleCommand
    {
        private readonly IProjectService _projectService;
        private readonly ILogger _logger;

        private ImportCommandOption _option;

        public ImportCommand(IProjectService projectService, ILogger logger)
        {
            _projectService = projectService;
            _logger = logger;
            _option = new ImportCommandOption();

            this.IsCommand("import", "Import nuget package references of selected project.");
            this.HasOption("s|project=", "select project path", v => _option.SolutionPath = v);
            this.HasOption("f|file=", "saved nuget reference json file path", v => _option.FileName = v);
            this.HasOption("d|display", "display 'install commands' only for selected file", v => _option.DisplayOnly = true);
        }

        public override int RunCommand()
        {
            if (string.IsNullOrEmpty(_option.SolutionPath))
                _option.SolutionPath = Environment.CurrentDirectory;

            var valid = ValidateProjectFile(_option.SolutionPath);

            if (!valid)
            {
                PrintErrorLine($"Project file is not found to import ...");
                return (int)ExitCodes.UnknownError;
            }

            var foundPackages = _projectService.GetAllProjectFilesAsync(_option.SolutionPath).Result.ToList();

            PrintProjectGroups(foundPackages);

            var importPackages = GetPackages(_option.FileName);

            PrintTitleLine($"Import nuget packages ...");

            // if packages already exist, dont install them
            var selectedPackages = importPackages.Where(x => !foundPackages.Any(o => o.Name == x.Package && o.Version == x.Version)).ToList();

            if(selectedPackages.Count <= 0)
            {
                PrintErrorLine("import packages not found or already installed in solution.");
            }

            var commands = selectedPackages.Select(x => (GenerateUpdateCommand(_option.SolutionPath, x.Package, x.Version))).ToList();

            // print commands for projects
            if (commands.Count > 0)
            {
                PrintTitleLine("Printing update commands for importing packages");
                PrintLinesForList(commands);
                PrintSplitLine();
            }     

            if (!_option.DisplayOnly && commands.Count > 0)
            {
                PrintTitleLine("Install selected packages");
                foreach (var c in commands)
                {
                    PrintSuccessLine($"Running command: {c}");
                    Console.WriteLine(BashCommand.Bash(c));
                }

                PrintSplitLine();
            }

            return 0;
        }

        private bool ValidateProjectFile(string path)
        {
            if (File.Exists(path) && Path.GetExtension(path).Equals("csproj"))
            {
                return true;
            }

            if (Directory.Exists(path) && Directory.GetFiles(path, "*.csproj").Count()>0)
            {
                return true;
            }

            return false;
        }

        private List<ExportedPackage> GetPackages(string path)
        {
            if(File.Exists(path))
            {
                using(var reader = new StreamReader(path))
                {
                    var json = reader.ReadToEnd();
                    var packages = JsonConvert.DeserializeObject<List<ExportedPackage>>(json);
                    return packages;
                }
            }

            throw new FileNotFoundException("Import file is not found");
        }
    }
}