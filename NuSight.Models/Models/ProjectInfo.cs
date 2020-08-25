using System;
using System.Collections.Generic;

namespace NuSight.Models.Models
{
    public class ProjectInfo
    {
        public string Project { get; set; }
        public string FolderName { get; set; }
        public string Path { get; set; }
        public string Framework { get; set; }
        public List<PackageReference> Packages { get; set; }
    }
}
