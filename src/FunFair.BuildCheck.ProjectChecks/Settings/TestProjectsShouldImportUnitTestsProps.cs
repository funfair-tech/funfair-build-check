using System.Threading;
using System.Threading.Tasks;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using FunFair.BuildCheck.ProjectChecks.Settings.LoggingExtensions;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class TestProjectsShouldImportUnitTestsProps : IProjectCheck
{
    private const string UnitTestsPropsImport = "$(SolutionDir)UnitTests.props";
    private readonly ILogger<TestProjectsShouldImportUnitTestsProps> _logger;

    public TestProjectsShouldImportUnitTestsProps(ILogger<TestProjectsShouldImportUnitTestsProps> logger)
    {
        this._logger = logger;
    }

    public ValueTask CheckAsync(ProjectContext project, CancellationToken cancellationToken)
    {
        if (!project.IsTestProject(this._logger))
        {
            return ValueTask.CompletedTask;
        }

        bool found = project.HasProjectImport(UnitTestsPropsImport);

        if (!found)
        {
            this._logger.TestProjectShouldImportUnitTestsProps(
                projectName: project.Name,
                projectImport: UnitTestsPropsImport
            );
        }

        return ValueTask.CompletedTask;
    }
}
