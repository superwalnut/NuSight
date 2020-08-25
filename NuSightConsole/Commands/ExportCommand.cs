using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ManyConsole;
using Newtonsoft.Json;
using NuSight.Core.Attributes;
using NuSight.Services.Interfaces;
using NuSightConsole.Interfaces;
using Serilog;

namespace NuSightConsole.Commands
{
    [AutofacRegistrationOrder(3)]
    public class ExportCommand : ConsoleCommand, IConsoleCommand
    {
        private readonly IProjectService _projectService;
        private readonly ILogger _logger;

        private string _sourcePath;

        private string _path;

        public ExportCommand(IProjectService projectService, ILogger logger)
        {
            _projectService = projectService;
            _logger = logger;

            this.IsCommand("export", "Export nuget package references of selected project.");
            this.HasOption("s|source=", "source project path", v => _sourcePath = v);
            this.HasOption("f|file=", "save packages referece list as json file", v => _path = v);
        }

        public override int Run(string[] remainingArguments)
        {            
            var projects = _projectService.GetAllProjectFilesAsync(_sourcePath).Result;
            var commands = new List<string>();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Saving package reference ...");
            Console.ResetColor();

            var packages = projects.SelectMany(x=>x.Packages).Distinct().ToList();
            
            var data = packages.Select(x=> new { Package = x.Name, Version = x.Version }).ToList();

            var json = JsonConvert.SerializeObject(data);

            System.IO.File.WriteAllText(GetTargetFilePath(_path), json);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Job completed...");
            Console.ResetColor();

            return 0;
        }

        private string GetTargetFilePath(string path)
        {
            if (Directory.Exists(path))
                return Path.Combine(path, "export.json");
            else
                return path;
        }
    }
}