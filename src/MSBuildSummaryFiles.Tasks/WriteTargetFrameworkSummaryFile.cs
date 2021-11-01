// Copyright (c) Microsoft Corporation.
//
// Licensed under the MIT license.

using Microsoft.Build.Framework;
using System.Collections.Generic;
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
        /// Gets or sets an array of <see cref="ITaskItem" /> items representing folder roots for output files.
        /// </summary>
        [Required]
        public ITaskItem[] OutputFolderRoots { get; set; }

        /// <summary>
        /// Gets or sets an array of <see cref="ITaskItem" /> items representing output files.
        /// </summary>
        [Required]
        public ITaskItem[] Outputs { get; set; }

        /// <summary>
        /// Gets or sets an array of <see cref="ITaskItem" /> items representing folder roots for assembly reference files.
        /// </summary>
        [Required]
        public ITaskItem[] ReferenceFolderRoots { get; set; }

        /// <summary>
        /// Gets or sets an array of <see cref="ITaskItem" /> items representing assembly references.
        /// </summary>
        [Required]
        public ITaskItem[] References { get; set; }

        /// <summary>
        /// Gets or sets an array of <see cref="ITaskItem" /> items representing folder roots for source files.
        /// </summary>
        [Required]
        public ITaskItem[] SourceFolderRoots { get; set; }

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
            if (CountItemsWithAllowRelative(SourceFolderRoots) > 1 || CountItemsWithAllowRelative(OutputFolderRoots) > 1 || CountItemsWithAllowRelative(ReferenceFolderRoots) > 1)
            {
                Log.LogErrorFromResources(nameof(Strings.DuplicateAllowRelative));

                return false;
            }

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
            if (Outputs?.Length > 0)
            {
                foreach (string format in Outputs.GetNormalizedPaths(OutputFolderRoots).OrderBy(i => i))
                {
                    writer.Write("  - ");
                    writer.WriteLine(format);
                }
            }

            writer.WriteLine("  references:");
            if (References?.Length > 0)
            {
                foreach (string normalizedPath in References.GetNormalizedPaths(ReferenceFolderRoots).OrderBy(i => i))
                {
                    writer.Write("  - ");
                    writer.WriteLine(normalizedPath);
                }
            }

            writer.WriteLine("  sources:");
            if (Sources?.Length > 0)
            {
                foreach (string normalizedPath in Sources.GetNormalizedPaths(SourceFolderRoots).OrderBy(i => i))
                {
                    writer.Write("  - ");
                    writer.WriteLine(normalizedPath);
                }
            }

            return base.Execute();
        }

        private int CountItemsWithAllowRelative(IEnumerable<ITaskItem> items)
        {
            if (items == null)
            {
                return 0;
            }

            return items.Count(i => string.Equals(i.GetMetadata("AllowRelative"), bool.TrueString));
        }
    }
}