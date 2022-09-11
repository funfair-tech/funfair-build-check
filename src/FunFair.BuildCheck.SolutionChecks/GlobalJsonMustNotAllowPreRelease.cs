using System;
using System.IO;
using System.Text.Json;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.SolutionChecks.Models;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.SolutionChecks;

/// <summary>
///     Checks the global.json pre-release settings.
/// </summary>
public sealed class GlobalJsonMustNotAllowPreRelease : ISolutionCheck
{
    private readonly bool _allowPreRelease;

    private readonly ILogger<GlobalJsonMustNotAllowPreRelease> _logger;

    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="logger">Logging.</param>
    public GlobalJsonMustNotAllowPreRelease(ILogger<GlobalJsonMustNotAllowPreRelease> logger)
    {
        this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        string allowPreRelease = Environment.GetEnvironmentVariable("DOTNET_SDK_ALLOW_PRE_RELEASE") ?? "false";

        this._allowPreRelease = StringComparer.InvariantCultureIgnoreCase.Equals(x: allowPreRelease, y: "true");
    }

    /// <inheritdoc />
    public void Check(string solutionFileName)
    {
        string? solutionDir = Path.GetDirectoryName(solutionFileName);

        if (solutionDir == null)
        {
            return;
        }

        string file = Path.Combine(path1: solutionDir, path2: @"global.json");

        if (!File.Exists(file))
        {
            return;
        }

        string content = File.ReadAllText(file);

        try
        {
            GlobalJsonPacket? p = JsonSerializer.Deserialize(json: content, jsonTypeInfo: MustBeSerializable.Default.GlobalJsonPacket);

            if (p?.Sdk?.AllowPrerelease != null)
            {
                bool preReleaseAllowedInConfig = p.Sdk.AllowPrerelease.GetValueOrDefault(defaultValue: true);

                if (!this._allowPreRelease && preReleaseAllowedInConfig)
                {
                    this._logger.LogError($"global.json is using SDK pre-release policy of {FormatPolicy(preReleaseAllowedInConfig)} rather than {FormatPolicy(this._allowPreRelease)}");
                }
            }
            else
            {
                this._logger.LogError(message: "global.json does not specify a SDK pre-release policy");
            }
        }
        catch (Exception exception)
        {
            this._logger.LogError(new(exception.HResult), exception: exception, $"Failed to read {file} : {exception.Message}");
        }
    }

    private static string FormatPolicy(bool preReleaseAllowed)
    {
        return preReleaseAllowed
            ? "Pre-Release Allowed"
            : "Production Only";
    }
}