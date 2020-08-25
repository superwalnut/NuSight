using System;
using System.Threading.Tasks;
using NuSight.Core.Configs;
using Microsoft.Extensions.Options;
using NuSight.Models.Nuget;
using System.Linq;
using AutoMapper;
using NuSight.Services.Interfaces;
using Flurl.Http;
using CacheManager.Core;
using NuSight.Models.Interfaces;
using Newtonsoft.Json;
using Flurl.Http.Configuration;

namespace NuSight.Services.Services
{
    public class NugetService : INugetService
    {
        private readonly IOptions<NugetConfig> _config;

        private readonly IMapper _mapper;

        private readonly ICacheManager<ICacheItem> _cache;

        private const string SEARCH_QUERY_SERVICE = "SearchQueryService";

        public NugetService(IOptions<NugetConfig> config, IMapper mapper, ICacheManager<ICacheItem> cache)
        {
            _config = config;
            _mapper = mapper;
            _cache = cache;

            FlurlHttp.Configure(settings => {
                var jsonSettings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    ObjectCreationHandling = ObjectCreationHandling.Replace,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };
                settings.JsonSerializer = new NewtonsoftJsonSerializer(jsonSettings);
            });
        }

        private async Task<string> GetSearchQueryServiceUrl()
        {
            var cachedVal = _cache.Get(SEARCH_QUERY_SERVICE);

            if (cachedVal != null)
            {
                return ((NugetServiceIndexResource)cachedVal).Id;
            }                

            var services = await _config.Value.ServiceIndexUrl.GetJsonAsync<NugetServiceIndex>();            
            var service = services.Resources.FirstOrDefault(x => x.Type == SEARCH_QUERY_SERVICE);
            if (service != null)
            {
                _cache.Add(SEARCH_QUERY_SERVICE, service);
                return service.Id;
            }

            throw new Exception("Cannot retrieve nuget service index information");
        }

        public async Task<NugetPackageSummary> SearchByPackageName(string packageName)
        {
            var cached = _cache.Get(packageName);
            if (cached != null)
            {
                return (NugetPackageSummary)cached;
            }

            var baseUrl = await GetSearchQueryServiceUrl();
            var searchUrl = $"{baseUrl}?q={packageName}&semVerLevel=2.0.0&take=1&skip=0";
            var items = await searchUrl.GetJsonAsync<NugetSearchResult>();

            var package = items.Data.FirstOrDefault(x => x.Id.Equals(packageName, StringComparison.CurrentCultureIgnoreCase));

            _cache.Add(packageName, package);

            return package;
        }

        public async Task<NugetPackageCatalogEntry> GetNugetVersionSummary(string versionId)
        {
            var summary = await versionId.GetJsonAsync<NugetPackageVersionSummary>();

            if(summary != null)
            {
                var catalog = await summary.CatalogEntry.GetJsonAsync<NugetPackageCatalogEntry>();
                return catalog;
            }

            return null;
        }
    }
}
