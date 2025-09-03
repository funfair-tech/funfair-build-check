using System;
using FunFair.BuildCheck.Interfaces;
using FunFair.BuildCheck.ProjectChecks.Helpers;
using Microsoft.Extensions.Logging;

namespace FunFair.BuildCheck.ProjectChecks.Settings;

internal static class PackPubHelper
{
    public static void Check(in ProjectContext project, bool isTestProject, string outputType, string property, Func<bool, bool, bool, string, bool> policy, ILogger logger)
    {
        bool isDotNetTool = project.IsDotNetTool();

        bool isExe = StringComparer.OrdinalIgnoreCase.Equals(x: outputType, project.GetOutputType());

        bool packable = policy(arg1: isDotNetTool, arg2: isExe, arg3: isTestProject, arg4: project.Name);

        ProjectValueHelpers.CheckValue(project: project, nodePresence: property, requiredValue: packable, logger: logger);
    }
}