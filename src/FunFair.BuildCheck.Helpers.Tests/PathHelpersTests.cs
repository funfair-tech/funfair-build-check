using System.IO;
using FunFair.BuildCheck.Helpers;
using FunFair.Test.Common;
using Xunit;

namespace FunFair.BuildCheck.Helpers.Tests;

public sealed class PathHelpersTests : TestBase
{
    [Theory]
    [InlineData("path/to/file", "path/to/file")]
    [InlineData(@"path\to\file", "path/to/file")]
    [InlineData("", "")]
    [InlineData("single", "single")]
    public void ConvertToNativeReturnsPathWithNativeSeparators(string input, string expectedOnLinux)
    {
        string expected =
            Path.DirectorySeparatorChar == '/'
                ? expectedOnLinux
                : expectedOnLinux.Replace('/', Path.DirectorySeparatorChar);

        string actual = PathHelpers.ConvertToNative(input);

        Assert.Equal(expected: expected, actual: actual);
    }
}
