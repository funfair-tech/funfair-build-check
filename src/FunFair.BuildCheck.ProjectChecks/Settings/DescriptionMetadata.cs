using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings
{
    /// <summary>
    ///     Checks that the &quot;Description&quot; property is set in the project.
    /// </summary>
    [SuppressMessage(category: "ReSharper", checkId: "ClassNeverInstantiated.Global", Justification = "Created by DI")]
    public sealed class DescriptionMetadata : PackableMetadataBase
    {
        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="logger">Logging.</param>
        public DescriptionMetadata(ILogger<DescriptionMetadata> logger)
            : base(property: @"Description", logger: logger)
        {
        }
    }
}