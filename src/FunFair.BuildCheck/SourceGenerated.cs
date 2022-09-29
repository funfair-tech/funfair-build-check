using System;
using System.Text.RegularExpressions;

namespace FunFair.BuildCheck;

#if NET6_0
internal static class SourceGenerated
{
    private static readonly TimeSpan Short = TimeSpan.FromMilliseconds(RegexSettings.TimeoutMilliseconds);

    public static Regex ProjectReferenceRegex()
    {
        return new(pattern: RegexSettings.ProjectReferenceRegex, options: RegexSettings.ProjectReferenceOptions, matchTimeout: Short);
    }
}
#else
internal static partial class SourceGenerated
{
    [GeneratedRegex(pattern: RegexSettings.ProjectReferenceRegex, options: RegexSettings.ProjectReferenceOptions, matchTimeoutMilliseconds: RegexSettings.TimeoutMilliseconds)]
    public static partial Regex ProjectReferenceRegex();
}
#endif