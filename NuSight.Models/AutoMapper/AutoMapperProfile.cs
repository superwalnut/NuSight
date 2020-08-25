using AutoMapper;
using NuSight.Models;
using NuSight.Models.Models;
using NuSight.Models.Nuget;

namespace NuSight.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<NugetPackageSummary, PackageShortSummary>()
                .ForMember(x => x.Description, o => o.MapFrom(z => z.Description))
                .ForMember(x => x.IconUrl, o => o.MapFrom(z => z.IconUrl))
                .ForMember(x => x.LatestVersion, o => o.MapFrom(z => z.Version))
                .ForMember(x => x.NugetPackageId, o => o.MapFrom(z => z.Id))
                .ForMember(x => x.PackageApiUrl, o => o.MapFrom(z => z.Url))
                .ForMember(x => x.ProjectUrl, o => o.MapFrom(z => z.ProjectUrl))
                .ForAllOtherMembers(x=>x.Ignore())
                ;
        }
    }
}
