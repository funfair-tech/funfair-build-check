using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BuildCheck.SolutionChecks
{
    public sealed class GlobalJsonIsLatest : ISolutionCheck
    {
        private readonly string? _dotnetVersion;
        private readonly ILogger<GlobalJsonIsLatest> _logger;

        public GlobalJsonIsLatest(ILogger<GlobalJsonIsLatest> logger)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this._dotnetVersion = Environment.GetEnvironmentVariable(variable: @"DOTNET_CORE_SDK_VERSION");
        }

        /// <inheritdoc />
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

            string? file = Path.Combine(solutionDir, path2: @"global.json");
            if (file == null)
            {
                return;
            }

            if (!File.Exists(file))
            {
                return;
            }

            string content = File.ReadAllText(file);

            try
            {
                Packet p = JsonConvert.DeserializeObject<Packet>(content);

                if (p?.Sdk?.Version != null)
                {
                    if (!StringComparer.InvariantCultureIgnoreCase.Equals(p.Sdk.Version, this._dotnetVersion))
                    {
                        this._logger.LogError($"global.json is using {p.Sdk.Version} rather than {this._dotnetVersion}");
                    }
                }
                else
                {
                    this._logger.LogError("global.json does not specify a version");
                }

            }
            catch (Exception exception)
            {
                this._logger.LogError(new EventId(exception.HResult), exception, $"Failed to read {file} : {exception.Message}");
            }
        }

        [SuppressMessage(category: "Microsoft.Performance", checkId: "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Created through deserialization")]
        private sealed class Packet
        {
            public Sdk? Sdk { get; set; }
        }

        [SuppressMessage(category: "Microsoft.Performance", checkId: "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Created through deserialization")]
        private sealed class Sdk
        {
            public string? Version { get; set; }
        }
    }
}