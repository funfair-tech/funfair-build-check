# Changelog
All notable changes to this project will be documented in this file.

<!--
Please ADD ALL Changes to the UNRELEASED SECTION and not a specific release
-->

## [Unreleased]
### Added
- Using Microsoft.AspNetCore.Authentication.JwtBearer package should include System.IdentityModel.Tokens.Jwt
### Fixed
### Changed
- FF-1429 - Updated Meziantou.Analyzer to 1.0.681
- FF-1429 - Updated TeamCity.VSTest.TestAdapter to 1.0.31
- FF-1429 - Updated TeamCity.VSTest.TestAdapter to 1.0.32
- FF-1429 - Updated Meziantou.Analyzer to 1.0.682
- FF-1429 - Updated Meziantou.Analyzer to 1.0.683
- FF-1429 - Updated Meziantou.Analyzer to 1.0.684
- FF-1429 - Updated Meziantou.Analyzer to 1.0.685
### Removed
### Deployment Changes

<!--
Releases that have at least been deployed to staging, BUT NOT necessarily released to live.  Changes should be moved from [Unreleased] into here as they are merged into the appropriate release branch
-->
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