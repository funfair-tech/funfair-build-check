using System;
using System.IO;
using System.Text.Json;
using FunFair.BuildCheck.SolutionChecks.Models;

namespace FunFair.BuildCheck.SolutionChecks
{
    public sealed class GlobalJsonMustNotAllowPreRelease : ISolutionCheck
    {
        private const bool PRE_RELEASE_POLICY = false;

        private readonly ILogger<GlobalJsonIsLatest> _logger;

        public GlobalJsonMustNotAllowPreRelease(ILogger<GlobalJsonIsLatest> logger)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public void Check(string solutionFileName)
        {
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
                GlobalJsonPacket p = JsonSerializer.Deserialize<GlobalJsonPacket>(json: content,
                                                                                  new JsonSerializerOptions
                                                                                  {
                                                                                      IgnoreNullValues = true,
                                                                                      PropertyNameCaseInsensitive = false,
                                                                                      PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                                                                                      WriteIndented = false,
                                                                                      AllowTrailingCommas = false
                                                                                  });

                if (p?.Sdk?.AllowPrerelease != null)
                {
                    if (p.Sdk.AllowPrerelease != PRE_RELEASE_POLICY)
                    {
                        this._logger.LogError(
                            $"global.json is using SDK pre-release policy of {FormatPolicy(p.Sdk.AllowPrerelease.GetValueOrDefault(defaultValue: true))} rather than {FormatPolicy(PRE_RELEASE_POLICY)}");
                    }
                }
                else
                {
                    this._logger.LogError(message: "global.json does not specify a SDK pre-release policy");
                }
            }
            catch (Exception exception)
            {
                this._logger.LogError(new EventId(exception.HResult), exception: exception, $"Failed to read {file} : {exception.Message}");
            }
        }

        private static string FormatPolicy(bool preReleaseAllowed)
        {
            return preReleaseAllowed ? "Pre-Release Allowed" : "Production Only";
        }
    }
}