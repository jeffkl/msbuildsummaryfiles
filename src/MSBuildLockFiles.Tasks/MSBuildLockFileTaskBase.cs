using System;
using Microsoft.Build.Utilities;

namespace MSBuildLockFiles.Tasks
{
    public abstract class MSBuildLockFileTaskBase : Task
    {
        public bool Debug { get; set; } = string.Equals(Environment.GetEnvironmentVariable("DEBUG_LOCKFILE_TASK"), bool.TrueString, StringComparison.OrdinalIgnoreCase);

        public override bool Execute()
        {
            return !Log.HasLoggedErrors;
        }
    }
}