using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.ReferencedPackages;

public sealed class ShouldUseCredfetoVersionInformationGeneratorRatherThanThisAssemblyAssemblyInfo
    : ShouldUseAlternatePackage
{
    public ShouldUseCredfetoVersionInformationGeneratorRatherThanThisAssemblyAssemblyInfo(
        ILogger<ShouldUseCredfetoVersionInformationGeneratorRatherThanThisAssemblyAssemblyInfo> logger
    )
        : base(
            matchPackageId: "ThisAssembly.AssemblyInfo",
            usePackageId: "Credfeto.Version.Information.Generator",
            logger: logger
        ) { }
}
