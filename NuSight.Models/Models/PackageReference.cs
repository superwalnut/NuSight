using System;
namespace NuSight.Models.Models
{
    public class PackageReference
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public PackageShortSummary Summary { get; set; }
    }
}
