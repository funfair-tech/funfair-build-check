﻿using System;
using System.Xml;
using Microsoft.Extensions.Logging;

namespace BuildCheck.ProjectChecks
{
    public abstract class HasAppropriateNonAnalysisPackages : IProjectCheck
    {
        private const string PACKAGE_PRIVATE_ASSETS = @"All";

        private readonly string _detectPackageId;
        private readonly ILogger<NoPreReleaseNuGetPackages> _logger;
        private readonly string _mustIncludePackageId;

        protected HasAppropriateNonAnalysisPackages(string detectPackageId, string mustIncludePackageId, ILogger<NoPreReleaseNuGetPackages> logger)
        {
            this._detectPackageId = detectPackageId ?? throw new ArgumentNullException(nameof(detectPackageId));
            this._mustIncludePackageId = mustIncludePackageId ?? throw new ArgumentNullException(nameof(mustIncludePackageId));
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public void Check(string projectName, XmlDocument project)
        {
            XmlNodeList nodes = project.SelectNodes(xpath: "/Project/ItemGroup/PackageReference");

            bool foundSourcePackage = false;
            bool foundAnalyzerPackage = false;

            foreach (XmlElement? reference in nodes)
            {
                if (reference == null)
                {
                    continue;
                }

                string packageName = reference.GetAttribute(name: @"Include");

                if (string.IsNullOrWhiteSpace(packageName))
                {
                    this._logger.LogError($"{projectName}: Contains bad reference to packages.");

                    continue;
                }

                if (StringComparer.InvariantCultureIgnoreCase.Equals(this._detectPackageId, packageName))
                {
                    foundSourcePackage = true;
                }

                if (StringComparer.InvariantCultureIgnoreCase.Equals(this._mustIncludePackageId, packageName))
                {
                    foundAnalyzerPackage = true;
                }
            }

            if (foundSourcePackage && !foundAnalyzerPackage)
            {
                this._logger.LogError($"{projectName}: Found {this._detectPackageId} but did not find analyzer {this._mustIncludePackageId}.");
            }
        }
    }
}