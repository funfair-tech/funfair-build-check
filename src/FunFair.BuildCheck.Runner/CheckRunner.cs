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
using FunFair.BuildCheck.Runner.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.Runner;

public static class CheckRunner
{
    private static readonly Regex ProjectReferenceRegex = SourceGenerated.ProjectReferenceRegex();

    public static async ValueTask<int> CheckAsync(string solutionFileName,
                                                  bool preReleaseBuild,
                                                  bool warningsAsErrors,
                                                  IFrameworkSettings frameworkSettings,
                                                  IProjectClassifier projectClassifier,
                                                  ILogger logger,
                                                  CancellationToken cancellationToken)
    {
        IReadOnlyList<SolutionProject> projects = await LoadProjectsAsync(solution: solutionFileName, cancellationToken: cancellationToken);
        IServiceProvider services = Setup(warningsAsErrors: warningsAsErrors,
                                          preReleaseBuild: preReleaseBuild,
                                          projects: projects,
                                          frameworkSettings: frameworkSettings,
                                          projectClassifier: projectClassifier,
                                          logger: logger);

        ITrackingLogger logging = services.GetRequiredService<ITrackingLogger>();

        PerformChecks(services: services, solutionFileName: solutionFileName, logging: logging);

        return (int)logging.Errors;
    }

    private static void PerformChecks(IServiceProvider services, string solutionFileName, ITrackingLogger logging)
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

    private static void CheckProject(SolutionProject project, IProjectLoader projectLoader, IReadOnlyList<IProjectCheck> projectChecks, ITrackingLogger logging)
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

    private static IServiceProvider Setup(bool warningsAsErrors,
                                          bool preReleaseBuild,
                                          IReadOnlyList<SolutionProject> projects,
                                          IFrameworkSettings frameworkSettings,
                                          IProjectClassifier projectClassifier,
                                          ILogger logger)
    {
        IRepositorySettings wrappedRepositorySettings = new RepositorySettings(projects: projects, frameworkSettings: frameworkSettings, projectClassifier: projectClassifier);

        TrackingLogger trackingLogger = new(warningsAsErrors: warningsAsErrors, logger: logger);

        return new ServiceCollection().AddSingleton(wrappedRepositorySettings)
                                      .AddSingleton<ILogger>(trackingLogger)
                                      .AddSingleton<ITrackingLogger>(trackingLogger)
                                      .AddSingleton(typeof(ILogger<>), typeof(LoggerProxy<>))
                                      .AddSingleton(projects)
                                      .AddSingleton<IProjectLoader, ProjectLoader>()
                                      .SetupSolutionChecks()
                                      .SetupProjectChecks(repositorySettings: wrappedRepositorySettings)
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