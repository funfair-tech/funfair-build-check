using System.IO;

namespace FunFair.BuildCheck.Helpers
{
    /// <summary>
    ///     Helpers for working with paths
    /// </summary>
    public static class PathHelpers
    {
        /// <summary>
        ///     Converts the path to the native format.
        /// </summary>
        /// <param name="path">The path to convert.</param>
        /// <returns>The native format path.</returns>
        public static string ConvertToNative(string path)
        {
            if (Path.PathSeparator == '\\')
            {
                return path.Replace(oldChar: '/', newChar: Path.PathSeparator);
            }

            return path.Replace(oldChar: '\\', newChar: Path.PathSeparator);
        }
    }
}