using Microsoft.Build.Framework;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace MSBuildLockFiles.Tasks
{
    public class WriteBuildLockFile : MSBuildLockFileTaskBase
    {
        [Required]
        public string FilePath { get; set; }

        [Required]
        public ITaskItem[] LockFiles { get; set; }

        public override bool Execute()
        {
            if (Debug)
            {
                Debugger.Launch();
            }

            using StreamWriter writer = new StreamWriter(FilePath);

            foreach (string fullPath in LockFiles.Select(i => i.GetMetadata("FullPath")).OrderBy(i => i, StringComparer.OrdinalIgnoreCase))
            {
                using StreamReader reader = new StreamReader(fullPath);

                var buffer = new char[4 * 1024];
                int len;
                while ((len = reader.Read(buffer, 0, buffer.Length)) > 0)
                {
                    writer.Write(buffer, 0, len);
                }
            }

            return base.Execute();
        }
    }
}