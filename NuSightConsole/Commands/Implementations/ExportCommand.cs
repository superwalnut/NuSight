using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ManyConsole;
using Newtonsoft.Json;
using NuSight.Core.Attributes;
using NuSight.Services.Interfaces;
using NuSightConsole.Commands.Options;
using NuSightConsole.Interfaces;
using Serilog;

namespace NuSightConsole.Commands
{
    [AutofacRegistrationOrder(3)]
    public class ExportCommand : BaseConsoleCommand
    {
        private readonly IProjectService _projectService;
        private readonly ILogger _logger;
        private readonly string _exportName = "export.json";

        private ExportCommandOption _option;

        public ExportCommand(IProjectService projectService, ILogger logger)
        {
            _projectService = projectService;
            _logger = logger;
            _option = new ExportCommandOption();

            this.IsCommand("export", "Export nuget package references of selected project.");
            this.HasOption("s|source=", "source project path", v => _option.SolutionPath = v);
            this.HasOption("f|file=", "save packages referece list as json file", v => _option.FileName = v);
        }

        public override int RunCommand()
        {
            if (string.IsNullOrEmpty(_option.SolutionPath))
                _option.SolutionPath = Environment.CurrentDirectory;

            if (string.IsNullOrEmpty(_option.FileName))
                _option.FileName = Path.Combine(Environment.CurrentDirectory, _exportName);

            var packages = _projectService.GetAllProjectFilesAsync(_option.SolutionPath).Result;
            var commands = new List<string>();

            PrintTitleLine("Export package reference as file...");

            var data = packages.Select(x=> new { Package = x.Name, Version = x.Version }).Distinct().ToList();

            var json = JsonConvert.SerializeObject(data);

            var target = GetTargetFilePath(_option.FileName);

            System.IO.File.WriteAllText(target, json);

            PrintSuccessLine($"Saved file {target}");
        
            return 0;
        }

        private string GetTargetFilePath(string path)
        {            
            if (Directory.Exists(path))
                return Path.Combine(path, _exportName);
            else
                return path;
        }
    }
}