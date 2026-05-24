using FunFair.BuildCheck.Runner.Services;
using FunFair.Test.Common;
using Xunit;

namespace FunFair.BuildCheck.Runner.Tests.Services;

public sealed class CheckConfigurationTests : TestBase
{
    [Fact]
    public void PreReleaseBuildTrueIsStoredCorrectly()
    {
        CheckConfiguration config = new(preReleaseBuild: true, allowPackageVersionMismatch: false);

        Assert.True(config.PreReleaseBuild, "PreReleaseBuild should be true when initialized with true");
    }

    [Fact]
    public void PreReleaseBuildFalseIsStoredCorrectly()
    {
        CheckConfiguration config = new(preReleaseBuild: false, allowPackageVersionMismatch: false);

        Assert.False(config.PreReleaseBuild, "PreReleaseBuild should be false when initialized with false");
    }

    [Fact]
    public void AllowPackageVersionMismatchTrueIsStoredCorrectly()
    {
        CheckConfiguration config = new(preReleaseBuild: false, allowPackageVersionMismatch: true);

        Assert.True(
            config.AllowPackageVersionMismatch,
            "AllowPackageVersionMismatch should be true when initialized with true"
        );
    }

    [Fact]
    public void AllowPackageVersionMismatchFalseIsStoredCorrectly()
    {
        CheckConfiguration config = new(preReleaseBuild: false, allowPackageVersionMismatch: false);

        Assert.False(
            config.AllowPackageVersionMismatch,
            "AllowPackageVersionMismatch should be false when initialized with false"
        );
    }
}
