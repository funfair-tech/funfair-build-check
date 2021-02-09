using System;
using System.Diagnostics.CodeAnalysis;
using System.Xml;
using FunFair.BuildCheck.Interfaces;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks
{
    /// <summary>
    ///     Checks that the FxCop analyzer is installed.
    /// </summary>
    [SuppressMessage(category: "ReSharper", checkId: "ClassNeverInstantiated.Global", Justification = "Created by DI")]
    public sealed class MustHaveFxCopAnalyzerPackage : IProjectCheck
    {
        private const string PACKAGE_ID = @"Microsoft.CodeAnalysis.FxCopAnalyzers";

        private const string PACKAGE_PRIVATE_ASSETS = @"All";

        private const string RULESET_FILENAME = @"$(SolutionDir)\CodeAnalysis.ruleset";

        private readonly ILogger<MustHaveFxCopAnalyzerPackage> _logger;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="logger">Logging.</param>
        public MustHaveFxCopAnalyzerPackage(ILogger<MustHaveFxCopAnalyzerPackage> logger)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public void Check(string projectName, string projectFolder, XmlDocument project)
        {
            if (this.CheckPackageReference(projectName: projectName, project: project))
            {
                return;
            }

            this.CheckRuleSet(projectName: projectName, project: project);
        }

        private void CheckRuleSet(string projectName, XmlDocument project)
        {
            XmlElement? codeAnalysisRuleSet = project.SelectSingleNode(xpath: "/Project/PropertyGroup/CodeAnalysisRuleSet") as XmlElement;

            if (codeAnalysisRuleSet == null)
            {
                this._logger.LogError($"{projectName}: Does not reference {RULESET_FILENAME} as the defined ruleset.");

                return;
            }

            string filename = codeAnalysisRuleSet.InnerText;

            if (!StringComparer.InvariantCultureIgnoreCase.Equals(x: filename, y: RULESET_FILENAME))
            {
                this._logger.LogError($"{projectName}: Does not reference {RULESET_FILENAME} as the defined ruleset.");
            }
        }

        private bool CheckPackageReference(string projectName, XmlDocument project)
        {
            XmlElement? reference = project.SelectSingleNode("/Project/ItemGroup/PackageReference[@Include='" + PACKAGE_ID + "']") as XmlElement;

            if (reference == null)
            {
                this._logger.LogError($"{projectName}: Does not reference {PACKAGE_ID} not using NuGet");

                return true;
            }

            string assets = reference.GetAttribute(name: "PrivateAssets");

            if (string.IsNullOrWhiteSpace(assets) || assets != PACKAGE_PRIVATE_ASSETS)
            {
                this._logger.LogError($"{projectName}: Does not reference {PACKAGE_ID} with a PrivateAssets=\"{PACKAGE_PRIVATE_ASSETS}\" attribute");
            }

            return false;
        }
    }
}