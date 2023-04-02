using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

public sealed class UsingJwtAuthenticationMustIncludeIdentityModelJwt : MustHaveRelatedPackage
{
    public UsingJwtAuthenticationMustIncludeIdentityModelJwt(ILogger<UsingJwtAuthenticationMustIncludeIdentityModelJwt> logger)
        : base(detectPackageId: "Microsoft.AspNetCore.Authentication.JwtBearer", mustIncludePackageId: "System.IdentityModel.Tokens.Jwt", logger: logger)
    {
    }
}