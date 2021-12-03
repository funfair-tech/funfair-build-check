namespace FunFair.BuildCheck.Interfaces;

/// <summary>
///     A Solution Check.
/// </summary>
public interface ISolutionCheck
{
    /// <summary>
    ///     Performs the check on the solution at <paramref name="solutionFileName" />
    /// </summary>
    /// <param name="solutionFileName">The Solution to check.</param>
    void Check(string solutionFileName);
}