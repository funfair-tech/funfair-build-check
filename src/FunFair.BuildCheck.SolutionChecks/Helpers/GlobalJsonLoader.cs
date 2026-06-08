using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FunFair.BuildCheck.SolutionChecks.Models;

namespace FunFair.BuildCheck.SolutionChecks.Helpers;

public sealed class GlobalJsonLoader : IGlobalJsonLoader
{
    private readonly Dictionary<string, GlobalJsonLoadResult> _cache = new(StringComparer.Ordinal);

    public async ValueTask<GlobalJsonLoadResult> LoadAsync(string solutionFileName, CancellationToken cancellationToken)
    {
        if (this._cache.TryGetValue(key: solutionFileName, out GlobalJsonLoadResult? cached))
        {
            return cached;
        }

        GlobalJsonLoadResult result = await LoadCoreAsync(
            solutionFileName: solutionFileName,
            cancellationToken: cancellationToken
        );

        this._cache[solutionFileName] = result;

        return result;
    }

    private static async ValueTask<GlobalJsonLoadResult> LoadCoreAsync(
        string solutionFileName,
        CancellationToken cancellationToken
    )
    {
        if (!GlobalJsonHelpers.GetFileNameForSolution(solutionFileName: solutionFileName, out string? file))
        {
            return GlobalJsonNotFoundResult.Instance;
        }

        string content = await File.ReadAllTextAsync(path: file, cancellationToken: cancellationToken);

        try
        {
            GlobalJsonPacket? packet = JsonSerializer.Deserialize(
                json: content,
                jsonTypeInfo: MustBeSerializable.Default.GlobalJsonPacket
            );

            GlobalJsonInfo info = new(
                sdkVersion: packet?.Sdk?.Version,
                rollForward: packet?.Sdk?.RollForward,
                allowPrerelease: packet?.Sdk?.AllowPrerelease
            );

            return new GlobalJsonLoadedResult(info);
        }
        catch (Exception exception)
        {
            return new GlobalJsonReadFailedResult(file: file, exception: exception);
        }
    }
}
