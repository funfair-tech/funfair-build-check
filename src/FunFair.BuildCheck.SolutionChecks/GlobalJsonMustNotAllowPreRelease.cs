using System;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.SolutionChecks.Helpers;
using FunFair.BuildCheck.SolutionChecks.LoggingExtensions;
using FunFair.BuildCheck.SolutionChecks.Models;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.SolutionChecks;

public sealed class GlobalJsonMustNotAllowPreRelease : GlobalJsonSolutionCheckBase
{
    private readonly bool _allowPreRelease;
    private readonly ILogger<GlobalJsonMustNotAllowPreRelease> _logger;

    public GlobalJsonMustNotAllowPreRelease(
        IRepositorySettings repositorySettings,
        IGlobalJsonLoader loader,
        ILogger<GlobalJsonMustNotAllowPreRelease> logger
    )
        : base(repositorySettings: repositorySettings, loader: loader)
    {
        this._logger = logger;
        this._allowPreRelease = StringComparer.OrdinalIgnoreCase.Equals(
            x: repositorySettings.DotNetAllowPreReleaseSdk,
            y: "true"
        );
    }

    protected override void OnReadFailed(string solutionFileName, string file, Exception exception)
    {
        this._logger.FailedToReadGlobalJson(
            solutionFileName: solutionFileName,
            file: file,
            message: exception.Message,
            exception: exception
        );
    }

    protected override void CheckSdk(string solutionFileName, GlobalJsonInfo info)
    {
        bool? allowPrereleaseInConfig = info.AllowPrerelease;

        if (allowPrereleaseInConfig is null)
        {
            this._logger.DoesNotSpecifyADotNetSdkPreReleasePolicy(solutionFileName);

            return;
        }

        if (!this._allowPreRelease && allowPrereleaseInConfig.Value)
        {
            this._logger.UsingIncorrectPreReleasePolicy(
                solutionFileName: solutionFileName,
                FormatPolicy(allowPrereleaseInConfig.Value),
                FormatPolicy(this._allowPreRelease)
            );
        }
    }

    private static string FormatPolicy(bool preReleaseAllowed)
    {
        return preReleaseAllowed ? "Pre-Release Allowed" : "Production Only";
    }
}
