using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

/// <summary>
///     Checks that the &quot;RepositoryUrl&quot; property is set in the project.
/// </summary>
[SuppressMessage(category: "ReSharper", checkId: "ClassNeverInstantiated.Global", Justification = "Created by DI")]
public sealed class RepositoryUrlMetadata : PackableMetadataBase
{
    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="logger">Logging.</param>
    public RepositoryUrlMetadata(ILogger<RepositoryUrlMetadata> logger)
        : base(property: @"RepositoryUrl", logger: logger)
    {
    }
}