using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using NuSight.Models.Interfaces;

namespace NuSight.Models.Nuget
{
    public partial class NugetPackageSummary : ICacheItem
    {
        [JsonProperty("@id")]
        public string Url { get; set; }

        [JsonProperty("@type")]
        public string Type { get; set; }

        [JsonProperty("registration")]
        public Uri Registration { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("iconUrl")]
        public string IconUrl { get; set; }

        [JsonProperty("licenseUrl")]
        public string LicenseUrl { get; set; }

        [JsonProperty("projectUrl")]
        public string ProjectUrl { get; set; }

        [JsonProperty("tags")]
        public List<string> Tags { get; set; }

        [JsonProperty("authors")]
        public List<string> Authors { get; set; }

        [JsonProperty("totalDownloads")]
        public long TotalDownloads { get; set; }

        [JsonProperty("verified")]
        public bool Verified { get; set; }

        [JsonProperty("packageTypes")]
        public List<PackageType> PackageTypes { get; set; }

        [JsonProperty("versions")]
        public List<Version> Versions { get; set; }
    }
}
