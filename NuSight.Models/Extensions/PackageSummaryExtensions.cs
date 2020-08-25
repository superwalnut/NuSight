using System;
using System.Collections.Generic;
using System.Linq;

namespace NuSight.Models.Nuget
{
    public static class PackageSummaryExtensions
    {
        public static int BehindCount(this NugetPackageSummary summary, string selected)
        {        
            var versions = summary.Versions.Select(x=>x.SemanticVersion).ToList();

            var index = versions.IndexOf(selected);

            return versions.Count() - 1 - index;            
        }
    }
}
