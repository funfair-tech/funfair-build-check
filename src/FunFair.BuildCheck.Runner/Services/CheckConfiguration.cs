using FunFair.BuildCheck.Interfaces;

namespace FunFair.BuildCheck.Runner.Services;

public sealed class CheckConfiguration : ICheckConfiguration
{
    public CheckConfiguration(bool preReleaseBuild, bool allowPackageVersionMismatch)
    {
        this.PreReleaseBuild = preReleaseBuild;
        this.AllowPackageVersionMismatch = allowPackageVersionMismatch;
    }

    public bool PreReleaseBuild { get; }

    public bool AllowPackageVersionMismatch { get; }
}
