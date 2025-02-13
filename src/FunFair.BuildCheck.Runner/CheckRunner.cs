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
using FunFair.BuildCheck.Runner.LoggingExtensions;
using FunFair.BuildCheck.Runner.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.Runner;

public static class CheckRunner
{
    private static readonly Regex ProjectReferenceRegex = SourceGenerated.ProjectReferenceRegex();

    public static async ValueTask<int> CheckAsync(
        string solutionFileName,
        bool warningsAsErrors,
        IFrameworkSettings frameworkSettings,
        IProjectClassifier projectClassifier,
        ICheckConfiguration checkConfiguration,
        Func<IServiceCollection, IServiceProvider> buildServiceProvider,
        ILogger logger,
        CancellationToken cancellationToken
    )
    {
        IReadOnlyList<SolutionProject> projects = await LoadProjectsAsync(
            solution: solutionFileName,
            cancellationToken: cancellationToken
        );
        IServiceProvider services = Setup(
            warningsAsErrors: warningsAsErrors,
            projects: projects,
            frameworkSettings: frameworkSettings,
            projectClassifier: projectClassifier,
            checkConfiguration: checkConfiguration,
            buildServiceProvider: buildServiceProvider,
            logger: logger
        );

        ITrackingLogger logging = services.GetRequiredService<ITrackingLogger>();

        await PerformChecksAsync(
            services: services,
            solutionFileName: solutionFileName,
            logging: logging,
            cancellationToken: cancellationToken
        );

        return (int)logging.Errors;
    }

    private static async ValueTask PerformChecksAsync(
        IServiceProvider services,
        string solutionFileName,
        ITrackingLogger logging,
        CancellationToken cancellationToken
    )
    {
        IProjectXmlLoader projectXmlLoader = services.GetRequiredService<IProjectXmlLoader>();
        IReadOnlyList<ISolutionCheck> solutionChecks = RegisteredSolutionChecks(services);
        IReadOnlyList<IProjectCheck> projectChecks = RegisteredProjectChecks(services);

        await TestSolutionAsync(
            solutionFileName: solutionFileName,
            solutionChecks: solutionChecks,
            cancellationToken: cancellationToken
        );

        IReadOnlyList<SolutionProject> projects = services.GetRequiredService<
            IReadOnlyList<SolutionProject>
        >();

        foreach (SolutionProject project in projects)
        {
            await CheckProjectAsync(
                project: project,
                projectXmlLoader: projectXmlLoader,
                projectChecks: projectChecks,
                logging: logging,
                cancellationToken: cancellationToken
            );
        }
    }

    private static async ValueTask TestSolutionAsync(
        string solutionFileName,
        IReadOnlyList<ISolutionCheck> solutionChecks,
        CancellationToken cancellationToken
    )
    {
        foreach (ISolutionCheck check in solutionChecks)
        {
            await check.CheckAsync(
                solutionFileName: solutionFileName,
                cancellationToken: cancellationToken
            );
        }
    }

    private static async ValueTask CheckProjectAsync(
        SolutionProject project,
        IProjectXmlLoader projectXmlLoader,
        IReadOnlyList<IProjectCheck> projectChecks,
        ITrackingLogger logging,
        CancellationToken cancellationToken
    )
    {
        logging.LogCheckingProject(project.DisplayName);

        string? projectFolder = Path.GetDirectoryName(project.FileName);

        if (string.IsNullOrEmpty(projectFolder))
        {
            logging.LogCouldNotGetBasePathOfProject(project.FileName);

            return;
        }

        XmlDocument doc = await projectXmlLoader.LoadAsync(
            path: project.FileName,
            cancellationToken: cancellationToken
        );

        await TestProjectAsync(
            projectChecks: projectChecks,
            project: project,
            projectFolder: projectFolder,
            doc: doc,
            cancellationToken: cancellationToken
        );
    }

    private static async ValueTask TestProjectAsync(
        IReadOnlyList<IProjectCheck> projectChecks,
        SolutionProject project,
        string projectFolder,
        XmlDocument doc,
        CancellationToken cancellationToken
    )
    {
        foreach (IProjectCheck check in projectChecks)
        {
            await check.CheckAsync(
                projectName: project.DisplayName,
                projectFolder: projectFolder,
                project: doc,
                cancellationToken: cancellationToken
            );
        }
    }

    private static IServiceProvider Setup(
        bool warningsAsErrors,
        IReadOnlyList<SolutionProject> projects,
        IFrameworkSettings frameworkSettings,
        IProjectClassifier projectClassifier,
        ICheckConfiguration checkConfiguration,
        Func<IServiceCollection, IServiceProvider> buildServiceProvider,
        ILogger logger
    )
    {
        IRepositorySettings wrappedRepositorySettings = new RepositorySettings(
            frameworkSettings: frameworkSettings,
            projectClassifier: projectClassifier,
            projects: projects
        );

        TrackingLogger trackingLogger = new(warningsAsErrors: warningsAsErrors, logger: logger);

        return buildServiceProvider(
            new ServiceCollection()
                .AddSingleton(wrappedRepositorySettings)
                .AddSingleton<ILogger>(trackingLogger)
                .AddSingleton<ITrackingLogger>(trackingLogger)
                .AddSingleton(typeof(ILogger<>), typeof(LoggerProxy<>))
                .AddSingleton(projects)
                .AddSingleton<IProjectXmlLoader, ProjectXmlLoader>()
                .SetupSolutionChecks()
                .SetupProjectChecks(repositorySettings: wrappedRepositorySettings)
                .AddSingleton(checkConfiguration)
        );
    }

    private static async ValueTask<IReadOnlyList<SolutionProject>> LoadProjectsAsync(
        string solution,
        CancellationToken cancellationToken
    )
    {
        string[] text = await File.ReadAllLinesAsync(
            path: solution,
            cancellationToken: cancellationToken
        );

        string? basePath = Path.GetDirectoryName(solution);

        if (string.IsNullOrEmpty(basePath))
        {
            Console.WriteLine($"Solution {solution} could not get base path");

            return [];
        }

        Console.WriteLine($"Solution base path: {basePath}");

        Console.WriteLine(value: "Looking for projects...");

        return
        [
            .. text.SelectMany(line => ProjectReferenceRegex.Matches(line).Select(ExtractProject)),
        ];

        SolutionProject ExtractProject(Match match)
        {
            string displayName = match.Groups[groupname: "DisplayName"].Value;
            string fileName = match.Groups[groupname: "FileName"].Value;

            Console.WriteLine($" * {displayName} = {fileName}");

            string fullPath = Path.Combine(path1: basePath, PathHelpers.ConvertToNative(fileName));
            Console.WriteLine($"    - {fullPath}");

            return new(fileName: fullPath, displayName: displayName);
        }
    }

    private static IReadOnlyList<ISolutionCheck> RegisteredSolutionChecks(IServiceProvider services)
    {
        return [.. services.GetServices<ISolutionCheck>()];
    }

    private static IReadOnlyList<IProjectCheck> RegisteredProjectChecks(IServiceProvider services)
    {
        return [.. services.GetServices<IProjectCheck>()];
    }
}
