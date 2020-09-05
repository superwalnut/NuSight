using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using AutoMapper;
using NuSight.Models.Models;
using NuSight.Models.Nuget;
using NuSight.Services.Interfaces;
using Serilog;

namespace NuSight.Services.Services
{
    public class ProjectService : IProjectService
    {
        private readonly INugetService _nugetService;

        private readonly IMapper _mapper;

        private readonly ILogger _logger;

        public ProjectService(INugetService nugetService, IMapper mapper, ILogger logger)
        {
            _nugetService = nugetService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<PackageReference>> GetAllProjectFilesAsync(string path)
        {
            var list = new List<PackageReference>();
            var csprojFiles = new List<FileInfo>();

            if (File.Exists(path) && Path.GetExtension(path).Equals(".csproj"))
                csprojFiles.Add(new FileInfo(path));
            else
            {
                // Loop through folders and find all project files
                DirectoryInfo root = new DirectoryInfo(path);
                var files = root.GetFiles("*.csproj", SearchOption.AllDirectories);
                csprojFiles.AddRange(files);
            }
            
            foreach (var f in csprojFiles)
            {
                var project = new ProjectInfo
                {
                    Project = f.Name,
                    FolderName = f.Directory.Name,
                    Path = f.FullName,                                        
                };
                var packages = await GetProjectDependencyPackages(project);
                list.AddRange(packages);
            }
            return list;
        }

        private async Task<List<PackageReference>> GetProjectDependencyPackages(ProjectInfo project)
        {
            var references = new List<PackageReference>();
            XDocument projDefinition = XDocument.Load(project.Path);

            // set framework for project
            project.Framework = GetFrameworkVersion(projDefinition);

            // parse nuget package dependencies
            var packageReferences = projDefinition.XPathSelectElements("//PackageReference");

            foreach (var pr in packageReferences)
            {
                var refs = new PackageReference
                {
                    Name = pr.Attribute("Include").Value,
                    Version = pr.Attribute("Version").Value,                    
                    Project = project
                };

                references.Add(refs);
            }

            var tasks = references.Select(x => GetPackageShortSummary(x.Name, x.Version).ContinueWith(o => x.Summary = o.Result));

            await Task.WhenAll(tasks);

            return references;
        }

        public async Task<PackageShortSummary> GetPackageShortSummary(string packageName, string version)
        {
            var package = await _nugetService.SearchByPackageName(packageName);

            if(package != null)
            {
                var behindCount = package.BehindCount(version);

                var latestReleaseVersion = package.LatestReleaseVersion();

                var shortSummary = _mapper.Map<PackageShortSummary>(package);

                shortSummary.BehindCount = behindCount;
                shortSummary.LatestReleaseVersion = latestReleaseVersion;

                return shortSummary;
            }
            return new PackageShortSummary { IsUnpublished  = true };
        }

        private string GetFrameworkVersion(XDocument projDefinition)
        {
            string framework = null;
            var frameworkVersionNode = projDefinition.XPathSelectElement("//TargetFrameworkVersion");
            if (frameworkVersionNode != null)
            {
                framework = frameworkVersionNode.Value;
            }

            var frameworkNode = projDefinition.XPathSelectElement("//TargetFramework");
            if (frameworkNode != null)
            {
                framework = frameworkNode.Value;
            }

            return framework;
        }
    }
}
