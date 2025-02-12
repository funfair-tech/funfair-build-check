using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FunFair.BuildCheck.Helpers;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.Runner;
using FunFair.BuildCheck.Runner.Services;
using FunFair.BuildCheck.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FunFair.BuildCheck;

internal static class Program
{
    private const int SUCCESS = 0;
    private const int ERROR = 1;

    private static void Usage()
    {
        Console.WriteLine();
        Console.WriteLine(value: "Usage:");
        Console.WriteLine($"{typeof(Program).Namespace} -Solution D:\\Source\\Solution.sln [-WarningAsErrors true|false] [-PreReleaseBuild true|false]");
    }

    public static async Task<int> Main(string[] args)
    {
        try
        {
            Console.WriteLine($"{VersionInformation.Product} {VersionInformation.Version}");

            IConfigurationRoot configuration = GetCommandLineConfiguration(args);

            if (!GetConfiguration(configuration: configuration, out string solutionFileName, out bool warningsAsErrors, out bool preReleaseBuild))
            {
                Usage();

                return ERROR;
            }

            string? baseFolder = Path.GetDirectoryName(solutionFileName);

            if (string.IsNullOrWhiteSpace(baseFolder))
            {
                Console.WriteLine(value: "Missing Solution file.");

                Usage();

                return ERROR;
            }

            IDiagnosticLogger logger = new DiagnosticLogger(warningsAsErrors: warningsAsErrors);
            IFrameworkSettings frameworkSettings = new FrameworkSettings();
            IProjectClassifier projectClassifier = new ProjectClassifier();

            int errors = await CheckRunner.CheckAsync(
                solutionFileName: 
                                                      ningsAsErrors,
                fram
                                                      ctClassifier: projectClassifier,
    
                                                      ReleaseBuild, allowPackageVersionMism
                                                      vices => services.BuildServiceProvider(),
                logger: logger,
                cancellationToken: CancellationToken.None
            );

            return ReportStatus(errors: errors, solutionFileName: solutionFileName);
        }
        catch (Exception exception)
        {
            Console.WriteLine($"ERROR: {exception.Message}");

            return ERROR;
        }
    }

    private static int ReportStatus(int errors, string solutionFileName)
    {
        if (errors != 0)
        {
            Console.WriteLine();
            Console.WriteLine(errors > 1 ? $"Found {errors} Errors" : $"Found {errors} Error");

            return errors;
        }

        OutputSolutionFileName(solutionFileName);

        Console.WriteLine();
        Console.WriteLine(value: "No errors found.");

        return SUCCESS;
    }

    private static bool GetConfiguration(IConfigurationRoot configuration, out string solutionFileName, out bool warningsAsErrors, out bool preReleaseBuild)
    {
        solutionFileName = configuration.GetValue<string>(key: "solution") ?? string.Empty;
        warningsAsErrors = false;
        preReleaseBuild = false;

        if (string.IsNullOrWhiteSpace(solutionFileName))
        {
            Console.WriteLine(value: "Missing Solution file.");

            return false;
        }

        solutionFileName = PathHelpers.ConvertToNative(solutionFileName);

        if (!File.Exists(solutionFileName))
        {
            Console.WriteLine(value: $"Could not find solution file {solutionFileName}");

            return false;
        }

        warningsAsErrors = configuration.GetValue<bool>(key: "WarningAsErrors");

        if (warningsAsErrors)
        {
            Console.WriteLine(value: "** Running with Warnings as Errors");
        }

        preReleaseBuild = configuration.GetValue<bool>(key: "PreReleaseBuild");

        if (!warningsAsErrors)
        {
            Console.WriteLine(value: "** Running with release build requirements");
        }

        return true;
    }

    private static IConfigurationRoot GetCommandLineConfiguration(string[] args)
    {
        return new ConfigurationBuilder()
            .AddCommandLine(
                args: args,
                new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    { "-Solution", "solution" },
                    { "-WarningAsErrors", "WarningAsErrors" },
                    { "-PreReleaseBuild", "PreReleaseBuild" },
                }
            )
            .Build();
    }

    private static void OutputSolutionFileName(string solutionFileName)
    {
        string? env = Environment.GetEnvironmentVariable("TEAMCITY_VERSION");

        if (!string.IsNullOrWhiteSpace(env))
        {
            Console.WriteLine($"##teamcity[setParameter name='env.SOLUTION_FILENAME' value='{solutionFileName}']");
        }
    }
}
