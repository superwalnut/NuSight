using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Autofac;
using AutoMapper.Contrib.Autofac.DependencyInjection;
using ManyConsole;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NuSight.AutoMapper;
using NuSight.Core.Configs;
using NuSight.Services.Interfaces;
using NuSight.Services.Modules;
using NuSight.Services.Services;
using NuSightConsole.Interfaces;
using NuSightConsole.Modules;

namespace NuSightConsole
{
    class Program
    {
        private static IContainer Container { get; set; }

        static int Main(string[] args)
        {
            var startup = new Startup();
            ConfigureServices(startup.ContainerBuilder);
            var serviceProvider = startup.BuildContainer();

            var listCommand = serviceProvider.GetService<IListCommand>();
            var cloneCommand = serviceProvider.GetService<ICloneCommand>();
            var removeCommand = serviceProvider.GetService<IDeleteCommand>();
            var exportCommand = serviceProvider.GetService<IExportCommand>();
            var importCommand = serviceProvider.GetService<IImportCommand>();

            var commands = new List<ConsoleCommand>();            
            commands.Add(listCommand as ConsoleCommand);
            commands.Add(cloneCommand as ConsoleCommand);
            commands.Add(removeCommand as ConsoleCommand);
            commands.Add(exportCommand as ConsoleCommand);
            commands.Add(importCommand as ConsoleCommand);

            // then run them.
            return ConsoleCommandDispatcher.DispatchCommand(commands, args, Console.Out);
        }

        private static void ConfigureServices(ContainerBuilder builder)
        {
            builder.RegisterModule<ConsoleModule>();
        }
    }
}
