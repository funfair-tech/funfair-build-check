using System;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.SolutionChecks.Helpers;
using FunFair.BuildCheck.SolutionChecks.LoggingExtensions;
using FunFair.BuildCheck.SolutionChecks.Models;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.SolutionChecks;

public sealed class GlobalJsonIsLatest : GlobalJsonSolutionCheckBase
{
    private readonly ILogger<GlobalJsonIsLatest> _logger;

    public GlobalJsonIsLatest(
        IRepositorySettings repositorySettings,
        IGlobalJsonLoader loader,
        ILogger<GlobalJsonIsLatest> logger
    )
        : base(repositorySettings: repositorySettings, loader: loader)
    {
        this._logger = logger;
    }

    protected override void OnNotConfigured(string solutionFileName)
    {
        this._logger.NotCheckingGlobalJson(solutionFileName);
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
        if (!string.IsNullOrWhiteSpace(info.SdkVersion))
        {
            if (!StringComparer.OrdinalIgnoreCase.Equals(x: info.SdkVersion, y: this.DotNetSdkVersion))
            {
                this._logger.UsingIncorrectDotNetSdkVersion(
                    solutionFileName: solutionFileName,
                    projectVersion: info.SdkVersion,
                    dotnetVersion: this.DotNetSdkVersion
                );
            }
        }
        else
        {
            this._logger.DoesNotSpecifyADotNetSdkVersion(solutionFileName);
        }
    }
}
