using System;
using System.Xml;
using Microsoft.Extensions.Logging;

namespace BuildCheck.ProjectChecks
{
    public class MustHaveFxCopAnalyzerPackage : IProjectCheck
    {
        //<PackageReference Include="Microsoft.CodeAnalysis.FxCopAnalyzers" Version="2.6.1" PrivateAssets="All"/>
        private const string PACKAGE_ID = @"Microsoft.CodeAnalysis.FxCopAnalyzers";

        //private const string PACKAGE_VERSION = @"2.8.3";
        private const string PACKAGE_PRIVATE_ASSETS = @"All";

        private const string RULESET_FILENAME = @"$(SolutionDir)\CodeAnalysis.ruleset";

        private readonly ILogger<ErrorPolicyWarningAsErrors> _logger;

        public MustHaveFxCopAnalyzerPackage(ILogger<ErrorPolicyWarningAsErrors> logger)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public void Check(string projectName, XmlDocument project)
        {
            if (this.CheckPackageReference(projectName, project))
            {
                return;
            }

            this.CheckRuleSet(projectName, project);
        }

        private void CheckRuleSet(string projectName, XmlDocument project)
        {
            XmlElement codeAnalysisRuleSet = project.SelectSingleNode(xpath: "/Project/PropertyGroup/CodeAnalysisRuleSet") as XmlElement;

            if (codeAnalysisRuleSet == null)
            {
                this._logger.LogError($"{projectName}: Does not reference {RULESET_FILENAME} as the defined ruleset.");

                return;
            }

            string filename = codeAnalysisRuleSet.InnerText;

            if (!StringComparer.InvariantCultureIgnoreCase.Equals(filename ?? string.Empty, RULESET_FILENAME))
            {
                this._logger.LogError($"{projectName}: Does not reference {RULESET_FILENAME} as the defined ruleset.");
            }
        }

        private bool CheckPackageReference(string projectName, XmlDocument project)
        {
            XmlElement reference = project.SelectSingleNode("/Project/ItemGroup/PackageReference[@Include='" + PACKAGE_ID + "']") as XmlElement;

            if (reference == null)
            {
                this._logger.LogInformation($"{projectName}: Does not reference {PACKAGE_ID} directly not using NuGet");

                return true;
            }

            string assets = reference.GetAttribute(name: "PrivateAssets");

            if (string.IsNullOrWhiteSpace(assets) || assets != PACKAGE_PRIVATE_ASSETS)
            {
                this._logger.LogInformation($"{projectName}: Does not reference {PACKAGE_ID} with a PrivateAssets=\"{PACKAGE_PRIVATE_ASSETS}\" attribute");
            }

            return false;
        }
    }
}