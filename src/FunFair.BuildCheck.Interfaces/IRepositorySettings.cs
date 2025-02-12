namespace FunFair.BuildCheck.Interfaces;

public interface IRepositorySettings : IFrameworkSettings
{
    public bool IsCodeAnalysisSolution { get; }

    public bool IsUnitTestBase { get; }

    bool MustHaveEnumSourceGeneratorAnalyzerPackage { get; }
}
