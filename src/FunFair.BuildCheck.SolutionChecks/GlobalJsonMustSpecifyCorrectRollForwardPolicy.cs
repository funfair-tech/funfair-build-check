using System;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.SolutionChecks.Helpers;
using FunFair.BuildCheck.SolutionChecks.LoggingExtensions;
using FunFair.BuildCheck.SolutionChecks.Models;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.SolutionChecks;

public sealed class GlobalJsonMustSpecifyCorrectRollForwardPolicy : GlobalJsonSolutionCheckBase
{
    private const string ROLL_FORWARD_POLICY = "latestPatch";
    private readonly ILogger<GlobalJsonMustSpecifyCorrectRollForwardPolicy> _logger;

    public GlobalJsonMustSpecifyCorrectRollForwardPolicy(
        IRepositorySettings repositorySettings,
        IGlobalJsonLoader loader,
        ILogger<GlobalJsonMustSpecifyCorrectRollForwardPolicy> logger
    )
        : base(repositorySettings: repositorySettings, loader: loader)
    {
        this._logger = logger;
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
        if (!string.IsNullOrWhiteSpace(info.RollForward))
        {
            if (!StringComparer.Ordinal.Equals(x: info.RollForward, y: ROLL_FORWARD_POLICY))
            {
                this._logger.UsingIncorrectRollForwardPolicy(
                    solutionFileName: solutionFileName,
                    projectPolicy: info.RollForward,
                    expectedPolicy: ROLL_FORWARD_POLICY
                );
            }
        }
        else
        {
            this._logger.DoesNotSpecifyADotNetSdkRollForwardPolicy(solutionFileName);
        }
    }
}
