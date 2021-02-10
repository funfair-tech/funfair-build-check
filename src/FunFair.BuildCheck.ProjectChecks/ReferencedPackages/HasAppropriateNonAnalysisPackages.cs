using System;
using System.Linq;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages
{
    /// <summary>
    ///     Checks that appropriate packages (non-analysis) are enabled if the package that is being analyzed is installed.
    /// </summary>
    public abstract class HasAppropriateNonAnalysisPackages : IProjectCheck
    {
        private readonly string _detectPackageId;
        private readonly ILogger _logger;
        private readonly string _mustIncludePackageId;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="detectPackageId">The package id used for detection.</param>
        /// <param name="mustIncludePackageId">The package id that must be included, if the <paramref name="detectPackageId" /> is installed.</param>
        /// <param name="logger">Logging.</param>
        protected HasAppropriateNonAnalysisPackages(string detectPackageId, string mustIncludePackageId, ILogger logger)
        {
            this._detectPackageId = detectPackageId ?? throw new ArgumentNullException(nameof(detectPackageId));
            this._mustIncludePackageId = mustIncludePackageId ?? throw new ArgumentNullException(nameof(mustIncludePackageId));
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public void Check(string projectName, string projectFolder, XmlDocument project)
        {
            XmlNodeList? nodes = project.SelectNodes(xpath: "/Project/ItemGroup/PackageReference");

            bool foundSourcePackage = false;
            bool foundRequiredPackage = false;

            if (nodes != null)
            {
                foreach (XmlElement reference in nodes.OfType<XmlElement>())
                {
                    string packageName = reference.GetAttribute(name: @"Include");

                    if (string.IsNullOrWhiteSpace(packageName))
                    {
                        this._logger.LogError($"{projectName}: Contains bad reference to packages.");

                        continue;
                    }

                    if (StringComparer.InvariantCultureIgnoreCase.Equals(x: this._detectPackageId, y: packageName))
                    {
                        foundSourcePackage = true;
                    }

                    if (StringComparer.InvariantCultureIgnoreCase.Equals(x: this._mustIncludePackageId, y: packageName))
                    {
                        foundRequiredPackage = true;
                    }
                }
            }

            if (foundSourcePackage && !foundRequiredPackage)
            {
                this._logger.LogError($"{projectName}: Found {this._detectPackageId} but did not find {this._mustIncludePackageId}.");
            }
        }
    }
}