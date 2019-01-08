using System;
using System.Xml;
using Microsoft.Extensions.Logging;

namespace BuildCheck.ProjectChecks
{
    public class MustHaveSourceLinkPackage : IProjectCheck
    {
        private const string HISTORICAL_PACKAGE_ID = @"SourceLink.Create.CommandLine";
        private const string PACKAGE_ID = @"Microsoft.SourceLink.GitHub";
        private const string PACKAGE_PRIVATE_ASSETS = @"All";

        private readonly ILogger<ErrorPolicyWarningAsErrors> _logger;

        public MustHaveSourceLinkPackage(ILogger<ErrorPolicyWarningAsErrors> logger)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public void Check(string projectName, XmlDocument project)
        {
            // check for xunit reference
            XmlElement xunitReference = project.SelectSingleNode("/Project/ItemGroup/PackageReference[@Include='xunit']") as XmlElement;
            if (xunitReference != null)
            {
                // has an xunit reference so is a unit test project, don't force sourcelink
                return;
            }

            bool packageExists = CheckReference(PACKAGE_ID, project);
            bool historicalPackageExists = CheckReference(HISTORICAL_PACKAGE_ID, project);

            if (!packageExists && !historicalPackageExists)
            {
                this._logger.LogError($"{projectName}: Does not reference {PACKAGE_ID} or {HISTORICAL_PACKAGE_ID} directly not using NuGet");
            }

            if (packageExists && historicalPackageExists)
            {
                this._logger.LogError($"{projectName}: References both {PACKAGE_ID} and {HISTORICAL_PACKAGE_ID}");
            }

            if (packageExists)
            {
                if (!CheckPrivateAssets(PACKAGE_ID, project))
                {
                    this._logger.LogError($"{projectName}: Does not reference {PACKAGE_ID} with a PrivateAssets=\"{PACKAGE_PRIVATE_ASSETS}\" attribute");
                }
            }

            if (historicalPackageExists)
            {
                if (!CheckPrivateAssets(HISTORICAL_PACKAGE_ID, project))
                {
                    this._logger.LogError($"{projectName}: Does not reference {HISTORICAL_PACKAGE_ID} with a PrivateAssets=\"{PACKAGE_PRIVATE_ASSETS}\" attribute");
                }
            }
        }

        private static bool CheckReference(string packageId, XmlDocument project)
        {
            XmlElement reference = project.SelectSingleNode("/Project/ItemGroup/PackageReference[@Include='" + packageId + "']") as XmlElement;
            return reference != null;
        }

        private static bool CheckPrivateAssets(string packageId, XmlDocument project)
        {
            XmlElement reference = project.SelectSingleNode("/Project/ItemGroup/PackageReference[@Include='" + packageId + "']") as XmlElement;
            if (reference == null) return false;

            // check for an attribute
            string assets = reference.GetAttribute(name: "PrivateAssets");
            if (string.IsNullOrEmpty(assets))
            {
                // no PrivateAssets attribute, check for an element
                XmlElement privateAssets = reference.SelectSingleNode("PrivateAssets") as XmlElement;
                if (privateAssets == null) return false;

                assets = privateAssets.InnerText;
            }

            return !string.IsNullOrEmpty(assets) && string.Compare(assets, PACKAGE_PRIVATE_ASSETS, StringComparison.OrdinalIgnoreCase) == 0;
        }
    }
}