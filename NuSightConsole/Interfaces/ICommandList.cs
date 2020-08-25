using System;
using System.Collections.Generic;
using ManyConsole;

namespace NuSightConsole.Interfaces
{
    public interface ICommandList
    {
        IList<ConsoleCommand> Commands { get; set; }
    }
}
