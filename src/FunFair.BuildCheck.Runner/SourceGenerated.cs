using System.Text.RegularExpressions;

namespace FunFair.BuildCheck.Runner;

internal static partial class SourceGenerated
{
    private const int TIMEOUT_MILLISECONDS = 5000;

    private const RegexOptions PROJECT_REFERENCE_OPTIONS = RegexOptions.Compiled | RegexOptions.ExplicitCapture;

    private const string PROJECT_REFERENCE_REGEX =
        "^Project\\(\"[{(]?[0-9A-F]{8}[-]?([0-9A-F]{4}[-]?){3}[0-9A-F]{12}[)}]\"\\)\\s*=\\s*\"(?<DisplayName>.*?)\",\\s*\"(?<FileName>.*?\\.csproj)\",\\s*\"[{(]?[0-9A-F]{8}[-]?([0-9A-F]{4}[-]?){3}[0-9A-F]{12}[)}]\"$";
}
