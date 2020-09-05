using System;
namespace NuSightConsole.Commands.Options
{
    public class DeleteCommandOption
    {
        public string SolutionPath { get; set; }

        public bool DisplayOnly { get; set; }

        public string PackageName { get; set; }
    }
}
