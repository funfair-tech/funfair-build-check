using FunFair.BuildCheck.Interfaces;

namespace FunFair.BuildCheck.Services;

/// <summary>
///     Check configuration.
/// </summary>
public sealed class CheckConfiguration : ICheckConfiguration
{
    /// <inheritdoc />
    public bool PreReleaseBuild { get; init; }
}