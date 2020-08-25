using System;
using Newtonsoft.Json;

namespace NuSight.Models.Nuget
{
    public class NugetContext
    {
        [JsonProperty("@vocab")]
        public string Vocab { get; set; }

        [JsonProperty("xsd")]
        public string Xsd { get; set; }

        [JsonProperty("catalogEntry")]
        public CatalogEntry CatalogEntry { get; set; }

        [JsonProperty("registration")]
        public CatalogEntry Registration { get; set; }

        [JsonProperty("packageContent")]
        public CatalogEntry PackageContent { get; set; }

        [JsonProperty("published")]
        public CatalogEntry Published { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; }

        [JsonProperty("@base")]
        public string Base { get; set; }

        [JsonProperty("catalog")]
        public string Catalog { get; set; }

        [JsonProperty("dependencies")]
        public Dependencies Dependencies { get; set; }

        [JsonProperty("dependencyGroups")]
        public Dependencies DependencyGroups { get; set; }

        [JsonProperty("packageEntries")]
        public Dependencies PackageEntries { get; set; }

        [JsonProperty("packageTypes")]
        public Dependencies PackageTypes { get; set; }

        [JsonProperty("supportedFrameworks")]
        public Dependencies SupportedFrameworks { get; set; }

        [JsonProperty("tags")]
        public Dependencies Tags { get; set; }

        [JsonProperty("created")]
        public CatalogCommitTimeStamp Created { get; set; }

        [JsonProperty("lastEdited")]
        public CatalogCommitTimeStamp LastEdited { get; set; }

        [JsonProperty("catalog:commitTimeStamp")]
        public CatalogCommitTimeStamp CatalogCommitTimeStamp { get; set; }

        [JsonProperty("reasons")]
        public Reasons Reasons { get; set; }
    }
}
