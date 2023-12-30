#if NET7_0_OR_GREATER
using System.Text.RegularExpressions;

namespace FunFair.BuildCheck.Runner;

internal static partial class SourceGenerated
{
    [GeneratedRegex(pattern: PROJECT_REFERENCE_REGEX, options: PROJECT_REFERENCE_OPTIONS, matchTimeoutMilliseconds: TIMEOUT_MILLISECONDS)]
    public static partial Regex ProjectReferenceRegex();
}
#endif