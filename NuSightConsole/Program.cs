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
            var serviceProvider = startup.BuildContainer();

            var list = serviceProvider.GetService<ICommandList>();           

            return ConsoleCommandDispatcher.DispatchCommand(list.Commands, args, Console.Out);
        }
    }
}
