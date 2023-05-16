using System;
using System.IO;
using System.Text.Json;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.SolutionChecks.Models;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.SolutionChecks;

public sealed class GlobalJsonMustSpecifyCorrectRollforwardPolicy : ISolutionCheck
{
    private const string ROLL_FORWARD_POLICY = @"latestPatch";
    private readonly ILogger<GlobalJsonMustSpecifyCorrectRollforwardPolicy> _logger;
    private readonly IRepositorySettings _repositorySettings;

    public GlobalJsonMustSpecifyCorrectRollforwardPolicy(IRepositorySettings repositorySettings, ILogger<GlobalJsonMustSpecifyCorrectRollforwardPolicy> logger)
    {
        this._repositorySettings = repositorySettings;
        this._logger = logger;
    }

    public void Check(string solutionFileName)
    {
        if (string.IsNullOrWhiteSpace(this._repositorySettings.DotNetSdkVersion))
        {
            return;
        }

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