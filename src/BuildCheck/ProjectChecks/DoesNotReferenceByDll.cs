using System;
using System.Xml;
using Microsoft.Extensions.Logging;

namespace BuildCheck.ProjectChecks
{
    public class DoesNotReferenceByDll : IProjectCheck
    {
        private readonly ILogger<ErrorPolicyWarningAsErrors> _logger;

        public DoesNotReferenceByDll(ILogger<ErrorPolicyWarningAsErrors> logger)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Check(string projectName, XmlDocument project)
        {
            XmlNodeList references = project.SelectNodes(xpath: "/Project/ItemGroup/Reference");

            foreach (XmlElement reference in references)
            {
                string assembly = reference.GetAttribute(name: "Reference");
                this._logger.LogError($"{projectName}: References {assembly} directly not using NuGet or a project reference.");
            }
        }
    }
}