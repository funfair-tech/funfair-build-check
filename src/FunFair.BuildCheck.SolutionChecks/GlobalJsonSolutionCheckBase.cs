using System;
using System.Threading;
using System.Threading.Tasks;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.SolutionChecks.Helpers;
using FunFair.BuildCheck.SolutionChecks.Models;

namespace FunFair.BuildCheck.SolutionChecks;

public abstract class GlobalJsonSolutionCheckBase : ISolutionCheck
{
    private readonly IGlobalJsonLoader _loader;

    protected GlobalJsonSolutionCheckBase(IRepositorySettings repositorySettings, IGlobalJsonLoader loader)
    {
        this._loader = loader;
        this.DotNetSdkVersion = repositorySettings.DotNetSdkVersion ?? string.Empty;
    }

    protected string DotNetSdkVersion { get; }

    public async ValueTask CheckAsync(string solutionFileName, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(this.DotNetSdkVersion))
        {
            this.OnNotConfigured(solutionFileName);

            return;
        }

        GlobalJsonLoadResult result = await this._loader.LoadAsync(
            solutionFileName: solutionFileName,
            cancellationToken: cancellationToken
        );

        switch (result)
        {
            case GlobalJsonLoadedResult loaded:
                this.CheckSdk(solutionFileName: solutionFileName, info: loaded.Info);

                return;
            case GlobalJsonReadFailedResult failed:
                this.OnReadFailed(solutionFileName: solutionFileName, file: failed.File, exception: failed.Exception);

                return;
            default:
                return;
        }
    }

    protected virtual void OnNotConfigured(string solutionFileName) { }

    protected abstract void OnReadFailed(string solutionFileName, string file, Exception exception);

    protected abstract void CheckSdk(string solutionFileName, GlobalJsonInfo info);
}
