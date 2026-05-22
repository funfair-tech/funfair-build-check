using FunFair.BuildCheck.Interfaces;
using FunFair.Test.Common;
using Xunit;

namespace FunFair.BuildCheck.Interfaces.Tests;

public sealed class SolutionProjectTests : TestBase
{
    [Fact]
    public void FileNameIsStoredCorrectly()
    {
        SolutionProject project = new(fileName: "MyProject.csproj", displayName: "MyProject");

        Assert.Equal(expected: "MyProject.csproj", actual: project.FileName);
    }

    [Fact]
    public void DisplayNameIsStoredCorrectly()
    {
        SolutionProject project = new(fileName: "MyProject.csproj", displayName: "MyProject");

        Assert.Equal(expected: "MyProject", actual: project.DisplayName);
    }
}
