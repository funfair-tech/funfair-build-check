using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.SolutionChecks.Helpers;
using FunFair.BuildCheck.SolutionChecks.LoggingExtensions;
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

    public async ValueTask CheckAsync(string solutionFileName, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(this._dotnetVersion))
        {
            this._logger.NotCheckingGlobalJson(solutionFileName);

            return;
        }

        if (!GlobalJsonHelpers.GetFileNameForSolution(solutionFileName: solutionFileName, out string? file))
        {
            return;
        }

        string content = await File.ReadAllTextAsync(path: file, cancellationToken: cancellationToken);

        try
        {
            GlobalJsonPacket? p = JsonSerializer.Deserialize(json: content, jsonTypeInfo: MustBeSerializable.Default.GlobalJsonPacket);

            if (!string.IsNullOrWhiteSpace(p?.Sdk?.RollForward))
            {
                if (!StringComparer.OrdinalIgnoreCase.Equals(x: p.Sdk.Version, y: this._dotnetVersion))
                {
                    this._logger.UsingIncorrectDotNetSdkVersion(solutionFileName: solutionFileName, projectVersion: p.Sdk.Version, dotnetVersion: this._dotnetVersion);
                }
            }
            else
            {
                this._logger.DoesNotSpecifyADotNetSdkVersion(solutionFileName);
            }
        }
        catch (Exception exception)
        {
            this._logger.FailedToReadGlobalJson(solutionFileName: solutionFileName, file: file, message: exception.Message, exception: exception);
        }
    }
}