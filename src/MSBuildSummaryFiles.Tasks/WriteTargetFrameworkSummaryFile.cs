// Copyright (c) Microsoft Corporation.
//
// Licensed under the MIT license.

using Microsoft.Build.Framework;
using System.IO;
using System.Linq;

namespace MSBuildSummaryFiles.Tasks
{
    /// <summary>
    /// Represents a task that writes a build summary file for a target framework.
    /// </summary>
    public class WriteTargetFrameworkSummaryFile : MSBuildSummaryFileTaskBase
    {
        /// <summary>
        /// Gets or sets an array of <see cref="ITaskItem" /> items representing compiler constants.
        /// </summary>
        [Required]
        public ITaskItem[] DefineConstants { get; set; }

        /// <summary>
        /// Gets or sets the path to the file to write.
        /// </summary>
        [Required]
        public string FilePath { get; set; }

        /// <summary>
        /// Gets or sets an array of <see cref="ITaskItem" /> items representing file writes.
        /// </summary>
        [Required]
        public ITaskItem[] FileWrites { get; set; }

        /// <summary>
        /// Gets or sets an array of <see cref="ITaskItem" /> items representing folder roots.
        /// </summary>
        [Required]
        public ITaskItem[] FolderRoots { get; set; }

        /// <summary>
        /// Gets or sets an array of <see cref="ITaskItem" /> items representing assembly references.
        /// </summary>
        [Required]
        public ITaskItem[] References { get; set; }

        /// <summary>
        /// Gets or sets an array of <see cref="ITaskItem" /> items representing source files.
        /// </summary>
        [Required]
        public ITaskItem[] Sources { get; set; }

        /// <summary>
        /// Gets or sets the target framework.
        /// </summary>
        [Required]
        public string TargetFramework { get; set; }

        /// <inheritdoc />
        public override bool Execute()
        {
            FileInfo fileInfo = new FileInfo(FilePath);

            fileInfo.Directory!.Create();

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