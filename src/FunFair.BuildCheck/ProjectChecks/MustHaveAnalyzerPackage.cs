using System;
using System.Xml;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks
{
    public abstract class MustHaveAnalyzerPackage : IProjectCheck
    {
        private const string PACKAGE_PRIVATE_ASSETS = @"All";
        private readonly ILogger<ErrorPolicyWarningAsErrors> _logger;

        private readonly string _packageId;

        protected MustHaveAnalyzerPackage(string packageId, ILogger<ErrorPolicyWarningAsErrors> logger)
        {
            this._packageId = packageId ?? throw new ArgumentNullException(nameof(packageId));
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public void Check(string projectName, XmlDocument project)
        {
            bool packageExists = CheckReference(packageId: this._packageId, project: project);

            if (packageExists)
            {
                if (!CheckPrivateAssets(packageId: this._packageId, project: project))
                {
                    this._logger.LogError($"{projectName}: Does not reference {this._packageId} with a PrivateAssets=\"{PACKAGE_PRIVATE_ASSETS}\" attribute");
                }
            }
            else
            {
                this._logger.LogError($"{projectName}: Does not reference {this._packageId} using NuGet");
            }
        }

        private static bool CheckReference(string packageId, XmlDocument project)
        {
            return project.SelectSingleNode("/Project/ItemGroup/PackageReference[@Include='" + packageId + "']") is XmlElement;
        }

        private static bool CheckPrivateAssets(string packageId, XmlDocument project)
        {
            XmlElement? reference = project.SelectSingleNode("/Project/ItemGroup/PackageReference[@Include='" + packageId + "']") as XmlElement;

            if (reference == null)
            {
                return false;
            }

            // check for an attribute
            string assets = reference.GetAttribute(name: "PrivateAssets");

            if (string.IsNullOrEmpty(assets))
            {
                // no PrivateAssets attribute, check for an element
                XmlElement? privateAssets = reference.SelectSingleNode(xpath: "PrivateAssets") as XmlElement;

                if (privateAssets == null)
                {
                    return false;
                }

                assets = privateAssets.InnerText;
            }

            return !string.IsNullOrEmpty(assets) && string.Compare(strA: assets, strB: PACKAGE_PRIVATE_ASSETS, comparisonType: StringComparison.OrdinalIgnoreCase) == 0;
        }
    }
}