using System;
using System.IO;
using System.Text.Json;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.SolutionChecks.LoggingExtensions;
using FunFair.BuildCheck.SolutionChecks.Models;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.SolutionChecks;

public sealed class GlobalJsonMustSpecifyCorrectRollForwardPolicy : ISolutionCheck
{
    private const string ROLL_FORWARD_POLICY = "latestPatch";
    private readonly ILogger<GlobalJsonMustSpecifyCorrectRollForwardPolicy> _logger;
    private readonly IRepositorySettings _repositorySettings;

    public GlobalJsonMustSpecifyCorrectRollForwardPolicy(IRepositorySettings repositorySettings, ILogger<GlobalJsonMustSpecifyCorrectRollForwardPolicy> logger)
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

        if (solutionDir is null)
        {
            return;
        }

        string file = Path.Combine(path1: solutionDir, path2: "global.json");

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
                    this._logger.UsingIncorrectRollForwardPolicy(solutionFileName: solutionFileName, projectPolicy: p.Sdk.RollForward, expectedPolicy: ROLL_FORWARD_POLICY);
                }
            }
            else
            {
                this._logger.DoesNotSpecifyADotNetSdkRollForwardPolicy(solutionFileName);
            }
        }
        catch (Exception exception)
        {
            this._logger.FailedToReadGlobalJson(solutionFileName: solutionFileName, file: file, message: exception.Message, exception: exception);
        }
    }
}