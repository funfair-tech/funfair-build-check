using System.Threading;
using System.Threading.Tasks;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using FunFair.BuildCheck.ProjectChecks.Settings.LoggingExtensions;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class TestProjectsMustImportUnitTestsProps : IProjectCheck
{
    private const string UnitTestsPropsImport = "$(SolutionDir)UnitTests.props";

    private readonly ILogger<TestProjectsMustImportUnitTestsProps> _logger;

    public TestProjectsMustImportUnitTestsProps(ILogger<TestProjectsMustImportUnitTestsProps> logger)
    {
        this._logger = logger;
    }

    public ValueTask CheckAsync(ProjectContext project, CancellationToken cancellationToken)
    {
        if (!project.IsTestProject(this._logger))
        {
            return ValueTask.CompletedTask;
        }

        if (project.IsExplicitlyNotTestProject())
        {
            return ValueTask.CompletedTask;
        }

        if (!project.HasProjectImport(UnitTestsPropsImport))
        {
            this._logger.TestProjectShouldImportUnitTestsProps(
                projectName: project.Name,
                projectImport: UnitTestsPropsImport
            );
        }

        return ValueTask.CompletedTask;
    }
}
