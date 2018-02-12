using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using BuildCheck.ProjectChecks;
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
            Console.WriteLine("Usage:");
            Console.WriteLine($"{typeof(Program).Namespace} -Solution D:\\Source\\Solution.sln [-WarningAsErrors true|false]");
        }

        public static async Task<int> Main(string[] args)
        {
            try
            {
                Console.WriteLine($"{typeof(Program).Namespace} {ExecutableVersionInformation.ProgramVersion()}");

                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .AddCommandLine(args, new Dictionary<string, string> {{@"-Solution", @"solution"}, {@"-WarningAsErrors", @"WarningAsErrors"}})
                    .Build();

                string solutionFileName = configuration.GetValue<string>(@"solution");
                if (string.IsNullOrWhiteSpace(solutionFileName))
                {
                    Console.WriteLine("Missing Solution file.");

                    Usage();
                    return ERROR;
                }

                if (!File.Exists(solutionFileName))
                {
                    Console.WriteLine("Missing Solution file.");
                    Usage();
                    return ERROR;
                }

                bool warningsAsErrors = configuration.GetValue<bool>(@"WarningAsErrors");
                Console.WriteLine($"{warningsAsErrors}");

                IServiceProvider services = Setup(warningsAsErrors);

                string baseFolder = Path.GetDirectoryName(solutionFileName);

                IDiagnosticLogger logging = services.GetService<IDiagnosticLogger>();

                await PerformChecks(services, solutionFileName, logging, baseFolder);

                return logging.Errors == 0 ? SUCCESS : ERROR;
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

            foreach (ISolutionCheck check in solutionChecks) check.Check(solutionFileName);

            Project[] projects = await LoadProjects(solutionFileName);

            foreach (Project project in projects)
            {
                logging.LogInformation($"Checking Project: {project.DisplayName}:");

                string projectPath = Path.Combine(baseFolder, project.FileName);

                XmlDocument doc = new XmlDocument();
                doc.Load(projectPath);

                foreach (IProjectCheck check in projectChecks) check.Check(project.DisplayName, doc);
            }
        }

        private static IServiceProvider Setup(bool warningsAsErrors)
        {
            IServiceCollection services = new ServiceCollection();

            DiagnosticLogger logger = new DiagnosticLogger(warningsAsErrors);
            services.AddSingleton<ILogger>(logger);
            services.AddSingleton<IDiagnosticLogger>(logger);
            services.AddSingleton(typeof(ILogger<>), typeof(LoggerProxy<>));

            services.AddSingleton<IProjectCheck, NoPreReleaseNuGetPackages>();

            IServiceProviderFactory<IServiceCollection> spf = new DefaultServiceProviderFactory();

            return spf.CreateServiceProvider(services);
        }

        private static async Task<Project[]> LoadProjects(string solution)
        {
            string[] text = await File.ReadAllLinesAsync(solution);

            List<Project> projects = new List<Project>();

            Console.WriteLine("Looking for projects...");

            Regex regex = new Regex(
                "^Project\\(\"[{(]?[0-9A-F]{8}[-]?([0-9A-F]{4}[-]?){3}[0-9A-F]{12}[)}]\"\\)\\s*=\\s*\"(?<DisplayName>.*?)\",\\s*\"(?<FileName>.*?\\.csproj)\",\\s*\"[{(]?[0-9A-F]{8}[-]?([0-9A-F]{4}[-]?){3}[0-9A-F]{12}[)}]\"$",
                RegexOptions.Compiled);

            foreach (string line in text)
            foreach (Match match in regex.Matches(line))
            {
                string displayName = match.Groups[@"DisplayName"]
                    .Value;
                string fileName = match.Groups[@"FileName"]
                    .Value;

                Console.WriteLine($" * {displayName}");

                projects.Add(new Project(displayName: displayName, fileName: fileName));
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

    public sealed class Project
    {
        public Project(string fileName, string displayName)
        {
            this.FileName = fileName;
            this.DisplayName = displayName;
        }

        public string FileName { get; }

        public string DisplayName { get; }
    }

    public interface IDiagnosticLogger : ILogger
    {
        long Errors { get; }

        bool IsErrored { get; }
    }
}