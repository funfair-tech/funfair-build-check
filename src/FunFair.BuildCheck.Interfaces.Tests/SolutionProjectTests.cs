using FunFair.BuildCheck.Interfaces;
using FunFair.Test.Common;
using Xunit;

namespace FunFair.BuildCheck.Interfaces.Tests;

public sealed class SolutionProjectTests : TestBase
{
    [Fact]
    public void FileNameIsStoredCorrectly()
    {
        SolutionProject project = new(FileName: "MyProject.csproj", DisplayName: "MyProject");

        Assert.Equal(expected: "MyProject.csproj", actual: project.FileName);
    }

    [Fact]
    public void DisplayNameIsStoredCorrectly()
    {
        SolutionProject project = new(FileName: "MyProject.csproj", DisplayName: "MyProject");

        Assert.Equal(expected: "MyProject", actual: project.DisplayName);
    }
}
