using System;
using System.Xml;
using Microsoft.Extensions.Logging;

namespace BuildCheck.ProjectChecks
{
    public class MustHaveFxCopAnalyzerPackate : IProjectCheck
    {
        //<PackageReference Include="Microsoft.CodeAnalysis.FxCopAnalyzers" Version="2.6.1" PrivateAssets="All"/>
        private const string PACKAGE_ID = @"Microsoft.CodeAnalysis.FxCopAnalyzers";

        //private const string PACKAGE_VERSION = @"2.8.3";
        private const string PACKAGE_PRIVATE_ASSETS = @"All";

        private readonly ILogger<ErrorPolicyWarningAsErrors> _logger;

        public MustHaveFxCopAnalyzerPackate(ILogger<ErrorPolicyWarningAsErrors> logger)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public void Check(string projectName, XmlDocument project)
        {
            XmlElement reference = project.SelectSingleNode("/Project/ItemGroup/PackageReference[@Include='" + PACKAGE_ID + "']") as XmlElement;

            if (reference == null)
            {
                this._logger.LogWarning($"{projectName}: Does not reference {PACKAGE_ID} directly not using NuGet");
                return;
            }

            string assets = reference.GetAttribute(name: "PrivateAssets");

            if (string.IsNullOrWhiteSpace(assets) || assets != PACKAGE_PRIVATE_ASSETS)
            {
                this._logger.LogWarning($"{projectName}: Does not reference {PACKAGE_ID} with a PrivateAssets=\"{PACKAGE_PRIVATE_ASSETS}\" attribute");
            }
        }
    }
}