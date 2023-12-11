using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using FunFair.BuildCheck.Helpers;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck;

internal static class Program
{
    private const int SUCCESS = 0;
    private const int ERROR = 1;

    private static readonly Regex ProjectReferenceRegex = SourceGenerated.ProjectReferenceRegex();

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
            Console.WriteLine($"{typeof(Program).Namespace} {ExecutableVersionInformation.ProgramVersion}");

            IConfigurationRoot configuration = GetCommandLineConfiguration(args);

            if (!GetConfiguration(configuration: configuration, out string solutionFileName, out bool warningsAsErrors, out bool preReleaseBuild))
            {
                Usage();

                return ERROR;
            }

            IReadOnlyList<SolutionProject> projects = await LoadProjectsAsync(solution: solutionFileName, cancellationToken: CancellationToken.None);
            IServiceProvider services = Setup(warningsAsErrors: warningsAsErrors, preReleaseBuild: preReleaseBuild, projects: projects);

            string? baseFolder = Path.GetDirectoryName(solutionFileName);

            if (string.IsNullOrWhiteSpace(baseFolder))
            {
                Console.WriteLine(value: "Missing Solution file.");

                Usage();

                return ERROR;
            }

            IDiagnosticLogger logging = services.GetRequiredService<IDiagnosticLogger>();

            PerformChecks(services: services, solutionFileName: solutionFileName, logging: logging);

            return ReportStatus(logging: logging, solutionFileName: solutionFileName);
        }
        catch (Exception exception)
        {
            Console.WriteLine($"ERROR: {exception.Message}");

            return ERROR;
        }
    }

    private static int ReportStatus(IDiagnosticLogger logging, string solutionFileName)
    {
        if (logging.IsErrored)
        {
            Console.WriteLine();
            Console.WriteLine(logging.Errors > 1
                                  ? $"Found {logging.Errors} Errors"
                                  : $"Found {logging.Errors} Error");

            return (int)logging.Errors;
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
        return new ConfigurationBuilder().AddCommandLine(args: args,
                                                         new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                                                         {
                                                             { "-Solution", "solution" }, { "-WarningAsErrors", "WarningAsErrors" }, { "-PreReleaseBuild", "PreReleaseBuild" }
                                                         })
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

    private static void PerformChecks(IServiceProvider services, string solutionFileName, IDiagnosticLogger logging)
    {
        IProjectLoader projectLoader = services.GetRequiredService<IProjectLoader>();
        IReadOnlyList<ISolutionCheck> solutionChecks = RegisteredSolutionChecks(services);
        IReadOnlyList<IProjectCheck> projectChecks = RegisteredProjectChecks(services);

        TestSolution(solutionFileName: solutionFileName, solutionChecks: solutionChecks);

        IReadOnlyList<SolutionProject> projects = services.GetRequiredService<IReadOnlyList<SolutionProject>>();

        foreach (SolutionProject project in projects)
        {
            CheckProject(project: project, projectLoader: projectLoader, projectChecks: projectChecks, logging: logging);
        }
    }

    private static void TestSolution(string solutionFileName, IReadOnlyList<ISolutionCheck> solutionChecks)
    {
        foreach (ISolutionCheck check in solutionChecks)
        {
            check.Check(solutionFileName);
        }
    }

    private static void CheckProject(SolutionProject project, IProjectLoader projectLoader, IReadOnlyList<IProjectCheck> projectChecks, IDiagnosticLogger logging)
    {
        logging.LogInformation($"Checking Project: {project.DisplayName}:");

        string? projectFolder = Path.GetDirectoryName(project.FileName);

        if (string.IsNullOrEmpty(projectFolder))
        {
            logging.LogError($"Project: {project.FileName} could not get base path");

            return;
        }

        XmlDocument doc = projectLoader.Load(project.FileName);

        TestProject(projectChecks: projectChecks, project: project, projectFolder: projectFolder, doc: doc);
    }

    private static void TestProject(IReadOnlyList<IProjectCheck> projectChecks, SolutionProject project, string projectFolder, XmlDocument doc)
    {
        foreach (IProjectCheck check in projectChecks)
        {
            check.Check(projectName: project.DisplayName, projectFolder: projectFolder, project: doc);
        }
    }

    private static IServiceProvider Setup(bool warningsAsErrors, bool preReleaseBuild, IReadOnlyList<SolutionProject> projects)
    {
        IRepositorySettings repositorySettings = new RepositorySettings(projects);
        DiagnosticLogger logger = new(warningsAsErrors);

        return new ServiceCollection().AddSingleton(repositorySettings)
                                      .AddSingleton<ILogger>(logger)
                                      .AddSingleton<IDiagnosticLogger>(logger)
                                      .AddSingleton(typeof(ILogger<>), typeof(LoggerProxy<>))
                                      .AddSingleton(projects)
                                      .AddSingleton<IProjectLoader, ProjectLoader>()
                                      .SetupSolutionChecks()
                                      .SetupProjectChecks(repositorySettings: repositorySettings)
                                      .AddSingleton<ICheckConfiguration>(new CheckConfiguration { PreReleaseBuild = preReleaseBuild })
                                      .BuildServiceProvider();
    }

    private static async Task<IReadOnlyList<SolutionProject>> LoadProjectsAsync(string solution, CancellationToken cancellationToken)
    {
        string[] text = await File.ReadAllLinesAsync(path: solution, cancellationToken: cancellationToken);

        string? basePath = Path.GetDirectoryName(solution);

        if (string.IsNullOrEmpty(basePath))
        {
            Console.WriteLine($"Solution {solution} could not get base path");

            return Array.Empty<SolutionProject>();
        }

        Console.WriteLine($"Solution base path: {basePath}");

        List<SolutionProject> projects = new();

        Console.WriteLine(value: "Looking for projects...");

        foreach (string line in text)
        {
            foreach (Match? match in ProjectReferenceRegex.Matches(line))
            {
                if (match is null)
                {
                    continue;
                }

                string displayName = match.Groups[groupname: "DisplayName"].Value;
                string fileName = match.Groups[groupname: "FileName"].Value;

                Console.WriteLine($" * {displayName} = {fileName}");

                string fullPath = Path.Combine(path1: basePath, PathHelpers.ConvertToNative(fileName));
                Console.WriteLine($"    - {fullPath}");

                projects.Add(new(displayName: displayName, fileName: fullPath));
            }
        }

        return projects.ToArray();
    }

    private static IReadOnlyList<ISolutionCheck> RegisteredSolutionChecks(IServiceProvider services)
    {
        return services.GetServices<ISolutionCheck>()
                       .ToArray();
    }

    private static IReadOnlyList<IProjectCheck> RegisteredProjectChecks(IServiceProvider services)
    {
        return services.GetServices<IProjectCheck>()
                       .ToArray();
    }
}