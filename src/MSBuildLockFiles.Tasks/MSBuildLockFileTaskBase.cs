using Microsoft.Build.Utilities;

namespace MSBuildLockFiles.Tasks
{
    public abstract class MSBuildLockFileTaskBase : Task
    {
        public bool Debug { get; set; }

        public override bool Execute()
        {
            return !Log.HasLoggedErrors;
        }
    }
}