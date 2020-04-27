using System;
using System.Xml;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks
{
    public sealed class DoesNotReferenceByDll : IProjectCheck
    {
        private readonly ILogger<ErrorPolicyWarningAsErrors> _logger;

        public DoesNotReferenceByDll(ILogger<ErrorPolicyWarningAsErrors> logger)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public void Check(string projectName, XmlDocument project)
        {
            XmlNodeList references = project.SelectNodes(xpath: "/Project/ItemGroup/Reference");

            foreach (XmlElement? reference in references)
            {
                if (reference == null)
                {
                    continue;
                }

                string assembly = reference.GetAttribute(name: "Include");
                this._logger.LogError($"{projectName}: References {assembly} directly not using NuGet or a project reference.");
            }
        }
    }
}