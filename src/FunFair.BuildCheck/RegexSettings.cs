using System;
using System.Text.RegularExpressions;

namespace FunFair.BuildCheck;

internal static class RegexSettings
{
    public const int TimeoutMilliseconds = 5000;

    public const RegexOptions ProjectReferenceOptions = RegexOptions.Compiled | RegexOptions.ExplicitCapture;

    public const string ProjectReferenceRegex =
        "^Project\\(\"[{(]?[0-9A-F]{8}[-]?([0-9A-F]{4}[-]?){3}[0-9A-F]{12}[)}]\"\\)\\s*=\\s*\"(?<DisplayName>.*?)\",\\s*\"(?<FileName>.*?\\.csproj)\",\\s*\"[{(]?[0-9A-F]{8}[-]?([0-9A-F]{4}[-]?){3}[0-9A-F]{12}[)}]\"$";

    public static readonly TimeSpan TimeoutTimeSpan = TimeSpan.FromMilliseconds(TimeoutMilliseconds);
}