using System;
namespace NuSight.Modules
{
    using Autofac;
    using AutofacSerilogIntegration;
    using NuSight.AutoMapper;
    using global::AutoMapper.Contrib.Autofac.DependencyInjection;
    using NuSight.Services.Modules;

    public class ApiModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterLogger();
            builder.AddAutoMapper(x => x.AddProfile<AutoMapperProfile>());

            builder.RegisterModule<ServicesModule>();
        }
    }
}
