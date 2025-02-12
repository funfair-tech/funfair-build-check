using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public abstract class SimplePropertyProjectCheckBase : IProjectCheck
{
    private readonly ILogger _logger;
    private readonly string _propertyName;
    private readonly string _requiredValue;

    protected SimplePropertyProjectCheckBase(string propertyName, string requiredValue, ILogger logger)
    {
        this._propertyName = propertyName;
        this._requiredValue = requiredValue;
        this._logger = logger;
    }

    public ValueTask CheckAsync(string projectName, string projectFolder, XmlDocument project, CancellationToken cancellationToken)
    {
        this.DoCheck(projectName: projectName, projectFolder: projectFolder, project: project);

        return ValueTask.CompletedTask;
    }

    private void DoCheck(string projectName, string projectFolder, XmlDocument project)
    {
        if (this.CanCheck(projectName: projectName, projectFolder: projectFolder, project: project))
        {
            ProjectValueHelpers.CheckValue(projectName: projectName, project: project, nodePresence: this._propertyName, requiredValue: this._requiredValue, logger: this._logger);
        }
    }

    protected virtual bool CanCheck(string projectName, string projectFolder, XmlDocument project)
    {
        return true;
    }
}
