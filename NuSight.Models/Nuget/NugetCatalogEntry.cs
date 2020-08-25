namespace NuSight.Models.Nuget
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public partial class NugetPackageCatalogEntry
    {
        [JsonProperty("@id")]
        public string Id { get; set; }

        [JsonProperty("@type")]
        public List<string> Type { get; set; }

        [JsonProperty("authors")]
        public string Authors { get; set; }

        [JsonProperty("catalog:commitId")]
        public Guid CatalogCommitId { get; set; }

        [JsonProperty("catalog:commitTimeStamp")]
        public DateTimeOffset CatalogCommitTimeStamp { get; set; }

        [JsonProperty("created")]
        public DateTimeOffset Created { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("frameworkAssemblyGroup")]
        public FrameworkAssemblyGroup FrameworkAssemblyGroup { get; set; }

        [JsonProperty("iconFile")]
        public string IconFile { get; set; }

        [JsonProperty("id")]
        public string NugetPackageCatalogEntryId { get; set; }

        [JsonProperty("isPrerelease")]
        public bool IsPrerelease { get; set; }

        [JsonProperty("lastEdited")]
        public DateTimeOffset LastEdited { get; set; }

        [JsonProperty("licenseExpression")]
        public string LicenseExpression { get; set; }

        [JsonProperty("licenseUrl")]
        public string LicenseUrl { get; set; }

        [JsonProperty("listed")]
        public bool Listed { get; set; }

        [JsonProperty("packageHash")]
        public string PackageHash { get; set; }

        [JsonProperty("packageHashAlgorithm")]
        public string PackageHashAlgorithm { get; set; }

        [JsonProperty("packageSize")]
        public long PackageSize { get; set; }

        [JsonProperty("projectUrl")]
        public string ProjectUrl { get; set; }

        [JsonProperty("published")]
        public DateTimeOffset Published { get; set; }

        [JsonProperty("releaseNotes")]
        public string ReleaseNotes { get; set; }

        [JsonProperty("repository")]
        public string Repository { get; set; }

        [JsonProperty("requireLicenseAcceptance")]
        public bool RequireLicenseAcceptance { get; set; }

        [JsonProperty("verbatimVersion")]
        public string VerbatimVersion { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("dependencyGroups")]
        public List<DependencyGroup> DependencyGroups { get; set; }

        [JsonProperty("packageEntries")]
        public List<PackageEntry> PackageEntries { get; set; }

        [JsonProperty("tags")]
        public List<string> Tags { get; set; }

        [JsonProperty("@context")]
        public NugetContext Context { get; set; }
    }  

    public partial class CatalogCommitTimeStamp
    {
        [JsonProperty("@type")]
        public string Type { get; set; }
    }

    public partial class Dependencies
    {
        [JsonProperty("@id")]
        public string Id { get; set; }

        [JsonProperty("@container")]
        public string Container { get; set; }
    }

    public partial class Reasons
    {
        [JsonProperty("@container")]
        public string Container { get; set; }
    }

    public partial class DependencyGroup
    {
        [JsonProperty("@id")]
        public string Id { get; set; }

        [JsonProperty("@type")]
        public string Type { get; set; }

        [JsonProperty("dependencies", NullValueHandling = NullValueHandling.Ignore)]
        public List<Dependency> Dependencies { get; set; }

        [JsonProperty("targetFramework")]
        public string TargetFramework { get; set; }
    }

    public partial class Dependency
    {
        [JsonProperty("@id")]
        public string Id { get; set; }

        [JsonProperty("@type")]
        public string Type { get; set; }

        [JsonProperty("id")]
        public string DependencyId { get; set; }

        [JsonProperty("range")]
        public string Range { get; set; }
    }

    public partial class FrameworkAssemblyGroup
    {
        [JsonProperty("@id")]
        public string Id { get; set; }

        [JsonProperty("assembly")]
        public string Assembly { get; set; }

        [JsonProperty("targetFramework")]
        public string TargetFramework { get; set; }
    }

    public partial class PackageEntry
    {
        [JsonProperty("@id")]
        public string Id { get; set; }

        [JsonProperty("@type")]
        public string Type { get; set; }

        [JsonProperty("compressedLength")]
        public long CompressedLength { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("length")]
        public long Length { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
