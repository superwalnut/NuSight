using System;
using System.Text.RegularExpressions;
using NuSight.Models.Nuget;

namespace NuSight.Models.Models
{
    public class PackageReference
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public PackageShortSummary Summary { get; set; }

        public ProjectInfo Project {get;set;}

        public bool IsPreReleaseVersion
        {
            get
            {
                return PackageSummaryExtensions.IsPreReleaseVersion(Version);
            }
        }
    }
}
