using System;
using System.Diagnostics;
using Microsoft.Build.Utilities;

namespace MSBuildSummaryFiles.Tasks
{
    public abstract class MSBuildSummaryFileTaskBase : Task
    {
        static MSBuildSummaryFileTaskBase()
        {
            if (string.Equals(Environment.GetEnvironmentVariable("DEBUG_SUMMARYFILE_TASK"), bool.TrueString, StringComparison.OrdinalIgnoreCase))
            {
                Debugger.Launch();
            }
        }

        public override bool Execute()
        {
            return !Log.HasLoggedErrors;
        }
    }
}