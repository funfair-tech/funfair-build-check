using System;
using System.IO;
using System.Text.Json;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.SolutionChecks.Models;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.SolutionChecks;

public sealed class GlobalJsonIsLatest : ISolutionCheck
{
    private readonly string? _dotnetVersion;
    private readonly ILogger<GlobalJsonIsLatest> _logger;

    public GlobalJsonIsLatest(IRepositorySettings repositorySettings, ILogger<GlobalJsonIsLatest> logger)
    {
        this._logger = logger;
        this._dotnetVersion = repositorySettings.DotNetSdkVersion;
    }

    public void Check(string solutionFileName)
    {
        if (string.IsNullOrWhiteSpace(this._dotnetVersion))
        {
            this._logger.LogInformation(message: "Not checking global.json as DOTNET_CORE_SDK_VERSION is not defined");

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
                if (!StringComparer.InvariantCultureIgnoreCase.Equals(x: p.Sdk.Version, y: this._dotnetVersion))
                {
                    this._logger.LogError($"global.json is using SDK {p.Sdk.Version} rather than {this._dotnetVersion}");
                }
            }
            else
            {
                this._logger.LogError(message: "global.json does not specify a SDK version");
            }
        }
        catch (Exception exception)
        {
            this._logger.LogError(new(exception.HResult), exception: exception, $"Failed to read {file} : {exception.Message}");
        }
    }
}