namespace FunFair.BuildCheck.Interfaces;

public interface IRepositorySettings
{
    public bool IsCodeAnalysisSolution { get; }

    public bool IsNullableGloballyEnforced { get; }

    public bool IsUnitTestBase { get; }
}