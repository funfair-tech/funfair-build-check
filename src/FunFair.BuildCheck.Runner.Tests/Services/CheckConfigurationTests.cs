using FunFair.BuildCheck.Runner.Services;
using FunFair.Test.Common;
using Xunit;

namespace FunFair.BuildCheck.Runner.Tests.Services;

public sealed class CheckConfigurationTests : TestBase
{
    [Fact]
    public void PreReleaseBuildTrueIsStoredCorrectly()
    {
        CheckConfiguration config = new(PreReleaseBuild: true, AllowPackageVersionMismatch: false);

        Assert.True(config.PreReleaseBuild, "PreReleaseBuild should be true when initialized with true");
    }

    [Fact]
    public void PreReleaseBuildFalseIsStoredCorrectly()
    {
        CheckConfiguration config = new(PreReleaseBuild: false, AllowPackageVersionMismatch: false);

        Assert.False(config.PreReleaseBuild, "PreReleaseBuild should be false when initialized with false");
    }

    [Fact]
    public void AllowPackageVersionMismatchTrueIsStoredCorrectly()
    {
        CheckConfiguration config = new(PreReleaseBuild: false, AllowPackageVersionMismatch: true);

        Assert.True(
            config.AllowPackageVersionMismatch,
            "AllowPackageVersionMismatch should be true when initialized with true"
        );
    }

    [Fact]
    public void AllowPackageVersionMismatchFalseIsStoredCorrectly()
    {
        CheckConfiguration config = new(PreReleaseBuild: false, AllowPackageVersionMismatch: false);

        Assert.False(
            config.AllowPackageVersionMismatch,
            "AllowPackageVersionMismatch should be false when initialized with false"
        );
    }
}
