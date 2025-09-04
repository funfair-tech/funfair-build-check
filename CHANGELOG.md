# Changelog
All notable changes to this project will be documented in this file.

<!--
Please ADD ALL Changes to the UNRELEASED SECTION and not a specific release
-->

## [Unreleased]
### Added
### Fixed
### Changed
- Dependencies - Updated Meziantou.Analyzer to 2.0.215
- Dependencies - Updated Microsoft.Sbom.Targets to 4.1.2
### Removed
### Deployment Changes

<!--
Releases that have at least been deployed to staging, BUT NOT necessarily released to live.  Changes should be moved from [Unreleased] into here as they are merged into the appropriate release branch
-->
## [474.1.6] - 2025-09-02
### Changed
- Dependencies - Updated xunit.v3 to 3.0.1
- Dependencies - Updated Microsoft.Sbom.Targets to 4.1.1
- Dependencies - Updated Meziantou.Analyzer to 2.0.213
- Dependencies - Updated Philips.CodeAnalysis.MaintainabilityAnalyzers to 1.9.1
- Dependencies - Updated FunFair.CodeAnalysis to 7.1.23.1420
- Excluded auto-generated projects

## [474.1.5] - 2025-08-16
### Added
- Check for duplicate properties

## [474.1.4] - 2025-08-15
### Added
- Check for <WarnOnPackingNonPackableProject> property on non-packable projects so can build with warnings as errors without getting errors saying that the explicitly <IsPackable>false</IsPackable> project couldn't be packed
### Changed
- Dependencies - Updated Philips.CodeAnalysis.MaintainabilityAnalyzers to 1.8.0

## [474.1.3] - 2025-08-14
### Changed
- Dependencies - Updated Nullable.Extended.Analyzer to 1.15.6581
- Dependencies - Updated NuGet to 6.14.0
- Dependencies - Updated Microsoft.VisualStudio.Threading.Analyzers to 17.14.15
- Dependencies - Updated Microsoft.NET.Test.Sdk to 17.14.1
- Dependencies - Updated xunit.analyzers to 1.23.0
- Dependencies - Updated xunit.v3 to 3.0.0
- Dependencies - Updated SonarAnalyzer.CSharp to 10.15.0.120848
- Dependencies - Updated Microsoft.Sbom.Targets to 4.1.0
- Dependencies - Updated Roslynator.Analyzers to 4.14.0
- Dependencies - Updated Philips.CodeAnalysis.DuplicateCodeAnalyzer to 1.6.4
- Dependencies - Updated Microsoft.Extensions to 9.0.8
- Dependencies - Updated Meziantou.Analyzer to 2.0.212
- SDK - Updated DotNet SDK to 9.0.304
- Dependencies - Updated Philips.CodeAnalysis.MaintainabilityAnalyzers to 1.7.0
- Dependencies - Updated Credfeto.Enumeration to 1.2.127.1366
- Dependencies - Updated FunFair.Test.Common to 6.1.285.1620
- Dependencies - Updated FunFair.CodeAnalysis to 7.1.22.1366
- Dependencies - Updated Credfeto.Version.Information.Generator to 1.0.114.790
- Switched Must user open api analyzers to must not as building with net10 reports warnings The IncludeOpenAPIAnalyzers property and its associated MVC API analyzers are deprecated and will be removed in a future release.

## [474.1.2] - 2025-04-26
### Changed
- Dependencies - Updated Credfeto.Enumeration to 1.2.79.1071
- Dependencies - Updated Credfeto.Version.Information.Generator to 1.0.66.501
- Dependencies - Updated Meziantou.Analyzer to 2.0.199
### Removed
- CSharpier.MSBuild as it does too much e.g. formats projects when don't want it to

## [474.1.1] - 2025-04-24
### Fixed
- Case sensitivity issues
### Changed
- Dependencies - Updated xunit.analyzers to 1.21.0
- Dependencies - Updated xunit.v3 to 2.0.1
- Dependencies - Updated SonarAnalyzer.CSharp to 10.8.0.113526
- Dependencies - Updated Microsoft.Extensions to 9.0.4
- SDK - Updated DotNet SDK to 9.0.203
- Dependencies - Updated FunFair.CodeAnalysis to 7.0.79.1041
- Dependencies - Updated FunFair.Test.Common to 6.1.268.1269
- Dependencies - Updated Meziantou.Analyzer to 2.0.197
- Dependencies - Updated Credfeto.Enumeration to 1.2.77.1054
- Dependencies - Updated Credfeto.Version.Information.Generator to 1.0.64.491
- Dependencies - Updated CSharpier.MSBuild to 1.0.0

## [474.1.0] - 2025-03-24
### Added
- Check for CSharpier.MSBuild
### Changed
- Dependencies - Updated Nullable.Extended.Analyzer to 1.15.6495
- Dependencies - Updated xunit to 2.9.3
- Dependencies - Updated Microsoft.Sbom.Targets to 3.1.0
- Dependencies - Updated xunit.analyzers to 1.20.0
- Dependencies - Updated xunit.runner.visualstudio to 3.0.2
- Dependencies - Updated Microsoft.NET.Test.Sdk to 17.13.0
- Dependencies - Updated Roslynator.Analyzers to 4.13.1
- Dependencies - Updated SonarAnalyzer.CSharp to 10.7.0.110445
- Dependencies - Updated NuGet to 6.13.2
- Dependencies - Updated Credfeto.Version.Information.Generator to 1.0.56.380
- Dependencies - Updated Microsoft.VisualStudio.Threading.Analyzers to 17.13.61
- Dependencies - Updated Microsoft.Extensions to 9.0.3
- Dependencies - Updated Credfeto.Enumeration to 1.2.70.976
- SDK - Updated DotNet SDK to 9.0.202
- Dependencies - Updated Meziantou.Analyzer to 2.0.189
- Dependencies - Updated FunFair.CodeAnalysis to 7.0.73.978
- Dependencies - Updated FunFair.Test.Common to 6.1.265.1215
- Xunit v3 tests

## [474.0.22] - 2024-12-17
### Added
- Check for projects having the Microsoft SBOM tool
- Check for GenerateSBOM setting on packable projects
### Changed
- Dependencies - Updated ThisAssembly.AssemblyInfo to 1.5.0
- SDK - Update DotNet to 9.0 RC1
- Dependencies - Updated xunit to 2.9.2
- Dependencies - Updated Roslynator.Analyzers to 4.12.9
- Dependencies - Updated NSubstitute to 5.3.0
- Dependencies - Updated xunit.analyzers to 1.17.0
- Dependencies - Updated FunFair.CodeAnalysis to 7.0.32.711
- Dependencies - Updated Microsoft.Extensions to 9.0.0
- Dependencies - Updated Microsoft.VisualStudio.Threading.Analyzers to 17.12.19
- Dependencies - Updated NuGet to 6.12.1
- Dependencies - Updated FunFair.Test.Common to 6.1.239.979
- Dependencies - Updated Microsoft.NET.Test.Sdk to 17.12.0
- Dependencies - Updated SonarAnalyzer.CSharp to 10.3.0.106239
- Dependencies - Updated Meziantou.Analyzer to 2.0.182
- Dependencies - Updated Credfeto.Enumeration to 1.2.40.746
- Dependencies - Updated Credfeto.Version.Information.Generator to 1.0.28.176
- SDK - Updated DotNet SDK to 9.0.101

## [474.0.21] - 2024-07-12
### Added
- Check of NuGetAuditLevel
### Changed
- Dependencies - Updated Credfeto.Enumeration to 1.1.7.384
- Dependencies - Updated Microsoft.VisualStudio.Threading.Analyzers to 17.10.48
- Dependencies - Updated Microsoft.NET.Test.Sdk to 17.10.0
- Dependencies - Updated Roslynator.Analyzers to 4.12.4
- Dependencies - Updated FunFair.CodeAnalysis to 7.0.18.436
- Dependencies - Updated NuGet to 6.10.1
- Dependencies - Updated CSharpIsNullAnalyzer to 0.1.593
- Dependencies - Updated xunit.analyzers to 1.15.0
- Dependencies - Updated xunit.runner.visualstudio to 2.8.2
- Dependencies - Updated xunit to 2.9.0
- SDK - Updated DotNet SDK to 8.0.303
- Dependencies - Updated Meziantou.Analyzer to 2.0.160
- Dependencies - Updated Microsoft.Extensions to 8.0.2
- Dependencies - Updated FunFair.Test.Common to 6.1.62.556
- Dependencies - Updated SonarAnalyzer.CSharp to 9.29.0.95321

## [474.0.20] - 2024-04-23
### Changed
- Dependencies - Updated Credfeto.Enumeration to 1.1.6.354
- Dependencies - Updated SmartAnalyzers.CSharpExtensions.Annotations to 4.2.11
- Dependencies - Updated FunFair.CodeAnalysis to 7.0.14.369
- Dependencies - Updated FunFair.Test.Common to 6.1.51.455
- Dependencies - Updated SonarAnalyzer.CSharp to 9.24.0.89429
### Removed
- Removed test link to coverlet
- Removed test link to teamcity test adapter

## [474.0.19] - 2024-04-18
### Changed
- Dependencies - Updated coverlet to 6.0.2
- Dependencies - Updated Microsoft.Extensions to 8.0.1
- Dependencies - Updated Credfeto.Enumeration.Source.Generation to 1.1.5.315
- SDK - Updated DotNet SDK to 8.0.204
- Dependencies - Updated FunFair.CodeAnalysis to 7.0.13.341
- Dependencies - Updated SonarAnalyzer.CSharp to 9.23.2.88755
- Dependencies - Updated xunit.analyzers to 1.12.0
- Dependencies - Updated xunit.runner.visualstudio to 2.5.8
- Dependencies - Updated xunit to 2.7.1
- Dependencies - Updated FunFair.Test.Common to 6.1.50.439
- Dependencies - Updated Roslynator.Analyzers to 4.12.1
- Dependencies - Updated Meziantou.Analyzer to 2.0.149

## [474.0.18] - 2024-02-26
### Added
- Publishable check for EnableRequestDelegateGenerator
- Publishable check for PublishTrimmed when EnableTrimAnalyzer=true
- Check for EnableTrimAnalyzer=true|false
### Changed
- Dependencies - Updated Nullable.Extended.Analyzer to 1.15.6169
- Dependencies - Updated Microsoft.Extensions to 8.0.1
- Dependencies - Updated ThisAssembly.AssemblyInfo to 1.4.3
- Dependencies - Updated Microsoft.NET.Test.Sdk to 17.9.0
- Dependencies - Updated Microsoft.VisualStudio.Threading.Analyzers to 17.9.28
- Dependencies - Updated Credfeto.Enumeration.Source.Generation to 1.1.2.267
- Dependencies - Updated FunFair.CodeAnalysis to 7.0.8.274
- Dependencies - Updated FunFair.Test.Common to 6.1.41.357
- Dependencies - Updated NSubstitute.Analyzers.CSharp to 1.0.17
- Dependencies - Updated NuGet to 6.9.1
- Dependencies - Updated xunit.analyzers to 1.11.0
- Dependencies - Updated xunit.runner.visualstudio to 2.5.7
- Dependencies - Updated xunit to 2.7.0
- SDK - Updated DotNet SDK to 8.0.201
- Dependencies - Updated Roslynator.Analyzers to 4.11.0
- Dependencies - Updated SonarAnalyzer.CSharp to 9.20.0.85982
- Dependencies - Updated coverlet to 6.0.1
- Dependencies - Updated TeamCity.VSTest.TestAdapter to 1.0.40
- Dependencies - Updated Meziantou.Analyzer to 2.0.145

## [474.0.17] - 2023-12-24
### Changed
- Passed in ICheckConfiguration into the check runner

## [474.0.16] - 2023-12-24
### Changed
- Dependencies - Updated Roslynator.Analyzers to 4.7.0
- Dependencies - Updated Nullable.Extended.Analyzer to 1.14.6129
- Dependencies - Updated FunFair.Test.Common to 6.1.23.276
- Dependencies - Updated SmartAnalyzers.CSharpExtensions.Annotations to 4.2.9
- Dependencies - Updated TeamCity.VSTest.TestAdapter to 1.0.39
- Used common base class for simple PropertyGroup value checks
- Made project loading async
- Dependencies - Updated Meziantou.Analyzer to 2.0.127
- Dependencies - Updated Philips.CodeAnalysis.MaintainabilityAnalyzers to 1.5.0
- Dependencies - Updated SonarAnalyzer.CSharp to 9.16.0.82469
- Dependencies - Updated xunit.analyzers to 1.8.0
- Dependencies - Updated xunit to 2.6.4
- Dependencies - Updated xunit.runner.visualstudio to 2.5.6

## [474.0.15] - 2023-11-18
### Changed
- Dependencies - Updated NuGet to 6.8.0

## [474.0.14] - 2023-11-17
### Changed
- Dependencies - Updated Credfeto.Enumeration.Source.Generation to 1.1.1.168
- Dependencies - Updated FunFair.Test.Common to 6.1.19.238

## [474.0.13] - 2023-11-15
### Changed
- Dependencies - Updated Roslynator.Analyzers to 4.6.2
- Dependencies - Updated xunit.analyzers to 1.5.0
- Dependencies - Updated Philips.CodeAnalysis.MaintainabilityAnalyzers to 1.4.0
- SDK - Updated DotNet SDK to 8.0.100
- Dependencies - Updated Microsoft.Extensions to 8.0.0
- Dependencies - Updated FunFair.CodeAnalysis to 7.0.4.198
- Dependencies - Updated Meziantou.Analyzer to 2.0.110
- Dependencies - Updated Microsoft.VisualStudio.Threading.Analyzers to 17.8.14
- Dependencies - Updated FunFair.Test.Common to 6.1.18.233

## [474.0.12] - 2023-11-10
### Changed
- Dependencies - Updated SonarAnalyzer.CSharp to 9.12.0.78982
- Dependencies - Updated xunit.analyzers to 1.4.0
- Dependencies - Updated xunit.runner.visualstudio to 2.5.3
- Dependencies - Updated TeamCity.VSTest.TestAdapter to 1.0.38
- Dependencies - Updated Roslynator.Analyzers to 4.6.1
- Dependencies - Updated Philips.CodeAnalysis.MaintainabilityAnalyzers to 1.3.1
- Dependencies - Updated xunit to 2.6.1
- Dependencies - Updated Microsoft.NET.Test.Sdk to 17.8.0
- Dependencies - Updated Meziantou.Analyzer to 2.0.106
- Dependencies - Updated FunFair.Test.Common to 6.1.16.218

## [474.0.11] - 2023-10-12
### Changed
- Dependencies - Updated xunit.analyzers to 1.3.0
- Dependencies - Updated xunit to 2.5.1
- Dependencies - Updated xunit.runner.visualstudio to 2.5.1
- Dependencies - Updated SonarAnalyzer.CSharp to 9.11.0.78383
- Dependencies - Updated Meziantou.Analyzer to 2.0.93
- SDK - Updated DotNet SDK to 8.0.100-rc.2.23502.2
- Dependencies - Updated FunFair.Test.Common to 6.1.12.182

## [474.0.10] - 2023-09-16
### Changed
- Dependencies - Updated FunFair.Test.Common to 6.1.10.156
- SDK - Updated DotNet SDK to 8.0.100-rc.1.23455.8
- Dependencies - Updated FunFair.CodeAnalysis to 7.0.3.138
- Dependencies - Updated SonarAnalyzer.CSharp to 9.10.0.77988

## [474.0.9] - 2023-09-14
### Changed
- Dependencies - Updated SonarAnalyzer.CSharp to 9.9.0.77355
- Dependencies - Updated NSubstitute to 5.1.0
- SDK - Updated DotNet SDK to 7.0.401
- Dependencies - Updated Meziantou.Analyzer to 2.0.85
- Dependencies - Updated FunFair.Test.Common to 6.1.9.149
- Dependencies - Updated Credfeto.Enumeration.Source.Generation to 1.1.0.138

## [474.0.8] - 2023-08-30
### Changed
- Dependencies - Updated FunFair.Test.Common to 6.1.7.129
- Dependencies - Updated SonarAnalyzer.CSharp to 9.8.0.76515
- Dependencies - Updated TeamCity.VSTest.TestAdapter to 1.0.37
- Dependencies - Updated Roslynator.Analyzers to 4.5.0
- Dependencies - Updated FunFair.CodeAnalysis to 7.0.2.121
- Dependencies - Updated Microsoft.NET.Test.Sdk to 17.7.2
- Dependencies - Updated Credfeto.Enumeration.Source.Generation to 1.0.16.111

## [474.0.7] - 2023-08-18
### Changed
- Dependencies - Updated Meziantou.Analyzer to 2.0.82
- Dependencies - Updated Microsoft.NET.Test.Sdk to 17.7.1
- Dependencies - Updated FunFair.Test.Common to 6.1.6.127

## [474.0.6] - 2023-08-10
### Changed
- SDK - Updated DotNet SDK to 7.0.400
- Dependencies - Updated FunFair.CodeAnalysis to 7.0.1.87
- Dependencies - Updated Meziantou.Analyzer to 2.0.81
- Dependencies - Updated Roslynator.Analyzers to 4.4.0
- Dependencies - Updated SonarAnalyzer.CSharp to 9.7.0.75501
- Dependencies - Updated xunit.analyzers to 1.2.0
- Dependencies - Updated Microsoft.NET.Test.Sdk to 17.7.0
- Dependencies - Updated NonBlocking to 2.1.2
- Dependencies - Updated xunit to 2.5.0
- Dependencies - Updated xunit.runner.visualstudio to 2.5.0
- Dependencies - Updated FunFair.Test.Common to 6.1.5.120
- Dependencies - Updated Microsoft.VisualStudio.Threading.Analyzers to 17.7.30
- Dependencies - Updated NuGet to 6.7.0

## [474.0.5] - 2023-08-02
### Changed
- Dependencies - Updated Microsoft.Extensions to 7.0.1
- Dependencies - Updated NuGet to 6.6.1

## [474.0.4] - 2023-08-01
### Added
- Packages on new NuGet feed
### Removed
- Packages on MyGet feed as MyGet has disappeared completely

## [474.0.3] - 2023-06-20
### Removed
- Dotnet 8 preview

## [474.0.2] - 2023-06-02
### Changed
- SDK - Updated DotNet SDK to 7.0.203
- Dependencies - Updated Credfeto.Enumeration.Source.Generation to 1.0.9.588
- Dependencies - Updated Roslynator.Analyzers to 4.3.0
- Dependencies - Updated FunFair.CodeAnalysis to 7.0.0.18
- Dependencies - .NET 8 Preview 3
- Added EnableMicrosoftExtensionsConfigurationBinderSourceGenerator Policy
- Added JsonSerializerIsReflectionEnabledByDefault Policy
- Added OptimizationPreference Policy
- Dependencies - Updated Microsoft.VisualStudio.Threading.Analyzers to 17.6.40
- Dependencies - Updated NuGet to 6.6.0
- Dependencies - Updated coverlet to 6.0.0
- Dependencies - Updated Meziantou.Analyzer to 2.0.56
- Dependencies - Updated SonarAnalyzer.CSharp to 9.2.0.71021
- Dependencies - Updated FunFair.Test.Common to 6.1.1.49
- Dependencies - Updated Microsoft.NET.Test.Sdk to 17.6.1

## [474.0.1] - 2023-04-02
### Added
- Checks for nuget package symbol generation on packages
- Checks for nuget package symbol format on packages
- Checks for code analyzer extended checking on analyzer packages
### Changed
- Dependencies - Updated Credfeto.Enumeration.Source.Generation to 1.0.7.19
- Dependencies - Updated FunFair.Test.Common to 6.0.26.2754
- Dependencies - Updated Microsoft.Extensions to 7.0.4
- Dependencies - Updated Microsoft.NET.Test.Sdk to 17.5.0
- Dependencies - Updated NuGet to 6.5.0
- Dependencies - Updated SonarAnalyzer.CSharp to 8.55.0.65544
- Dependencies - Updated Meziantou.Analyzer to 2.0.29

## [474.0.0] - 2023-03-17
### Changed
- FF-1429 - Updated SmartAnalyzers.CSharpExtensions.Annotations to 4.2.8
- FF-1429 - Updated Meziantou.Analyzer to 2.0.14
- FF-1429 - Updated SonarAnalyzer.CSharp to 8.52.0.60960
- FF-1429 - Updated Microsoft.VisualStudio.Threading.Analyzers to 17.5.22
- FF-1429 - Updated NSubstitute to 5.0.0
- FF-1429 - Updated FunFair.Test.Common to 6.0.21.2653
- FF-1429 - Updated Microsoft.Extensions to 7.0.3
- FF-3881 - Updated DotNet SDK to 7.0.200
- SDK - Updated DotNet SDK to 7.0.202
- Forced version to be greater than what got accidentally uploaded, marked as do not use, but dotnet insists on installing

## [6.3.5] - 2023-01-22
### Added
- Special cases for non-abstraction packages that are ok to be used in test projects
### Changed
- FF-1429 - Updated FunFair.Test.Common to 6.0.17.2561
- FF-1429 - Updated Meziantou.Analyzer to 2.0.10
- FF-1429 - Updated NSubstitute.Analyzers.CSharp to 1.0.16

## [6.3.4] - 2023-01-19
### Changed
- FF-1429 - Updated Roslynator.Analyzers to 4.2.0
- FF-1429 - Updated xunit.analyzers to 1.1.0
- FF-1429 - Updated Microsoft.NET.Test.Sdk to 17.4.1
- FF-1429 - Updated SonarAnalyzer.CSharp to 8.51.0.59060
- FF-1429 - Updated NonBlocking to 2.1.1
- FF-1429 - Updated Microsoft.VisualStudio.Threading.Analyzers to 17.4.33
- FF-1429 - Updated Philips.CodeAnalysis.MaintainabilityAnalyzers to 1.2.32
- FF-1429 - Updated Microsoft.Extensions to 7.0.2
- FF-3881 - Updated DotNet SDK to 7.0.102
- FF-1429 - Updated Meziantou.Analyzer to 2.0.8
- FF-1429 - Updated Credfeto.Enumeration.Source.Generation to 1.0.5.17
- FF-1429 - Updated FunFair.Test.Common to 6.0.16.2535

## [6.3.3] - 2022-11-09
### Changed
- FF-1429 - Updated NuGet to 6.4.0

## [6.3.2] - 2022-11-09
### Changed
- FF-1429 - Updated Microsoft.VisualStudio.Threading.Analyzers to 17.4.27
- FF-1429 - Updated Meziantou.Analyzer to 1.0.746
- FF-1429 - Updated Philips.CodeAnalysis.MaintainabilityAnalyzers to 1.2.30
- Added special exclusion for test projects

## [6.3.1] - 2022-11-08
### Changed
- FF-1429 - Updated Microsoft.Extensions to 7.0.0
- FF-1429 - Updated Credfeto.Enumeration.Source.Generation to 0.0.7.9
- FF-1429 - Updated SonarAnalyzer.CSharp to 8.48.0.56517

## [6.3.0] - 2022-11-08
### Added
- Additional analyzers
- Requirements that analyzers be referenced with ExcludeAssets='runtime'
### Changed
- FF-1429 - Updated Microsoft.Extensions to 6.0.1
- FF-1429 - Updated NuGet to 6.3.1
- FF-1429 - Updated xunit to 2.4.2
- FF-1429 - Updated SonarAnalyzer.CSharp to 8.47.0.55603
- FF-1429 - Updated coverlet to 3.2.0
- FF-1429 - Updated Microsoft.NET.Test.Sdk to 17.4.0
- FF-1429 - Updated FunFair.Test.Common to 6.0.7.2278
- FF-1429 - Updated AsyncFixer to 1.6.0
- FF-1429 - Updated FunFair.CodeAnalysis to 5.9.0.1493
- FF-1429 - Updated Meziantou.Analyzer to 1.0.745
- FF-1429 - Updated Microsoft.VisualStudio.Threading.Analyzers to 17.3.48
- FF-1429 - Updated Philips.CodeAnalysis.DuplicateCodeAnalyzer to 1.1.7
- FF-1429 - Updated Philips.CodeAnalysis.MaintainabilityAnalyzers to 1.2.29
- FF-1429 - Updated Roslynator.Analyzers to 4.1.2
- FF-1429 - Updated SecurityCodeScan.VS2019 to 5.6.7
- FF-1429 - Updated SmartAnalyzers.CSharpExtensions.Annotations to 4.2.7

## [6.2.0] - 2022-09-20
### Added
- Dotnet 7 specific checks
### Changed
- FF-1429 - Updated Microsoft.Extensions to 6.0.2
- FF-1429 - Updated Meziantou.Analyzer to 1.0.730

## [6.1.0] - 2022-09-11
### Changed
- FF-1429 - Updated FunFair.Test.Common to 6.0.0.1932
- FF-1429 - Updated NSubstitute to 4.4.0
- FF-3881 - Updated DotNet SDK to 6.0.302
- FF-1429 - Updated FunFair.Test.Common to 6.0.1.1951
- FF-1429 - Updated SmartAnalyzers.CSharpExtensions.Annotations to 4.2.7
- FF-1429 - Updated xunit.analyzers to 1.0.0
- FF-1429 - Updated SonarAnalyzer.CSharp to 8.43.0.51858
- FF-3881 - Updated DotNet SDK to 6.0.400
- FF-1429 - Updated NuGet to 6.3.0
- FF-1429 - Updated Microsoft.VisualStudio.Threading.Analyzers to 17.3.44
- FF-1429 - Updated SonarAnalyzer.CSharp to 8.44.0.52574
- FF-1429 - Updated Microsoft.NET.Test.Sdk to 17.3.1
- FF-1429 - Updated SecurityCodeScan.VS2019 to 5.6.7
- FF-1429 - Updated FunFair.CodeAnalysis to 5.9.0.1493
- Added DOTNET_SDK_ALLOW_PRE_RELEASE environment variable override to allow pre-release versions

## [6.0.0] - 2022-07-05
### Added
- Pre-compiled json parsing
### Changed
- FF-1429 - Updated SmartAnalyzers.CSharpExtensions.Annotations to 4.2.5
- FF-1429 - Updated Roslynator.Analyzers to 4.1.1
- FF-1429 - Updated SecurityCodeScan.VS2019 to 5.6.3
- FF-1429 - Updated NuGet to 6.2.1
- FF-3881 - Updated DotNet SDK to 6.0.301
- FF-1429 - Updated FunFair.CodeAnalysis to 5.8.3.1353
- FF-1429 - Updated NonBlocking to 2.1.0
- FF-1429 - Updated Meziantou.Analyzer to 1.0.704
- FF-1429 - Updated FunFair.Test.Common to 5.9.10.1914
- FF-1429 - Updated SonarAnalyzer.CSharp to 8.41.0.50478
### Removed
- Support for Dotnet 5.0 builds

## [3.9.2] - 2022-05-18
### Added
- Using Microsoft.AspNetCore.Authentication.JwtBearer package should include System.IdentityModel.Tokens.Jwt
- Enabled AOT Compilation
- Configurable XMLDOC comment requirements.
### Changed
- FF-1429 - Updated NSubstitute to 4.3.0
- FF-1429 - Updated coverlet to 3.1.2
- FF-1429 - Updated FunFair.CodeAnalysis to 5.8.1.1203
- FF-1429 - Updated NuGet to 6.1.0
- FF-1429 - Updated TeamCity.VSTest.TestAdapter to 1.0.36
- FF-1429 - Updated SecurityCodeScan.VS2019 to 5.6.2
- FF-1429 - Updated SmartAnalyzers.CSharpExtensions.Annotations to 4.2.2
- FF-1429 - Updated NonBlocking to 2.0.0
- FF-1429 - Updated FunFair.Test.Common to 5.9.4.1729
- FF-1429 - Updated Roslynator.Analyzers to 4.1.0
- FF-1429 - Updated Meziantou.Analyzer to 1.0.701
- FF-1429 - Updated SonarAnalyzer.CSharp to 8.38.0.46746
- FF-1429 - Updated xunit.runner.visualstudio to 2.4.5
- FF-1429 - Updated Microsoft.VisualStudio.Threading.Analyzers to 17.2.32
- FF-1429 - Updated AsyncFixer to 1.6.0
- FF-1429 - Updated Microsoft.NET.Test.Sdk to 17.2.0
- FF-3881 - Updated DotNet SDK to 6.0.300

## [3.9.1] - 2021-12-17
### Changed
- FF-1429 - Updated SonarAnalyzer.CSharp to 8.33.0.40503
- FF-1429 - Updated TeamCity.VSTest.TestAdapter to 1.0.30
- FF-1429 - Updated FunFair.Test.Common to 5.7.2.1514
- FF-3881 - Updated DotNet SDK to 6.0.101
- Obsoleted package message where its been obsoleted with an SDK reference

## [3.9.0] - 2021-12-03
### Added
- Check that packable libraries do not depend on non-packable libraries.
### Changed
- FF-1429 - Updated Roslynator.Analyzers to 3.3.0
- FF-1429 - Updated FunFair.Test.Common to 5.7.0.1478
- FF-1429 - Updated TeamCity.VSTest.TestAdapter to 1.0.29
- FF-1429 - Updated FunFair.CodeAnalysis to 5.7.3.1052

## [3.8.0] - 2021-11-22
### Added
- Check to see if web.config transforms are enabled for web library projects
- Check to see if Neutral Resources Language is set properly
- Check to see if Package Validation is set properly on packable projects
- Checks to see if the Validate executable references match property is set on publishable projects.
- Checks to see if the implicit usings property is set on publishable projects.
### Changed
- FF-1429 - Updated Microsoft.VisualStudio.Threading.Analyzers to 17.0.64
- FF-1429 - Updated to Dotnet SDK 5.0.403
- FF-1429 - Updated FunFair.Test.Common to 5.6.4.1351
- FF-1429 - Updated NuGet to 6.0.0
- FF-1429 - Updated FunFair.CodeAnalysis to 5.7.0.995
- FF-1429 - Updated NSubstitute.Analyzers.CSharp to 1.0.15
- FF-1429 - Updated SonarAnalyzer.CSharp to 8.32.0.39516
- FF-3856 - Updated to DotNet 6.0 with DotNet 5.0 fallback

## [3.7.0] - 2021-11-08
### Added
- Check for explicitly removed classes from project
### Changed
- FF-1429 - Updated FunFair.CodeAnalysis to 5.6.1.965
- FF-1429 - Updated FunFair.Test.Common to 5.6.3.1339

## [3.6.0] - 2021-11-02
### Added
- Check that NSubstitute is used not Moq
### Changed
- FF-1429 - Updated SonarAnalyzer.CSharp to 8.30.0.37606
- FF-1429 - Updated Microsoft.NET.Test.Sdk to 17.0.0
- FF-1429 - Updated FunFair.CodeAnalysis to 5.6.0.956

## [3.5.0] - 2021-09-24
### Added
- Check that Microsoft.VisualStudio.Threading is not used.
### Changed
- FF-1429 - Updated Microsoft.NET.Test.Sdk to 16.11.0
- FF-1429 - Updated NuGet to 5.11.0
- FF-1429 - Updated Roslynator.Analyzers to 3.2.2
- FF-1429 - Updated FunFair.CodeAnalysis to 5.4.0.915
- FF-1429 - Updated SonarAnalyzer.CSharp to 8.28.0.36354
- FF-1429 - Updated SonarAnalyzer.CSharp to 8.29.0.36737
- FF-1429 - Updated FunFair.CodeAnalysis to 5.5.0.926
- FF-1429 - Updated TeamCity.VSTest.TestAdapter to 1.0.27
- FF-1429 - Updated Microsoft.VisualStudio.Threading.Analyzers to 17.0.63

## [3.4.0] - 2021-08-12
### Added
- Check for RuntimeIdentifiers being set on anything that is publishable.
### Changed
- FF-1429 - Updated coverlet to 3.1.0
- FF-1429 - Updated FunFair.CodeAnalysis to 5.2.4.854
- FF-1429 - Updated FunFair.Test.Common to 5.5.0.1192
- FF-1429 - Updated FunFair.Test.Common to 5.5.0.1195
- FF-1429 - Updated FunFair.CodeAnalysis to 5.2.5.870
- FF-1429 - Updated FunFair.CodeAnalysis to 5.3.0.879
- FF-1429 - Updated SonarAnalyzer.CSharp to 8.27.0.35380

## [3.3.0] - 2021-07-15
### Added
- Tests for IsPublishable
- Checks that test projects reference coverlet.collector and coverlet.msbuild
### Changed
- FF-1429 - Updated SonarAnalyzer.CSharp to 8.26.0.34506
- FF-1429 - Updated FunFair.CodeAnalysis to 5.2.3.837
- FF-1429 - Updated Microsoft.Extensions to 5.0.2
- FF-1429 - Updated TeamCity.VSTest.TestAdapter to 1.0.26

## [3.2.1] - 2021-07-07
### Added
- Checks for metadata on packable projects - Description, RepositoryUrl, PackageTags
- Checks for common metadata import on packable projects via DOTNET_PACK_PROJECT_METADATA_IMPORT environment variable
### Changed
- FF-1429 - Updated Roslynator.Analyzers to 3.2.0
- FF-1429 - Updated NuGet to 5.10.0
- FF-1429 - Updated SonarAnalyzer.CSharp to 8.25.0.33663
- FF-1429 - Updated FunFair.Test.Common to 5.4.0.1031
- FF-1429 - Updated FunFair.CodeAnalysis to 5.2.1.809

## [3.2.0] - 2021-06-11
### Changed
- FF-1429 - Updated SonarAnalyzer.CSharp to 8.24.0.32949
- Corrected paths on linux
- SET SOLUTION_FILENAME environment variable when running under Teamcity

## [3.1.0] - 2021-06-03
### Changed
- FF-1429 - Updated coverlet to 3.0.3
- FF-1429 - Updated Microsoft.NET.Test.Sdk to 16.9.1
- FF-1429 - Updated SonarAnalyzer.CSharp to 8.19.0.28253
- FF-1429 - Updated Microsoft.VisualStudio.Threading.Analyzers to 16.9.54
- FF-1429 - Updated NuGet to 5.9.0
- FF-1429 - Updated Microsoft.VisualStudio.Threading.Analyzers to 16.9.60
- FF-1429 - Updated FunFair.Test.Common to 5.1.2.864
- FF-1429 - Updated SonarAnalyzer.CSharp to 8.20.0.28934
- FF-1429 - Updated FunFair.Test.Common to 5.2.0.886
- FF-1429 - Updated Microsoft.NET.Test.Sdk to 16.9.4
- FF-1429 - Updated FunFair.Test.Common to 5.3.0.920
- FF-1429 - Updated NuGet to 5.9.1
- FF-1429 - Updated SonarAnalyzer.CSharp to 8.21.0.30542
- FF-1429 - Updated SonarAnalyzer.CSharp to 8.22.0.31243
- FF-1429 - Updated FunFair.CodeAnalysis to 5.2.0.740
- FF-1429 - Updated SonarAnalyzer.CSharp to 8.23.0.32424
- FF-1429 - Updated Microsoft.NET.Test.Sdk to 16.10.0
- FF-1429 - Updated Microsoft.VisualStudio.Threading.Analyzers to 16.10.56

## [2.3.0] - 2021-02-17
### Added
- Checks to see that projects that are packed to nuget packages do not reference *.All packages
### Changed
- FF-1429 - Updated TeamCity.VSTest.TestAdapter to 1.0.25

## [2.2.0] - 2021-02-12
### Added
- Changed global.json rollForward policy
### Changed
- FF-1429 - Updated Roslynator.Analyzers to 3.1.0
- FF-1429 - Updated SonarAnalyzer.CSharp to 8.18.0.27296
- FF-1429 - Updated FunFair.CodeAnalysis to 5.1.0.658

## [2.1.0] - 2021-01-27
### Added
- Force use of abstrations package for Microsoft.Extensions.Caching.Memory in non-exe's
- Added new Setting DOTNET_PACKABLE with options for NONE, ALL, LIBRARIES, LIBRARY_TOOL, TOOLS or a comma separated list of project names
### Changed
- FF-1429 - Updated SonarAnalyzer.CSharp to 8.15.0.24505
- FF-1429 - Updated Microsoft.Extensions to 5.0.1
- FF-1429 - Updated SonarAnalyzer.CSharp to 8.16.0.25740
- FF-1429 - Updated FunFair.CodeAnalysis to 5.0.0.619
- FF-1429 - Updated AsyncFixer to 1.4.0
- FF-1429 - Updated AsyncFixer to 1.4.1
- FF-1429 - Updated SonarAnalyzer.CSharp to 8.17.0.26580
- FF-1429 - Updated AsyncFixer to 1.5.1

## [2.0.0] 2020-11-20
### Added
- Check that project exists when using project references.
- Check that a library does not depend on an executable.
- Check that Microsoft.CodeAnalysis.FxCopAnalyzers is not enabled for .net 5.0 targets
- Check that Microsoft.Extensions.Hosting is referenced as an abstractions package rather than a full package
### Changed
- FF-1429 - Updated Microsoft.Extensions to 5.0.0
- FF-1429 - Updated Microsoft.VisualStudio.Threading.Analyzers to 16.8.55
- FF-1429 - Updated FunFair.CodeAnalysis to 1.15.0.542
- FF-1429 - Updated Microsoft.CodeAnalysis.FxCopAnalyzers to 3.3.1
- FF-1429 - Updated Microsoft.VisualStudio.Threading.Analyzers to 16.8.51
- FF-1429 - Updated Microsoft.VisualStudio.Threading.Analyzers to 16.8.50
- FF-1429 - Updated FunFair.CodeAnalysis to 1.15.0.518
- Converted to .NET 5.0

## [1.19.0] 2020-11-10
### Added
- Check that project exists when using project references.
- Check that a library does not depend on an executable.
- Check that Microsoft.CodeAnalysis.FxCopAnalyzers is not enabled for .net 5.0 targets
### Changed
- FF-1429 - Updated Microsoft.VisualStudio.Threading.Analyzers to 16.8.55
- FF-1429 - Updated FunFair.CodeAnalysis to 1.15.0.542
- FF-1429 - Updated Microsoft.CodeAnalysis.FxCopAnalyzers to 3.3.1
- FF-1429 - Updated Microsoft.VisualStudio.Threading.Analyzers to 16.8.51
- FF-1429 - Updated Microsoft.VisualStudio.Threading.Analyzers to 16.8.50
- FF-1429 - Updated FunFair.CodeAnalysis to 1.15.0.518

## [1.18.0] 2020-10-14
### Changed
- FF-1429 - Updated SonarAnalyzer.CSharp to 8.14.0.22654
- FF-1429 - Updated FunFair.CodeAnalysis to 1.14.0.468
- FF-1429 - Updated FunFair.CodeAnalysis to 1.13.0.452
- FF-2930 - Updated to .net core 3.1.403

## [1.17.0] 2020-09-25
### Added
- Check for missing projects in solution.
- Check for extra projects that aren't in the solution.
### Changed
- FF-1429 - Updated FunFair.CodeAnalysis to 1.12.0.445
- FF-1429 - Updated SonarAnalyzer.CSharp to 8.13.1.21947

## [1.16.0] 2020-09-16
### Added
- Checks that DocumentationFile is defined in a ways that it doesn't contain things that change on a per .net core version basis.

## [1.15.0] 2020-09-15
### Added
- Checks for unexpected use of ToString where it would output the class name rather than a readable per instance value.
### Changed
- FF-1429 - Updated SonarAnalyzer.CSharp to 8.13.0.21683

## [1.14.0] 2020-09-09
### Changed
- FF-2830 - Update all the .NET components to .NET Core 3.1.402
- FF-1429 - Updated FunFair.CodeAnalysis to 1.11.0.424
- FF-1429 - Updated Roslynator.Analyzers to 3.0.0
- FF-1429 - Updated Microsoft.VisualStudio.Threading.Analyzers to 16.7.56
- FF-1429 - Updated FunFair.CodeAnalysis to 1.10.0.414
- FF-1429 - Updated SonarAnalyzer.CSharp to 8.12.0.21095
- FF-1429 - Updated FunFair.CodeAnalysis to 1.9.0.394

## [1.13.0] 2020-08-12
### Changed
- FF-1429 - Updated SonarAnalyzer.CSharp to 8.11.0.20529
- FF-1429 - Updated Microsoft.CodeAnalysis.FxCopAnalyzers to 3.3.0
- FF-1429 - Updated Microsoft.VisualStudio.Threading.Analyzers to 16.7.54
- FF-1429 - Updated SonarAnalyzer.CSharp to 8.10.0.19839
- FF-2759 - Updated to .net core 3.1.401

## [1.12.0] 2020-07-20
### Changed
- FF-1429 - Updated FunFair.CodeAnalysis to 1.8.0.375
- FF-1429 - Updated FunFair.CodeAnalysis to 1.7.2.364
- FF-1429 - Updated FunFair.CodeAnalysis to 1.7.1.352
- FF-1429 - Updated FunFair.CodeAnalysis to 1.7.0.347
- FF-1429 - Updated FunFair.CodeAnalysis to 1.6.0.343
- FF-1429 - Updated FunFair.CodeAnalysis to 1.6.0.339
- FF-1429 - Updated SonarAnalyzer.CSharp to 8.9.0.19135
- FF-1429 - Updated FunFair.CodeAnalysis to 1.5.0.314
- FF-2652 - Update all the .NET components to .NET Core 3.1.302

## [1.11.0] 2020-06-18
### Changed
- FF-2488 - Updated packages and global.json to net core 3.1.301
- FF-1429 - Updated SonarAnalyzer.CSharp to 8.8.0.18411
- FF-1429 - Updated AsyncFixer to 1.3.0
- FF-1429 - Updated AsyncFixer to 1.1.8
- FF-2386 - Update all the .NET components to .NET Core 3.1.202

## [1.10.0] 2020-04-27
### Added
- Explicit check for obsoleted packages (obsoleted between .net core 2.2 and .net net core 3.0)
- Explicit check for global.json sdk rollForward policy
- Switched from being a nuget tool to being a dotnet tool
### Changed
- FF-2127 - upgrade dotnet core to 3.1.201
- List of explicitly allowed warnings in <NoWarn> section to remove all the nullable warning codes

## [1.9.0] 2020-03-17
### Changed
- Additional check for Output type - must be present and either Exe or Library

## [1.8.0] 2020-02-24
### Changed
- FF-1910 - updated to net core sdk 3.1.102

## [1.7.0] 2020-02-14
### Changed
- Enabled additional async validation checks

## [1.6.0] 2019-12-18
- Switched from DisableDateTimeNow to FunFair.CodeAnalysis for checks

## [1.5.0] 2019-12-12
- FF-1258 - Updated to .net core SDK 3.1.100

## [1.4.0] 2019-09-15
### Added
- Added additional tests for analysers.
### Changed
- FF-950 - Updated to .net core 2.2.402

## [1.3.0] 2019-07-12
### Changed
- FF-864 - Update to VS2019 and .net core SDK 2.2.6 SDK 2.2.301

## [1.2.0] 2019-05-15
### Changed
- Updated to .net core 2.2

## [1.1.0] 2019-02-15
### Changed
- Updated to .net core 2.2

## [1.0.0]
### Changed
- Updated FxCop version to 2.6.2
- Updated .net core 2.1 to latest LTS version