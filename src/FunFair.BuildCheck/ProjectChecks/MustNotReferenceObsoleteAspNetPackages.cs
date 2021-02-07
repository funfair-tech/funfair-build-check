using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks
{
    /// <summary>
    ///     Checks that obsolete packages aren't referenced.
    /// </summary>    [SuppressMessage(category: "ReSharper", checkId: "ClassNeverInstantiated.Global", Justification = "Created by DI")]
    public sealed class MustNotReferenceObsoleteAspNetPackages : MustNotReferencePackages
    {
        private static readonly IReadOnlyList<string> PackageIds = new[]
                                                                   {
                                                                       "Microsoft.AspNetCore",
                                                                       "Microsoft.AspNetCore.All",
                                                                       "Microsoft.AspNetCore.App",
                                                                       "Microsoft.AspNetCore.Antiforgery",
                                                                       "Microsoft.AspNetCore.Authentication",
                                                                       "Microsoft.AspNetCore.Authentication.Abstractions",
                                                                       "Microsoft.AspNetCore.Authentication.Cookies",
                                                                       "Microsoft.AspNetCore.Authentication.Core",

                                                                       //"Microsoft.AspNetCore.Authentication.JwtBearer",
                                                                       "Microsoft.AspNetCore.Authentication.OAuth",
                                                                       "Microsoft.AspNetCore.Authentication.OpenIdConnect",
                                                                       "Microsoft.AspNetCore.Authorization",
                                                                       "Microsoft.AspNetCore.Authorization.Policy",
                                                                       "Microsoft.AspNetCore.CookiePolicy",
                                                                       "Microsoft.AspNetCore.Cors",
                                                                       "Microsoft.AspNetCore.Cryptography.Internal",
                                                                       "Microsoft.AspNetCore.Cryptography.KeyDerivation",
                                                                       "Microsoft.AspNetCore.DataProtection",
                                                                       "Microsoft.AspNetCore.DataProtection.Abstractions",
                                                                       "Microsoft.AspNetCore.DataProtection.Extensions",
                                                                       "Microsoft.AspNetCore.Diagnostics",
                                                                       "Microsoft.AspNetCore.Diagnostics.HealthChecks",
                                                                       "Microsoft.AspNetCore.HostFiltering",
                                                                       "Microsoft.AspNetCore.Hosting",
                                                                       "Microsoft.AspNetCore.Hosting.Abstractions",
                                                                       "Microsoft.AspNetCore.Hosting.Server.Abstractions",
                                                                       "Microsoft.AspNetCore.Http",
                                                                       "Microsoft.AspNetCore.Http.Abstractions",
                                                                       "Microsoft.AspNetCore.Http.Connections",
                                                                       "Microsoft.AspNetCore.Http.Extensions",
                                                                       "Microsoft.AspNetCore.Http.Features",
                                                                       "Microsoft.AspNetCore.HttpOverrides",
                                                                       "Microsoft.AspNetCore.HttpsPolicy",
                                                                       "Microsoft.AspNetCore.Identity",
                                                                       "Microsoft.AspNetCore.Localization",
                                                                       "Microsoft.AspNetCore.Localization.Routing",
                                                                       "Microsoft.AspNetCore.MiddlewareAnalysis",
                                                                       "Microsoft.AspNetCore.Mvc",
                                                                       "Microsoft.AspNetCore.Mvc.Abstractions",
                                                                       "Microsoft.AspNetCore.Mvc.Analyzers",
                                                                       "Microsoft.AspNetCore.Mvc.ApiExplorer",
                                                                       "Microsoft.AspNetCore.Mvc.Api.Analyzers",
                                                                       "Microsoft.AspNetCore.Mvc.Core",
                                                                       "Microsoft.AspNetCore.Mvc.Cors",
                                                                       "Microsoft.AspNetCore.Mvc.DataAnnotations",
                                                                       "Microsoft.AspNetCore.Mvc.Formatters.Json",
                                                                       "Microsoft.AspNetCore.Mvc.Formatters.Xml",
                                                                       "Microsoft.AspNetCore.Mvc.Localization",
                                                                       "Microsoft.AspNetCore.Mvc.Razor",
                                                                       "Microsoft.AspNetCore.Mvc.Razor.Extensions",
                                                                       "Microsoft.AspNetCore.Mvc.Razor.ViewCompilation",
                                                                       "Microsoft.AspNetCore.Mvc.RazorPages",
                                                                       "Microsoft.AspNetCore.Mvc.TagHelpers",
                                                                       "Microsoft.AspNetCore.Mvc.ViewFeatures",
                                                                       "Microsoft.AspNetCore.Razor",
                                                                       "Microsoft.AspNetCore.Razor.Runtime",
                                                                       "Microsoft.AspNetCore.Razor.Design",
                                                                       "Microsoft.AspNetCore.ResponseCaching",
                                                                       "Microsoft.AspNetCore.ResponseCaching.Abstractions",
                                                                       "Microsoft.AspNetCore.ResponseCompression",
                                                                       "Microsoft.AspNetCore.Rewrite",
                                                                       "Microsoft.AspNetCore.Routing",
                                                                       "Microsoft.AspNetCore.Routing.Abstractions",
                                                                       "Microsoft.AspNetCore.Server.HttpSys",
                                                                       "Microsoft.AspNetCore.Server.IIS",
                                                                       "Microsoft.AspNetCore.Server.IISIntegration",
                                                                       "Microsoft.AspNetCore.Server.Kestrel",
                                                                       "Microsoft.AspNetCore.Server.Kestrel.Core",
                                                                       "Microsoft.AspNetCore.Server.Kestrel.Https",
                                                                       "Microsoft.AspNetCore.Server.Kestrel.Transport.Abstractions",
                                                                       "Microsoft.AspNetCore.Server.Kestrel.Transport.Sockets",
                                                                       "Microsoft.AspNetCore.Session",
                                                                       "Microsoft.AspNetCore.SignalR",
                                                                       "Microsoft.AspNetCore.SignalR.Core",
                                                                       "Microsoft.AspNetCore.StaticFiles",
                                                                       "Microsoft.AspNetCore.WebSockets",
                                                                       "Microsoft.AspNetCore.WebUtilities",
                                                                       "Microsoft.Net.Http.Headers"
                                                                   };

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="logger">Logging.</param>
        public MustNotReferenceObsoleteAspNetPackages(ILogger<MustNotReferenceObsoleteAspNetPackages> logger)
            : base(packageIds: PackageIds, reason: "Obsoleted with .net core 3.1", logger: logger)
        {
        }
    }
}
