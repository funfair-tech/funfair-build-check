# Changelog
All notable changes to this project will be documented in this file.

<!--
Please ADD ALL Changes to the UNRELASED SECTION and not a specific release
-->

## [Unreleased]
### Added
### Fixed
### Changed
- FF-1429 - Updated SonarAnalyzer.CSharp to 8.11.0.20529
- FF-1429 - Updated Microsoft.CodeAnalysis.FxCopAnalyzers to 3.3.0
- FF-1429 - Updated Microsoft.VisualStudio.Threading.Analyzers to 16.7.54
- FF-1429 - Updated SonarAnalyzer.CSharp to 8.10.0.19839
- FF-2759 - Updated to .net core 3.1.401

<!--
Releases that have at least been deployed to staging, BUT NOT necessarily released to live.  Changes should be moved from [Unreleased] into here as they are merged into the appropriate release branch
-->
## [1.12.0] 2020-07-20
## [Unreleased]
### Added
### Fixed
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















