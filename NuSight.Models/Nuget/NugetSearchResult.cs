namespace NuSight.Models.Nuget
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public partial class NugetSearchResult
    {
        [JsonProperty("@context")]
        public NugetContext Context { get; set; }

        [JsonProperty("totalHits")]
        public long TotalHits { get; set; }

        [JsonProperty("data")]
        public List<NugetPackageSummary> Data { get; set; }
    }

    public partial class PackageType
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public partial class Version
    {
        [JsonProperty("version")]
        public string SemanticVersion { get; set; }

        [JsonProperty("downloads")]
        public long Downloads { get; set; }

        [JsonProperty("@id")]
        public string Id { get; set; }
    }    
}
