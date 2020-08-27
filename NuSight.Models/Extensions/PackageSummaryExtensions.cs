using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace NuSight.Models.Nuget
{
    public static class PackageSummaryExtensions
    {
        private static Regex regex = new Regex(@"^\d*.\d*.\d*(?<patch>-.*)?$");

        public static int BehindCount(this NugetPackageSummary summary, string selected)
        {        
            var versions = summary.Versions.Select(x=>x.SemanticVersion).ToList();

            var index = versions.IndexOf(selected);

            var lastReleasedIndex = versions.IndexOf(versions.LastOrDefault(x => !IsPreReleaseVersion(x)));

            var diff = lastReleasedIndex - index;

            return diff > 0 ? diff : 0;
        }

        public static string LatestReleaseVersion(this NugetPackageSummary summary)
        {
            var versions = summary.Versions.Select(x => x.SemanticVersion).ToList();

            return versions.LastOrDefault(x=> !IsPreReleaseVersion(x));
        }

        public static bool IsPreReleaseVersion(string version)
        {            
            var match = regex.Match(version);
            if (match.Success && match.Groups["patch"] != null && !string.IsNullOrEmpty(match.Groups["patch"].Value))
            {
                return true;
            }

            return false;
        }        
    }
}
