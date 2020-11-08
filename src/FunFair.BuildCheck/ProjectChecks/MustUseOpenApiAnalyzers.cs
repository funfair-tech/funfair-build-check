using System;
using System.Diagnostics.CodeAnalysis;
using System.Xml;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks
{
    /// <summary>
    ///     Checks that Open Api Analyzers are enabled.
    /// </summary>
    [SuppressMessage(category: "ReSharper", checkId: "ClassNeverInstantiated.Global", Justification = "Created by DI")]
    public sealed class MustUseOpenApiAnalyzers : IProjectCheck
    {
        private readonly ILogger<MustUseOpenApiAnalyzers> _logger;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="logger">Logging.</param>
        public MustUseOpenApiAnalyzers(ILogger<MustUseOpenApiAnalyzers> logger)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public void Check(string projectName, string projectFolder, XmlDocument project)
        {
            ProjectValueHelpers.CheckValue(projectName: projectName, project: project, nodePresence: @"IncludeOpenAPIAnalyzers", requiredValue: "true", logger: this._logger);
        }
    }
}