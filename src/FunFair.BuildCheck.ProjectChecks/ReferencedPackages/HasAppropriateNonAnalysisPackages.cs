using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.ReferencedPackages.LoggingExtensions;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

public abstract class HasAppropriateNonAnalysisPackages : IProjectCheck
{
    private readonly string _detectPackageId;
    private readonly ILogger _logger;
    private readonly string _mustIncludePackageId;

    protected HasAppropriateNonAnalysisPackages(
        string detectPackageId,
        string mustIncludePackageId,
        ILogger logger
    )
    {
        this._detectPackageId = detectPackageId;
        this._mustIncludePackageId = mustIncludePackageId;
        this._logger = logger;
    }

    public ValueTask CheckAsync(ProjectContext project, CancellationToken cancellationToken)
    {
        XmlNodeList? nodes = project.CsProjXml.SelectNodes(
            xpath: "/Project/ItemGroup/PackageReference"
        );

        bool foundSourcePackage = false;
        bool foundRequiredPackage = false;

        if (nodes is not null)
        {
            foreach (XmlElement reference in nodes.OfType<XmlElement>())
            {
                string packageName = reference.GetAttribute(name: "Include");

                if (string.IsNullOrWhiteSpace(packageName))
                {
                    this._logger.ContainsBadReferenceToPackages(project.Name);

                    continue;
                }

                if (
                    StringComparer.InvariantCultureIgnoreCase.Equals(
                        x: this._detectPackageId,
                        y: packageName
                    )
                )
                {
                    foundSourcePackage = true;
                }

                if (
                    StringComparer.InvariantCultureIgnoreCase.Equals(
                        x: this._mustIncludePackageId,
                        y: packageName
                    )
                )
                {
                    foundRequiredPackage = true;
                }
            }
        }

        if (foundSourcePackage && !foundRequiredPackage)
        {
            this._logger.DidNotFindMustIncludePackageForDetectedPackage(
                projectName: project.Name,
                detectPackageId: this._detectPackageId,
                mustIncludePackageId: this._mustIncludePackageId
            );
        }

        return ValueTask.CompletedTask;
    }
}
