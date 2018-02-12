using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;

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

        public static int Main(string[] args)
        {
            try
            {
                Console.WriteLine($"{typeof(Program).Namespace} {ExecutableVersionInformation.ProgramVersion()}");

                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .AddCommandLine(args, new Dictionary<string, string> {{@"-Solution", @"solution"}, {@"-WarningAsErrors", @"WarningAsErrors"}})
                    .Build();

                string solution = configuration.GetValue<string>(@"solution");
                if (string.IsNullOrWhiteSpace(solution))
                {
                    Usage();
                    return ERROR;
                }

                if (!File.Exists(solution))
                {
                    Console.WriteLine("Missing Solution file.");
                    Usage();
                    return ERROR;
                }

                bool warningsAsErrors = configuration.GetValue<bool>(@"WarningAsErrors");
                Console.WriteLine($"{warningsAsErrors}");

                return SUCCESS;
            }
            catch (Exception exception)
            {
                Console.WriteLine($"ERROR: {exception.Message}");
                return ERROR;
            }
        }
    }
}