using System;
namespace NuSightConsole.Commands.Options
{
    public class ListCommandOption
    {
        public string SolutionPath {get;set;}
        public bool RunUpdate {get;set;}

        public bool CheckInconsistency {get;set;}

        public bool CheckOutdated {get;set;}

        public bool CheckPreReleased {get;set;}

        public bool CheckUnpublished {get;set;}

        public bool CheckIncompatibleFramework {get;set;}
    }
}
