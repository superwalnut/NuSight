using System;
namespace NuSightConsole.Modules
{
    using Autofac;
    using AutofacSerilogIntegration;
    using NuSight.AutoMapper;
    using global::AutoMapper.Contrib.Autofac.DependencyInjection;
    using NuSight.Services.Modules;
    using NuSightConsole.Commands;
    using NuSightConsole.Interfaces;

    public class ConsoleModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.AddAutoMapper(x => x.AddProfile<AutoMapperProfile>());
            builder.RegisterModule<ServicesModule>();

            builder.RegisterType<ListCommand>().As<IListCommand>();
            builder.RegisterType<CloneCommand>().As<ICloneCommand>();
            builder.RegisterType<DeleteCommand>().As<IDeleteCommand>();
            builder.RegisterType<ExportCommand>().As<IExportCommand>();
            builder.RegisterType<ImportCommand>().As<IImportCommand>();
        }
    }
}
