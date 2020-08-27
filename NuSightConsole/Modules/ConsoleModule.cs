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
    using ManyConsole;
    using System.Linq;
    using NuSight.Core.Attributes;

    public class ConsoleModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.AddAutoMapper(x => x.AddProfile<AutoMapperProfile>());
            builder.RegisterModule<ServicesModule>();

            var types = typeof(BaseConsoleCommand).Assembly.GetTypes().Where(x => x.IsAssignableTo<IConsoleCommand>() & !x.IsInterface & !x.IsAbstract);

            foreach (var t in types)
            {
                var orderAttribute = t.GetCustomAttributes(typeof(AutofacRegistrationOrderAttribute), false).OfType<AutofacRegistrationOrderAttribute>().FirstOrDefault();
                builder.RegisterType(t).AsImplementedInterfaces().WithMetadata(AutofacRegistrationOrderAttribute.AttributeName, orderAttribute?.Order);
            }

            builder.RegisterType<CommandList>().As<ICommandList>();

        }
    }
}
