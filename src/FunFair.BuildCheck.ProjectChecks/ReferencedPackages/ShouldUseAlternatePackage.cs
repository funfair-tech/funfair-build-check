using System;
using System.Linq;
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

    public void Check(string projectName, string projectFolder, XmlDocument project)
    {
        string outputType = project.GetOutputType();

        if (StringComparer.InvariantCultureIgnoreCase.Equals(x: outputType, y: "Exe"))
        {
            // Executables can use whatever they want.
            return;
        }

        string? awsProjectType = project.GetAwsProjectType();

        if (awsProjectType != null && StringComparer.InvariantCultureIgnoreCase.Equals(x: awsProjectType, y: "Lambda"))
        {
            // Lambdas are effectively executables so can use whatever they want.
            return;
        }

        if (this.ShouldExclude(project: project, projectName: projectName, logger: this._logger))
        {
            // Test projects can use whatever they want.
            return;
        }

        XmlNodeList? referenceNodes = project.SelectNodes("/Project/ItemGroup/PackageReference");

        if (referenceNodes == null)
        {
            return;
        }

        foreach (XmlElement referenceNode in referenceNodes.OfType<XmlElement>())
        {
            string packageId = referenceNode.GetAttribute(@"Include");

            if (StringComparer.InvariantCultureIgnoreCase.Equals(x: packageId, y: this._matchPackageId))
            {
                this._logger.LogError($"Should use package {this._usePackageId} rather than {this._matchPackageId}");
            }
        }
    }

    protected virtual bool ShouldExclude(XmlDocument project, string projectName, ILogger logger)
    {
        return false;
    }
}