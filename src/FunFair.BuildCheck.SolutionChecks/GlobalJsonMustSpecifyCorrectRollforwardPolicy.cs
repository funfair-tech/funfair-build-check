using System;
using System.IO;
using System.Text.Json;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.SolutionChecks.Models;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.SolutionChecks;

/// <summary>
///     Checks the global.json .net core roll-forward policy.
/// </summary>
public sealed class GlobalJsonMustSpecifyCorrectRollforwardPolicy : ISolutionCheck
{
    private const string ROLL_FORWARD_POLICY = @"latestPatch";
    private readonly ILogger<GlobalJsonMustSpecifyCorrectRollforwardPolicy> _logger;

    /// <summary>
    ///     Constructor.
    /// </summary>
    /// <param name="logger">Logging.</param>
    public GlobalJsonMustSpecifyCorrectRollforwardPolicy(ILogger<GlobalJsonMustSpecifyCorrectRollforwardPolicy> logger)
    {
        this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
            GlobalJsonPacket? p = JsonSerializer.Deserialize<GlobalJsonPacket>(json: content, MustBeSerializable.Default.GlobalJsonPacket);

            if (!string.IsNullOrWhiteSpace(p?.Sdk?.RollForward))
            {
                if (!StringComparer.InvariantCulture.Equals(x: p.Sdk.RollForward, y: ROLL_FORWARD_POLICY))
                {
                    this._logger.LogError($"global.json is using SDK rollForward policy of {p.Sdk.RollForward} rather than {ROLL_FORWARD_POLICY}");
                }
            }
            else
            {
                this._logger.LogError(message: "global.json does not specify a SDK rollForward policy");
            }
        }
        catch (Exception exception)
        {
            this._logger.LogError(new(exception.HResult), exception: exception, $"Failed to read {file} : {exception.Message}");
        }
    }
}