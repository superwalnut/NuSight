namespace NuSight.Models.Nuget
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public partial class NugetPackageVersionSummary
    {
        [JsonProperty("@id")]
        public string Id { get; set; }

        [JsonProperty("@type")]
        public List<string> Type { get; set; }

        [JsonProperty("catalogEntry")]
        public string CatalogEntry { get; set; }

        [JsonProperty("listed")]
        public bool Listed { get; set; }

        [JsonProperty("packageContent")]
        public string PackageContent { get; set; }

        [JsonProperty("published")]
        public DateTimeOffset Published { get; set; }

        [JsonProperty("registration")]
        public string Registration { get; set; }

        [JsonProperty("@context")]
        public NugetContext Context { get; set; }
    }

    public partial class CatalogEntry
    {
        [JsonProperty("@type")]
        public string Type { get; set; }
    }
}
