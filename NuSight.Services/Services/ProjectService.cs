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

namespace NuSight.Services.Services
{
    public class ProjectService : IProjectService
    {
        private readonly INugetService _nugetService;

        private readonly IMapper _mapper;

        public ProjectService(INugetService nugetService, IMapper mapper)
        {
            _nugetService = nugetService;
            _mapper = mapper;
        }

        public async Task<List<PackageReference>> GetAllProjectFilesAsync(string path)
        {
            var list = new List<PackageReference>();
            // Loop through folders and find all projct files
            DirectoryInfo root = new DirectoryInfo(path);
            var files = root.GetFiles("*.csproj", SearchOption.AllDirectories);
            foreach (var f in files)
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
                    Summary = await GetPackageShortSummary(pr.Attribute("Include").Value, pr.Attribute("Version").Value),
                    Project = project
                };

                references.Add(refs);
            }
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
