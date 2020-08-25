using System;
using Newtonsoft.Json;
using NuSight.Models.Interfaces;

namespace NuSight.Models.Nuget
{
    public partial class NugetServiceIndexResource : ICacheItem
    {
        [JsonProperty("@id")]
        public string Id { get; set; }

        [JsonProperty("@type")]
        public string Type { get; set; }

        [JsonProperty("comment", NullValueHandling = NullValueHandling.Ignore)]
        public string Comment { get; set; }

        [JsonProperty("clientVersion", NullValueHandling = NullValueHandling.Ignore)]
        public string ClientVersion { get; set; }
    }
}
