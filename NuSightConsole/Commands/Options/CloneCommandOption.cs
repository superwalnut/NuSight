using System;
namespace NuSightConsole.Commands.Options
{
    public class CloneCommandOption
    {
        public string SourcePath { get; set; }

        public bool DisplayOnly { get; set; }

        public string TargetPath { get; set; }

        public bool UseLatestVersion { get; set; }
    }
}
