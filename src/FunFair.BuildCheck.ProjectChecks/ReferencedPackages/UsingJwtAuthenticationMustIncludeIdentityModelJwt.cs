using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

/// <summary>
///     Checks that the System.IdentityModel.Tokens.Jwt is installed for projects that include Microsoft.AspNetCore.Authentication.JwtBearer.
/// </summary>
public sealed class UsingJwtAuthenticationMustIncludeIdentityModelJwt : MustHaveRelatedPackage
{
    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="logger">Logging.</param>
    public UsingJwtAuthenticationMustIncludeIdentityModelJwt(ILogger<UsingJwtAuthenticationMustIncludeIdentityModelJwt> logger)
        : base(detectPackageId: "Microsoft.AspNetCore.Authentication.JwtBearer", mustIncludePackageId: "System.IdentityModel.Tokens.Jwt", logger: logger)
    {
    }
}