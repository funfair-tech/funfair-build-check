using System;
using System.Collections.Generic;
using System.Xml;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks
{
    public abstract class MustNotReferencePackages : IProjectCheck
    {
        private readonly ILogger<ErrorPolicyWarningAsErrors> _logger;
        private readonly IReadOnlyList<string> _packageIds;
        private readonly string _reason;

        protected MustNotReferencePackages(IReadOnlyList<string> packageIds, string reason, ILogger<ErrorPolicyWarningAsErrors> logger)
        {
            this._packageIds = packageIds;
            this._reason = reason;
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public void Check(string projectName, XmlDocument project)
        {
            foreach (string packageId in this._packageIds)
            {
                bool packageExists = CheckReference(packageId, project);

                if (packageExists)
                {
                    this._logger.LogError($"{projectName}: References obsoleted package {packageId} using NuGet. {this._reason}");
                }
            }
        }

        private static bool CheckReference(string packageId, XmlDocument project)
        {
            XmlElement? reference = project.SelectSingleNode("/Project/ItemGroup/PackageReference[@Include='" + packageId + "']") as XmlElement;

            return reference != null;
        }
    }
}