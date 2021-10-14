using System;
using System.Diagnostics;
using Microsoft.Build.Utilities;

namespace MSBuildLockFiles.Tasks
{
    public abstract class MSBuildLockFileTaskBase : Task
    {
        static MSBuildLockFileTaskBase()
        {
            if (string.Equals(Environment.GetEnvironmentVariable("DEBUG_LOCKFILE_TASK"), bool.TrueString, StringComparison.OrdinalIgnoreCase))
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