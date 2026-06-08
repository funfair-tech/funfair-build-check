using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public sealed class MustNotDefineWarningsNotAsErrors : MustNotDefinePropertyProjectCheckBase
{
    public MustNotDefineWarningsNotAsErrors(ILogger<MustNotDefineWarningsNotAsErrors> logger)
        : base(propertyName: "WarningsNotAsErrors", logger: logger) { }
}
