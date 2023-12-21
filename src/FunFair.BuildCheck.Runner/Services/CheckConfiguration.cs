using FunFair.BuildCheck.Interfaces;

namespace FunFair.BuildCheck.Runner.Services;

public sealed class CheckConfiguration : ICheckConfiguration
{
    public bool PreReleaseBuild { get; init; }
}