namespace FunFair.BuildCheck.Interfaces
{
    public interface IRepositorySettings
    {
        // TODO: can this be merged with ICheckConfiguration?
        public bool IsCodeAnalysisSolution { get; }

        public bool IsNullableGloballyEnforced { get; }

        public bool IsUnitTestBase { get; }
    }
}