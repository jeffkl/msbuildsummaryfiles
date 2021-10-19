// Copyright (c) Microsoft Corporation.
//
// Licensed under the MIT license.

using Microsoft.Build.Framework;
using System;
using System.IO;
using System.Linq;

namespace MSBuildSummaryFiles.Tasks
{
    /// <summary>
    /// Represents a task that writes the final build summary file.
    /// </summary>
    public class WriteBuildSummaryFile : MSBuildSummaryFileTaskBase
    {
        /// <summary>
        /// Gets or sets the path of the file to write.
        /// </summary>
        [Required]
        public string FilePath { get; set; }

        /// <summary>
        /// Gets or sets an array of <see cref="ITaskItem" /> items representing folder roots.
        /// </summary>
        [Required]
        public ITaskItem[] FolderRoots { get; set; }

        /// <summary>
        /// Gets or sets an array of <see cref="ITaskItem" /> items representing outputs.
        /// </summary>
        [Required]
        public ITaskItem[] Outputs { get; set; }

        /// <summary>
        /// Gets or sets an array of <see cref="ITaskItem" /> items representing summary files for each target framework.
        /// </summary>
        [Required]
        public ITaskItem[] SummaryFiles { get; set; }

        /// <inheritdoc />
        public override bool Execute()
        {
            using StreamWriter writer = new StreamWriter(FilePath);

            if (Outputs.Any())
            {
                writer.WriteLine("all:");
                writer.WriteLine("  outputs:");
                foreach (string item in Outputs.GetNormalizedPaths(FolderRoots).OrderBy(i => i))
                {
                    writer.Write("  - ");
                    writer.WriteLine(item);
                }
            }

            foreach (string fullPath in SummaryFiles.Select(i => i.GetMetadata("FullPath")).OrderBy(i => i, StringComparer.OrdinalIgnoreCase))
            {
                using StreamReader reader = new StreamReader(fullPath);

                char[] buffer = new char[4 * 1024];
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