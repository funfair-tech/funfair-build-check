using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using BuildCheck.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BuildCheck
{
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
                Console.WriteLine($"{typeof(Program).Namespace} {ExecutableVersionInformation.ProgramVersion()}");

                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .AddCommandLine(args, new Dictionary<string, string> {{@"-Solution", @"solution"}, {@"-WarningAsErrors", @"WarningAsErrors"}, {@"-PreReleaseBuild", @"PreReleaseBuild"}})
                    .Build();

                string solutionFileName = configuration.GetValue<string>(key: @"solution");

                if (string.IsNullOrWhiteSpace(solutionFileName))
                {
                    Console.WriteLine(value: "Missing Solution file.");

                    Usage();

                    return ERROR;
                }

                if (!File.Exists(solutionFileName))
                {
                    Console.WriteLine(value: "Missing Solution file.");
                    Usage();

                    return ERROR;
                }

                bool warningsAsErrors = configuration.GetValue<bool>(key: @"WarningAsErrors");

                if (warningsAsErrors)
                {
                    Console.WriteLine(value: "** Running with Warnings as Errors");
                }

                bool preReleaseBuild = configuration.GetValue<bool>(key: @"PreReleaseBuild");

                if (!warningsAsErrors)
                {
                    Console.WriteLine(value: "** Running with release build requirements");
                }

                IServiceProvider services = Setup(warningsAsErrors, preReleaseBuild);

                string baseFolder = Path.GetDirectoryName(solutionFileName);

                IDiagnosticLogger logging = services.GetService<IDiagnosticLogger>();

                await PerformChecks(services, solutionFileName, logging, baseFolder)
                    .ConfigureAwait(continueOnCapturedContext: false);

                if (logging.IsErrored)
                {
                    Console.WriteLine();
                    Console.WriteLine(logging.Errors > 1 ? $"Found {logging.Errors} Errors" : $"Found {logging.Errors} Error");

                    return ERROR;
                }

                Console.WriteLine();
                Console.WriteLine(value: "No errors found.");

                return SUCCESS;
            }
            catch (Exception exception)
            {
                Console.WriteLine($"ERROR: {exception.Message}");

                return ERROR;
            }
        }

        private static async Task PerformChecks(IServiceProvider services, string solutionFileName, IDiagnosticLogger logging, string baseFolder)
        {
            ISolutionCheck[] solutionChecks = RegisteredSolutionChecks(services);
            IProjectCheck[] projectChecks = RegisteredProjectChecks(services);

            foreach (ISolutionCheck check in solutionChecks)
            {
                check.Check(solutionFileName);
            }

            Project[] projects = await LoadProjects(solutionFileName)
                .ConfigureAwait(continueOnCapturedContext: false);

            foreach (Project project in projects)
            {
                logging.LogInformation($"Checking Project: {project.DisplayName}:");

                string projectPath = Path.Combine(baseFolder, project.FileName);

                XmlDocument doc = new XmlDocument();
                doc.Load(projectPath);

                foreach (IProjectCheck check in projectChecks)
                {
                    check.Check(project.DisplayName, doc);
                }
            }
        }

        private static IServiceProvider Setup(bool warningsAsErrors, bool preReleaseBuild)
        {
            IServiceCollection services = new ServiceCollection();

            DiagnosticLogger logger = new DiagnosticLogger(warningsAsErrors);
            services.AddSingleton<ILogger>(logger);
            services.AddSingleton<IDiagnosticLogger>(logger);
            services.AddSingleton(typeof(ILogger<>), typeof(LoggerProxy<>));

            BuildCheck.Setup.SetupSolutionChecks(services);
            BuildCheck.Setup.SetupProjectChecks(services);

            services.AddSingleton<ICheckConfiguration>(new CheckConfiguration {PreReleaseBuild = preReleaseBuild});

            IServiceProviderFactory<IServiceCollection> spf = new DefaultServiceProviderFactory();

            return spf.CreateServiceProvider(services);
        }

        private static async Task<Project[]> LoadProjects(string solution)
        {
            string[] text = await File.ReadAllLinesAsync(solution)
                .ConfigureAwait(continueOnCapturedContext: false);

            List<Project> projects = new List<Project>();

            Console.WriteLine(value: "Looking for projects...");

            Regex regex = new Regex(
                pattern:
                "^Project\\(\"[{(]?[0-9A-F]{8}[-]?([0-9A-F]{4}[-]?){3}[0-9A-F]{12}[)}]\"\\)\\s*=\\s*\"(?<DisplayName>.*?)\",\\s*\"(?<FileName>.*?\\.csproj)\",\\s*\"[{(]?[0-9A-F]{8}[-]?([0-9A-F]{4}[-]?){3}[0-9A-F]{12}[)}]\"$",
                RegexOptions.Compiled);

            foreach (string line in text)
            {
                foreach (Match match in regex.Matches(line))
                {
                    string displayName = match.Groups[groupname: @"DisplayName"]
                        .Value;
                    string fileName = match.Groups[groupname: @"FileName"]
                        .Value;

                    Console.WriteLine($" * {displayName}");

                    projects.Add(new Project(displayName: displayName, fileName: fileName));
                }
            }

            return projects.ToArray();
        }

        private static ISolutionCheck[] RegisteredSolutionChecks(IServiceProvider services)
        {
            return services.GetServices<ISolutionCheck>()
                .ToArray();
        }

        private static IProjectCheck[] RegisteredProjectChecks(IServiceProvider services)
        {
            return services.GetServices<IProjectCheck>()
                .ToArray();
        }
    }
}