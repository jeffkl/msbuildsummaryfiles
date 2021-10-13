using Microsoft.Build.Framework;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace MSBuildLockFiles.Tasks
{
    public class WriteTargetFrameworkLockFile : MSBuildLockFileTaskBase
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
            if (Debug)
            {
                Debugger.Launch();
            }

            FileInfo fileInfo = new FileInfo(FilePath);

            fileInfo.Directory.Create();

            using StreamWriter writer = new StreamWriter(fileInfo.FullName);

            writer.Write(TargetFramework);
            writer.WriteLine(":");

            writer.WriteLine("  constants:");
            foreach (ITaskItem item in DefineConstants.OrderBy(i => i.ItemSpec))
            {
                writer.Write("  - ");
                writer.WriteLine(item.ItemSpec);
            }

            writer.WriteLine("  outputs:");
            foreach (string fullPath in FileWrites.Select(NormalizePath).OrderBy(i => i))
            {
                writer.Write("  - ");
                writer.WriteLine(fullPath);
            }

            writer.WriteLine("  references:");
            foreach (string normalizedPath in References.Select(NormalizePath).OrderBy(i => i))
            {
                writer.Write("  - ");
                writer.WriteLine(normalizedPath);
            }

            writer.WriteLine("  sources:");
            foreach (string fullPath in Sources.Select(NormalizePath).OrderBy(i => i))
            {
                writer.Write("  - ");
                writer.WriteLine(fullPath);
            }

            return base.Execute();
        }

        private string NormalizePath(ITaskItem taskItem)
        {
            string fullPath = taskItem.GetMetadata("FullPath");
            string relativePath = string.Empty;

            foreach (ITaskItem folderRoot in FolderRoots)
            {
                string name = folderRoot.ItemSpec;
                string path = folderRoot.GetMetadata("Path");

                bool allowRelative = string.Equals(bool.TrueString, folderRoot.GetMetadata("AllowRelative"), StringComparison.OrdinalIgnoreCase);

                if (string.IsNullOrWhiteSpace(path))
                {
                    continue;
                }

                if (fullPath.StartsWith(path))
                {
                    return fullPath.Replace(path, name.StartsWith("#") ? string.Empty : $"{{{name}}}").Replace(@"\", "/").Trim('/');
                }

                if (allowRelative)
                {
                    relativePath = fullPath.ToRelativePath(path).Replace(@"\", "/").Trim('/');
                }
            }

            return string.IsNullOrWhiteSpace(relativePath) ? fullPath : relativePath;
        }
    }
}