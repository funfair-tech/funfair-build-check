using System.Collections.Generic;

namespace FunFair.BuildCheck.Interfaces;

public interface IProjectClassifier
{
    public bool IsCodeAnalysisSolution(IReadOnlyList<SolutionProject> projects);

    public bool IsUnitTestBase(IReadOnlyList<SolutionProject> projects);

    bool MustHaveEnumSourceGeneratorAnalyzerPackage(IReadOnlyList<SolutionProject> projects);
}
