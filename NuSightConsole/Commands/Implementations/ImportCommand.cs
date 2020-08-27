using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ManyConsole;
using Newtonsoft.Json;
using NuSight.Core.Attributes;
using NuSight.Models.Models;
using NuSight.Services.Interfaces;
using NuSightConsole.Interfaces;
using Serilog;

namespace NuSightConsole.Commands
{
    [AutofacRegistrationOrder(2)]
    public class ImportCommand : BaseConsoleCommand
    {
        private readonly IProjectService _projectService;
        private readonly ILogger _logger;

        private string _sourcePath;

        private string _path;

        private bool _doDisplay;

        public ImportCommand(IProjectService projectService, ILogger logger)
        {
            _projectService = projectService;
            _logger = logger;

            this.IsCommand("import", "Import nuget package references of selected project.");
            this.HasOption("p|project=", "select project path", v => _sourcePath = v);
            this.HasOption("f|file=", "saved nuget-reference json file path", v => _path = v);
            this.HasOption("d|display", "display dotnet install commands only for selected packages", v => _doDisplay = true);
        }

        public override int Run(string[] remainingArguments)
        {
            var valid = ValidateProjectFile(_sourcePath);

            if (!valid)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Project file is not found to import ...");
                Console.ResetColor();
                return -1;
            }

            var packages = GetPackages(_path);
            var commands = new List<string>();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Import nuget packages ...");
            Console.ResetColor();
            Console.WriteLine("-----------------------------------------");

            foreach (var p in packages)
            {
                var command = GenerateUpdateCommand(_sourcePath, p.Package, p.Version);
                commands.Add(command);
                Console.WriteLine(command);
            }

            if (!_doDisplay && commands.Count > 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Install selected packages");
                Console.ResetColor();
                Console.WriteLine("-----------------------------------------");
                foreach (var c in commands)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Running command: {c}");
                    Console.ResetColor();
                    Console.WriteLine(BashCommand.Bash(c));
                }

                Console.WriteLine("*****************************************");
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Job completed...");
            Console.ResetColor();

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

            return null;
        }
    }
}