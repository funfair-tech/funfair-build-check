namespace FunFair.BuildCheck.Interfaces;

/// <summary>
///     Check configuration details.
/// </summary>
public interface ICheckConfiguration
{
    /// <summary>
    ///     Whether the build is a pre-release build.
    /// </summary>
    bool PreReleaseBuild { get; }
}