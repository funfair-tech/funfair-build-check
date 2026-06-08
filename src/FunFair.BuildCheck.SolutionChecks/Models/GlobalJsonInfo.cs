namespace FunFair.BuildCheck.SolutionChecks.Models;

public sealed class GlobalJsonInfo
{
    public GlobalJsonInfo(string? sdkVersion, string? rollForward, bool? allowPrerelease)
    {
        this.SdkVersion = sdkVersion;
        this.RollForward = rollForward;
        this.AllowPrerelease = allowPrerelease;
    }

    public string? SdkVersion { get; }

    public string? RollForward { get; }

    public bool? AllowPrerelease { get; }
}
