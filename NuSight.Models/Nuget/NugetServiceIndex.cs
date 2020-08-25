namespace NuSight.Models.Nuget
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using NuSight.Models.Interfaces;

    public partial class NugetServiceIndex
    {
        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("resources")]
        public List<NugetServiceIndexResource> Resources { get; set; }

        [JsonProperty("@context")]
        public NugetContext Context { get; set; }
    }
}
