using System;
using System.Collections.Generic;
using System.Diagnostics;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

public static class TrivialProjectChecksRegistration
{
    private static readonly IReadOnlyList<string> MustNotDefineStaticCodeAnalysisProperties =
    [
        "CodeAnalysisRuleSet",
        "IncludeOpenAPIAnalyzers",
        "WarningsNotAsErrors",
    ];

    public static IServiceCollection RegisterGeneralTrivialProjectChecks(
        this IServiceCollection services,
        IRepositorySettings repositorySettings
    )
    {
        services.AddSimplePropertyChecks(GetGeneralSimpleChecks(repositorySettings));
        services.AddSimplePropertyChecks(GetTestHarnessChecks());

        return services;
    }

    public static IServiceCollection RegisterPackagingTrivialProjectChecks(this IServiceCollection services)
    {
        services.AddSimplePropertyChecks(GetPackagingSimpleChecks());

        return services;
    }

    public static IServiceCollection RegisterPublishingTrivialProjectChecks(this IServiceCollection services)
    {
        services.AddSimplePropertyChecks(GetPublishingSimpleChecks());

        return services;
    }

    public static IServiceCollection RegisterStaticCodeAnalysisTrivialProjectChecks(this IServiceCollection services)
    {
        services.AddSimplePropertyChecks(GetStaticCodeAnalysisSimpleChecks());

        foreach (string propertyName in MustNotDefineStaticCodeAnalysisProperties)
        {
            services.AddMustNotDefinePropertyCheck(propertyName);
        }

        return services;
    }

    public static IServiceCollection RegisterUnitTestTrivialProjectChecks(this IServiceCollection services)
    {
        services.AddSimplePropertyChecks(GetUnitTestSimpleChecks());

        return services;
    }

    private static IReadOnlyList<SimplePropertyCheck> GetGeneralSimpleChecks(IRepositorySettings repositorySettings)
    {
        return
        [
            new(
                PropertyName: "GenerateNeutralResourcesLanguageAttribute",
                RequiredValue: "true",
                CanCheck: null,
                LoggerAwareCanCheck: null
            ),
            new(PropertyName: "ImplicitUsings", RequiredValue: "disable", CanCheck: null, LoggerAwareCanCheck: null),
            new(PropertyName: "LangVersion", RequiredValue: "latest", CanCheck: null, LoggerAwareCanCheck: null),
            new(
                PropertyName: "Nullable",
                RequiredValue: "enable",
                CanCheck: _ => repositorySettings.IsNullableGloballyEnforced,
                LoggerAwareCanCheck: null
            ),
            new(
                PropertyName: "DisableImplicitNuGetFallbackFolder",
                RequiredValue: "true",
                CanCheck: null,
                LoggerAwareCanCheck: null
            ),
            new(PropertyName: "RunAOTCompilation", RequiredValue: "false", CanCheck: null, LoggerAwareCanCheck: null),
            new(
                PropertyName: "TieredCompilation",
                RequiredValue: "true",
                CanCheck: static project => project.IsPublishable(),
                LoggerAwareCanCheck: null
            ),
            new(
                PropertyName: "EnableMicrosoftExtensionsConfigurationBinderSourceGenerator",
                RequiredValue: "true",
                CanCheck: static project => project.IsPackable(),
                LoggerAwareCanCheck: null
            ),
            new(
                PropertyName: "JsonSerializerIsReflectionEnabledByDefault",
                RequiredValue: "false",
                CanCheck: static project => project.IsPackable(),
                LoggerAwareCanCheck: null
            ),
            new(
                PropertyName: "OptimizationPreference",
                RequiredValue: "speed",
                CanCheck: static project => project.IsPackable(),
                LoggerAwareCanCheck: null
            ),
            new(
                PropertyName: "GenerateSBOM",
                RequiredValue: "true",
                CanCheck: static project => project.IsPackable(),
                LoggerAwareCanCheck: null
            ),
        ];
    }

    private static IReadOnlyList<SimplePropertyCheck> GetTestHarnessChecks()
    {
        return
        [
            new(
                PropertyName: "IsTestProject",
                RequiredValue: "false",
                CanCheck: static project => IsTestHarnessExecutable(project),
                LoggerAwareCanCheck: null
            ),
            new(
                PropertyName: "IsTestingPlatformApplication",
                RequiredValue: "false",
                CanCheck: static project => IsTestHarnessExecutable(project),
                LoggerAwareCanCheck: null
            ),
            new(
                PropertyName: "TestingPlatformDotnetTestSupport",
                RequiredValue: "false",
                CanCheck: static project => IsTestHarnessExecutable(project),
                LoggerAwareCanCheck: null
            ),
        ];
    }

    private static IReadOnlyList<SimplePropertyCheck> GetPackagingSimpleChecks()
    {
        return
        [
            new(
                PropertyName: "EnablePackageValidation",
                RequiredValue: "true",
                CanCheck: static project => project.IsPackable(),
                LoggerAwareCanCheck: null
            ),
            new(
                PropertyName: "IncludeSymbols",
                RequiredValue: "true",
                CanCheck: static project => project.IsPackable() && !project.IsDotNetTool(),
                LoggerAwareCanCheck: null
            ),
        ];
    }

    private static IReadOnlyList<SimplePropertyCheck> GetPublishingSimpleChecks()
    {
        return
        [
            new(PropertyName: "DebuggerSupport", RequiredValue: "true", CanCheck: null, LoggerAwareCanCheck: null),
            new(
                PropertyName: "IlcGenerateStackTraceData",
                RequiredValue: "false",
                CanCheck: null,
                LoggerAwareCanCheck: null
            ),
            new(
                PropertyName: "IlcOptimizationPreference",
                RequiredValue: "Size",
                CanCheck: null,
                LoggerAwareCanCheck: null
            ),
            new(
                PropertyName: "EnableRequestDelegateGenerator",
                RequiredValue: "true",
                CanCheck: static project =>
                    StringComparer.OrdinalIgnoreCase.Equals(project.GetOutputType(), y: "Exe")
                    && project.IsPublishable(),
                LoggerAwareCanCheck: null
            ),
            new(PropertyName: "TieredPGO", RequiredValue: "true", CanCheck: null, LoggerAwareCanCheck: null),
            new(
                PropertyName: "UseSystemResourceKeys",
                RequiredValue: "true",
                CanCheck: null,
                LoggerAwareCanCheck: null
            ),
            new(
                PropertyName: "ValidateExecutableReferencesMatchSelfContained",
                RequiredValue: "true",
                CanCheck: static project => project.IsPublishable(),
                LoggerAwareCanCheck: null
            ),
        ];
    }

    private static IReadOnlyList<SimplePropertyCheck> GetStaticCodeAnalysisSimpleChecks()
    {
        return
        [
            new(PropertyName: "AnalysisLevel", RequiredValue: "latest", CanCheck: null, LoggerAwareCanCheck: null),
            new(
                PropertyName: "AnalysisMode",
                RequiredValue: "AllEnabledByDefault",
                CanCheck: null,
                LoggerAwareCanCheck: null
            ),
            new(
                PropertyName: "CodeAnalysisTreatWarningsAsErrors",
                RequiredValue: "true",
                CanCheck: null,
                LoggerAwareCanCheck: null
            ),
            new(PropertyName: "EnableNETAnalyzers", RequiredValue: "true", CanCheck: null, LoggerAwareCanCheck: null),
            new(
                PropertyName: "EnforceCodeStyleInBuild",
                RequiredValue: "true",
                CanCheck: null,
                LoggerAwareCanCheck: null
            ),
            new(
                PropertyName: "Features",
                RequiredValue: "strict;flow-analysis",
                CanCheck: null,
                LoggerAwareCanCheck: null
            ),
            new(PropertyName: "NuGetAudit", RequiredValue: "true", CanCheck: null, LoggerAwareCanCheck: null),
            new(
                PropertyName: "SuppressTrimAnalysisWarnings",
                RequiredValue: "false",
                CanCheck: static project => project.HasProperty("SuppressTrimAnalysisWarnings"),
                LoggerAwareCanCheck: null
            ),
        ];
    }

    private static IReadOnlyList<SimplePropertyCheck> GetUnitTestSimpleChecks()
    {
        return
        [
            new(
                PropertyName: "UseMicrosoftTestingPlatformRunner",
                RequiredValue: "true",
                CanCheck: null,
                LoggerAwareCanCheck: static (project, logger) =>
                    project.IsTestProject(logger)
                    && (
                        project.ReferencesPackage("xunit.v3", logger)
                        || project.ReferencesPackage("xunit.v3.extensibility.core", logger)
                    )
            ),
            new(
                PropertyName: "OutputType",
                RequiredValue: "Exe",
                CanCheck: null,
                LoggerAwareCanCheck: static (project, logger) =>
                    project.IsTestProject(logger) && project.ReferencesPackage("xunit.v3", logger)
            ),
        ];
    }

    private static void AddSimplePropertyChecks(
        this IServiceCollection services,
        IReadOnlyList<SimplePropertyCheck> registrations
    )
    {
        foreach (SimplePropertyCheck registration in registrations)
        {
            services.AddSimplePropertyCheck(registration);
        }
    }

    private static IServiceCollection AddSimplePropertyCheck(
        this IServiceCollection services,
        SimplePropertyCheck registration
    )
    {
        Console.WriteLine($"* Project Check: {registration.PropertyName}");

        return services.AddSingleton<IProjectCheck>(sp =>
        {
            ILogger<SimplePropertyProjectCheckBase> logger = sp.GetRequiredService<
                ILogger<SimplePropertyProjectCheckBase>
            >();

            return new SimplePropertyProjectCheckBase(
                propertyName: registration.PropertyName,
                requiredValue: registration.RequiredValue,
                canCheck: registration.LoggerAwareCanCheck is null
                    ? registration.CanCheck
                    : project => registration.LoggerAwareCanCheck(project, logger),
                logger: logger
            );
        });
    }

    private static IServiceCollection AddMustNotDefinePropertyCheck(
        this IServiceCollection services,
        string propertyName
    )
    {
        Console.WriteLine($"* Project Check: {propertyName}");

        return services.AddSingleton<IProjectCheck>(sp => new MustNotDefinePropertyProjectCheckBase(
            propertyName: propertyName,
            logger: sp.GetRequiredService<ILogger<MustNotDefinePropertyProjectCheckBase>>()
        ));
    }

    private static bool IsTestHarnessExecutable(in ProjectContext project)
    {
        return project.Name.EndsWith(value: ".TestHarness", comparisonType: StringComparison.OrdinalIgnoreCase)
            && StringComparer.OrdinalIgnoreCase.Equals(x: project.GetOutputType(), y: "Exe");
    }

    [DebuggerDisplay("{PropertyName,nq}")]
    private readonly record struct SimplePropertyCheck(
        string PropertyName,
        string RequiredValue,
        Func<ProjectContext, bool>? CanCheck,
        Func<ProjectContext, ILogger, bool>? LoggerAwareCanCheck
    );
}
