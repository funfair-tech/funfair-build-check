using System.Text.Json.Serialization;

namespace FunFair.BuildCheck.SolutionChecks.Models;

internal sealed class GlobalJsonPacket
{
    [JsonConstructor]
    public GlobalJsonPacket(GlobalJsonSdk? sdk)
    {
        this.Sdk = sdk;
    }

    public GlobalJsonSdk? Sdk { get; set; }
}