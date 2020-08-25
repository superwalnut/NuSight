namespace NuSight.Services.Modules
{
    using System;
    using Autofac;
    using CacheManager.Core;
    using NuSight.Models.Interfaces;
    using NuSight.Services.Interfaces;
    using NuSight.Services.Services;

    public class ServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<NugetService>().As<INugetService>();
            builder.RegisterType<ProjectService>().As<IProjectService>();

            builder.Register(x=>
            {
                var cache = CacheFactory.Build<ICacheItem>(s => s.WithDictionaryHandle().WithExpiration(ExpirationMode.Sliding, TimeSpan.FromHours(1)));
                return cache;
            }).As<ICacheManager<ICacheItem>>().SingleInstance();             
        }
    }
}
