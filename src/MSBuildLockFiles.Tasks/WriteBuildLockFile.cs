using Microsoft.Build.Framework;
using System;
using System.IO;
using System.Linq;

namespace MSBuildLockFiles.Tasks
{
    public class WriteBuildLockFile : MSBuildLockFileTaskBase
    {
        [Required]
        public ITaskItem[] Outputs { get; set; }

        [Required]
        public ITaskItem[] FolderRoots { get; set; }

        [Required]
        public string FilePath { get; set; }

        [Required]
        public ITaskItem[] LockFiles { get; set; }

        public override bool Execute()
        {
            using StreamWriter writer = new StreamWriter(FilePath);

            if (Outputs.Any())
            {
                writer.WriteLine("all:");
                writer.WriteLine("  outputs:");
                foreach (var item in Outputs.GetNormalizedPaths(FolderRoots).OrderBy(i => i))
                {
                    writer.Write("  - ");
                    writer.WriteLine(item);
                }
            }

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