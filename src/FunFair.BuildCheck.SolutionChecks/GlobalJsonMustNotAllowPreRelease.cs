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

public sealed class GlobalJsonMustNotAllowPreRelease : ISolutionCheck
{
    private readonly bool _allowPreRelease;
    private readonly ILogger<GlobalJsonMustNotAllowPreRelease> _logger;

    private readonly IRepositorySettings _repositorySettings;

    public GlobalJsonMustNotAllowPreRelease(
        IRepositorySettings repositorySettings,
        ILogger<GlobalJsonMustNotAllowPreRelease> logger
    )
    {
        this._repositorySettings = repositorySettings;
        this._logger = logger;
        string allowPreRelease = repositorySettings.DotNetAllowPreReleaseSdk;

        this._allowPreRelease = StringComparer.InvariantCultureIgnoreCase.Equals(x: allowPreRelease, y: "true");
    }

    public async ValueTask CheckAsync(string solutionFileName, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(this._repositorySettings.DotNetSdkVersion))
        {
            return;
        }

        if (!GlobalJsonHelpers.GetFileNameForSolution(solutionFileName: solutionFileName, out string? file))
        {
            return;
        }

        string content = await File.ReadAllTextAsync(path: file, cancellationToken: cancellationToken);

        try
        {
            GlobalJsonPacket? p = JsonSerializer.Deserialize(
                json: content,
                jsonTypeInfo: MustBeSerializable.Default.GlobalJsonPacket
            );

            if (p?.Sdk?.AllowPrerelease is not null)
            {
                bool preReleaseAllowedInConfig = p.Sdk.AllowPrerelease ?? true;

                if (!this._allowPreRelease && preReleaseAllowedInConfig)
                {
                    this._logger.UsingIncorrectPreReleasePolicy(
                        solutionFileName: solutionFileName,
                        FormatPolicy(preReleaseAllowedInConfig),
                        FormatPolicy(this._allowPreRelease)
                    );
                }
            }
            else
            {
                this._logger.DoesNotSpecifyADotNetSdkPreReleasePolicy(solutionFileName);
            }
        }
        catch (Exception exception)
        {
            this._logger.FailedToReadGlobalJson(
                solutionFileName: solutionFileName,
                file: file,
                message: exception.Message,
                exception: exception
            );
        }
    }

    private static string FormatPolicy(bool preReleaseAllowed)
    {
        return preReleaseAllowed ? "Pre-Release Allowed" : "Production Only";
    }
}
