namespace FunFair.BuildCheck.Interfaces;

public interface ICheckConfiguration
{
    bool PreReleaseBuild { get; }

    bool AllowPackageVersionMismatch { get; }
}