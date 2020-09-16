using System;
using System.Xml;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks
{
    public abstract class ShouldUseAbstractionsPackage : IProjectCheck
    {
        private readonly ILogger _logger;
        private readonly string _matchPackageId;
        private readonly string _usePackageId;

        protected ShouldUseAbstractionsPackage(string matchPackageId, string usePackageId, ILogger logger)
        {
            this._matchPackageId = matchPackageId ?? throw new ArgumentNullException(nameof(matchPackageId));
            this._usePackageId = usePackageId ?? throw new ArgumentNullException(nameof(usePackageId));
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public void Check(string projectName, XmlDocument project)
        {
            string outputType = project.GetOutputType();

            if (StringComparer.InvariantCultureIgnoreCase.Equals(x: outputType, y: "Exe"))
            {
                // Executables can use whatever they want.
                return;
            }

            XmlNodeList? referenceNodes = project.SelectNodes("/Project/ItemGroup/PackageReference");

            foreach (XmlElement? referenceNode in referenceNodes)
            {
                if (referenceNode == null)
                {
                    continue;
                }

                string packageId = referenceNode.GetAttribute(@"Include");

                if (StringComparer.InvariantCultureIgnoreCase.Equals(x: packageId, y: this._matchPackageId))
                {
                    this._logger.LogError($"Should use package {this._usePackageId} rather than {this._matchPackageId}");
                }
            }
        }
    }
}