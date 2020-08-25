using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<List<ProjectInfo>> GetAllProjectFilesAsync(string path)
        {
            var list = new List<ProjectInfo>();
            // Loop through folders and find all projct files
            DirectoryInfo root = new DirectoryInfo(path);
            var files = root.GetFiles("*.csproj", SearchOption.AllDirectories);
            foreach (var f in files)
            {                
                var (packages, framework) = await GetProjectDependencyPackages(f.FullName);
                list.Add(new ProjectInfo
                {
                    Project = f.Name,
                    FolderName = f.Directory.Name,
                    Path = f.FullName,
                    Packages = packages,
                    Framework = framework
                });
            }
            return list;
        }

        private async Task<(List<PackageReference>,string)> GetProjectDependencyPackages(string file)
        {
            string framework = null;
            var references = new List<PackageReference>();
            XDocument projDefinition = XDocument.Load(file);
            var packageReferences = projDefinition.XPathSelectElements("//PackageReference");
            foreach (var pr in packageReferences)
            {
                var refs = new PackageReference
                {
                    Name = pr.Attribute("Include").Value,
                    Version = pr.Attribute("Version").Value,
                    Summary = await GetPackageShortSummary(pr.Attribute("Include").Value, pr.Attribute("Version").Value)
                };

                references.Add(refs);
            }

            var frameworkVersionNode = projDefinition.XPathSelectElement("//TargetFrameworkVersion");
            if(frameworkVersionNode!=null)
            {
                framework = frameworkVersionNode.Value;
            }

            var frameworkNode = projDefinition.XPathSelectElement("//TargetFramework");
            if(frameworkNode!= null)
            {
                framework = frameworkNode.Value;
            }

            return (references, framework);
        }

        public async Task<PackageShortSummary> GetPackageShortSummary(string packageName, string version)
        {
            var package = await _nugetService.SearchByPackageName(packageName);

            var behindCount = package.BehindCount(version);

            var shortSummary = _mapper.Map<PackageShortSummary>(package);

            shortSummary.BehindCount = behindCount;

            return shortSummary;
        }
    }
}
