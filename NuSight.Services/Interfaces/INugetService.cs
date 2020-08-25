using System;
using System.Threading.Tasks;
using NuSight.Models.Nuget;

namespace NuSight.Services.Interfaces
{
    public interface INugetService
    {
        Task<NugetPackageSummary> SearchByPackageName(string packageName);

        Task<NugetPackageCatalogEntry> GetNugetVersionSummary(string versionId);
    }
}
