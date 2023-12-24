using System.IO;

namespace FunFair.BuildCheck.Helpers;

public static class PathHelpers
{
    public static string ConvertToNative(string path)
    {
        return path.Replace(Path.DirectorySeparatorChar == '\\'
                                ? '/'
                                : '\\',
                            newChar: Path.DirectorySeparatorChar);
    }
}