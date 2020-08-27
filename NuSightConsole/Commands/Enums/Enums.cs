using System;
namespace NuSightConsole.Commands.Enums
{
    [Flags]
    enum ExitCodes : int
    {
        Success = 0,
        InconsistentVersion = 1,
        OutdatedPackage = 2,
        PreReleasedPackage = 4,
        UnpublishedPackage = 8,
        IncompatibleFramework = 16,
        UnknownError = 32
    }
}
