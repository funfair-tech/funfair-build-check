using System.Text.RegularExpressions;

namespace FunFair.BuildCheck;

#if NET6_0
internal static class SourceGenerated
{
    public static Regex ProjectReferenceRegex()
    {
        return new(pattern: RegexSettings.ProjectReferenceRegex, options: RegexSettings.ProjectReferenceOptions, matchTimeout: RegexSettings.TimeoutTimeSpan);
    }
}
#else
internal static partial class SourceGenerated
{
    [GeneratedRegex(pattern: RegexSettings.ProjectReferenceRegex, options: RegexSettings.ProjectReferenceOptions, matchTimeoutMilliseconds: RegexSettings.TimeoutMilliseconds)]
    public static partial Regex ProjectReferenceRegex();
}
#endif