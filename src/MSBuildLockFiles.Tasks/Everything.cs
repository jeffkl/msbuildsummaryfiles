using Microsoft.Build.Framework;
using System.Collections.Generic;
using Task = Microsoft.Build.Utilities.Task;

namespace MSBuildLockFiles.Tasks
{
    public class Everything
    {
        private const string Key = "46CAC923-8671-4F2A-AFB7-AD2F5615E55A";

        private static readonly RegisteredTaskObjectLifetime RegisteredTaskObjectLifetime = RegisteredTaskObjectLifetime.Build;

        public ICollection<string> CompileTimeReferences { get; set; }

        public static Everything GetEverything(Task task)
        {
            Everything everything = task.BuildEngine4.GetRegisteredTaskObject(Key, RegisteredTaskObjectLifetime) as Everything;

            if (everything == null)
            {
                everything = new Everything();

                task.BuildEngine4.RegisterTaskObject(Key, everything, RegisteredTaskObjectLifetime.Build, false);
            }

            return everything;
        }
    }
}