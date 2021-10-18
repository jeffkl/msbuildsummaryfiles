using Microsoft.Build.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MSBuildSummaryFiles.Tasks
{
    public class WriteTargetFrameworkSummaryFile : MSBuildSummaryFileTaskBase
    {
        [Required]
        public ITaskItem[] DefineConstants { get; set; }

        [Required]
        public string FilePath { get; set; }

        [Required]
        public ITaskItem[] FileWrites { get; set; }

        [Required]
        public ITaskItem[] FolderRoots { get; set; }

        [Required]
        public ITaskItem[] References { get; set; }

        [Required]
        public ITaskItem[] Sources { get; set; }

        [Required]
        public string TargetFramework { get; set; }

        public override bool Execute()
        {
            FileInfo fileInfo = new FileInfo(FilePath);

            fileInfo.Directory.Create();

            using StreamWriter writer = new StreamWriter(fileInfo.FullName);

            writer.Write(TargetFramework);
            writer.WriteLine(":");

            writer.WriteLine("  constants:");
            if (DefineConstants?.Length > 0)
            {
                foreach (string itemSpec in DefineConstants.Select(i => i.ItemSpec).Distinct().OrderBy(i => i))
                {
                    writer.Write("  - ");
                    writer.WriteLine(itemSpec);
                }
            }

            writer.WriteLine("  outputs:");
            if (FileWrites?.Length > 0)
            {
                foreach (string format in FileWrites.GetNormalizedPaths(FolderRoots).OrderBy(i => i))
                {
                    writer.Write("  - ");
                    writer.WriteLine(format);
                }
            }

            writer.WriteLine("  references:");
            if (References?.Length > 0)
            {
                foreach (string normalizedPath in References.GetNormalizedPaths(FolderRoots).OrderBy(i => i))
                {
                    writer.Write("  - ");
                    writer.WriteLine(normalizedPath);
                }
            }

            writer.WriteLine("  sources:");
            if (Sources?.Length > 0)
            {
                foreach (string normalizedPath in Sources.GetNormalizedPaths(FolderRoots).OrderBy(i => i))
                {
                    writer.Write("  - ");
                    writer.WriteLine(normalizedPath);
                }
            }

            return base.Execute();
        }
    }
}