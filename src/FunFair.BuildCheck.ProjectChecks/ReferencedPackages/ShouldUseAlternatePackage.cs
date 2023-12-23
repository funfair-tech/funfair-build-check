using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

public abstract class ShouldUseAlternatePackage : IProjectCheck
{
    private readonly ILogger _logger;
    private readonly string _matchPackageId;
    private readonly string _usePackageId;

    protected ShouldUseAlternatePackage(string matchPackageId, string usePackageId, ILogger logger)
    {
        this._matchPackageId = matchPackageId;
        this._usePackageId = usePackageId;
        this._logger = logger;
    }

    public ValueTask CheckAsync(string projectName, string projectFolder, XmlDocument project, CancellationToken cancellationToken)
    {
        string outputType = project.GetOutputType();

        if (StringComparer.InvariantCultureIgnoreCase.Equals(x: outputType, y: "Exe"))
        {
            // Executables can use whatever they want.
            return ValueTask.CompletedTask;
        }

        string? awsProjectType = project.GetAwsProjectType();

        if (awsProjectType is not null && StringComparer.InvariantCultureIgnoreCase.Equals(x: awsProjectType, y: "Lambda"))
        {
            // Lambdas are effectively executables so can use whatever they want.
            return ValueTask.CompletedTask;
        }

        if (this.ShouldExclude(project: project, projectName: projectName, logger: this._logger))
        {
            // Test projects can use whatever they want.
            return ValueTask.CompletedTask;
        }

        XmlNodeList? referenceNodes = project.SelectNodes("/Project/ItemGroup/PackageReference");

        if (referenceNodes is null)
        {
            return ValueTask.CompletedTask;
        }

        foreach (XmlElement referenceNode in referenceNodes.OfType<XmlElement>())
        {
            string packageId = referenceNode.GetAttribute("Include");

            if (StringComparer.InvariantCultureIgnoreCase.Equals(x: packageId, y: this._matchPackageId))
            {
                this._logger.LogError($"Should use package {this._usePackageId} rather than {this._matchPackageId}");
            }
        }

        return ValueTask.CompletedTask;
    }

    protected virtual bool ShouldExclude(XmlDocument project, string projectName, ILogger logger)
    {
        return false;
    }
}