// Copyright (c) Microsoft Corporation.
//
// Licensed under the MIT license.

using Microsoft.Build.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MSBuildSummaryFiles.Tasks
{
    /// <summary>
    /// Represents extension methods for this library.
    /// </summary>
    internal static class ExtensionMethods
    {
        /// <summary>
        /// Returns the paths in the specified folder roots which are treated as absolute.
        /// </summary>
        /// <param name="folderRoots">A array of <see cref="ITaskItem" /> items containing folder roots.</param>
        /// <returns>An <see cref="IEnumerable{T}" /> of folders which are treated as absolute.</returns>
        public static IEnumerable<ITaskItem> AbsolutePathRoots(this ITaskItem[] folderRoots)
        {
            foreach (ITaskItem folderRoot in folderRoots)
            {
                if (!string.Equals(bool.TrueString, folderRoot.GetMetadata("AllowRelative"), StringComparison.OrdinalIgnoreCase))
                {
                    yield return folderRoot;
                }
            }
        }

        /// <summary>
        /// Ensures the specified path has a trailing slash.
        /// </summary>
        /// <param name="path">The path to analyze.</param>
        /// <returns>The specified path with a trailing slash if one was not found, otherwise the original path.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="path" /> is <c>null</c>.</exception>
        public static string EnsureTrailingSlash(this string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (string.IsNullOrWhiteSpace(path) || !path.Any() || path.Last() == Path.DirectorySeparatorChar)
            {
                return path;
            }

            return path + Path.DirectorySeparatorChar;
        }

        /// <summary>
        /// Fixes a path by replacing known symlink paths like &quot;/private/var&quot; with real paths.
        /// </summary>
        /// <param name="path">The path to fix.</param>
        /// <returns>The fixed copy of the specified path. </returns>
        public static string FixPath(this string path)
        {
            if (path.StartsWith("/private/var"))
            {
                return path.Substring(8);
            }

            return path;
        }

        /// <summary>
        /// Normalizes the specified folders based on the folder roots.
        /// </summary>
        /// <param name="items">An <see cref="IEnumerable{T}" /> containing the items to normalize.</param>
        /// <param name="folderRoots">A array of <see cref="ITaskItem" /> items containing folder roots.</param>
        /// <returns>The normalized version of the specified paths.</returns>
        public static IEnumerable<string> GetNormalizedPaths(this IEnumerable<ITaskItem> items, ITaskItem[] folderRoots)
        {
            foreach (ITaskItem item in items)
            {
                string normalizedPath = item.NormalizePath(folderRoots);

                if (normalizedPath != null)
                {
                    yield return normalizedPath;
                }
            }
        }

        /// <summary>
        /// Normalizes the specified folder based on the folder roots.
        /// </summary>
        /// <param name="taskItem">A <see cref="ITaskItem" /> containing the path to normalize.</param>
        /// <param name="folderRoots">A array of <see cref="ITaskItem" /> items containing folder roots.</param>
        /// <returns>A normalized version of the specified path.</returns>
        /// <exception cref="InvalidOperationException">More than one relative path was specified with <paramref name="folderRoots" />.</exception>
        public static string NormalizePath(this ITaskItem taskItem, ITaskItem[] folderRoots)
        {
            string fullPath = taskItem.GetMetadata("FullPath").FixPath();

            // Need to exhaust absolute path options before try relative path.
            foreach (ITaskItem folderRoot in folderRoots.AbsolutePathRoots())
            {
                string name = folderRoot.ItemSpec;
                string path = folderRoot.GetMetadata("Path").FixPath().EnsureTrailingSlash();

                if (string.IsNullOrWhiteSpace(path))
                {
                    continue;
                }

                if (fullPath.StartsWith(path))
                {
                    if (name.StartsWith("!"))
                    {
                        return null;
                    }

                    string placeHolder = name.StartsWith("#")
                        ? string.Empty
                        : $"({name})/";

                    return fullPath.Replace(path, placeHolder).Replace(@"\", "/").Trim('/');
                }
            }

            List<ITaskItem> relativePathRoots = folderRoots.RelativePathRoots().ToList();

            if (relativePathRoots.Count > 1)
            {
                throw new InvalidOperationException("No more than 1 AllowRelative tag is permitted for roots.");
            }

            // Only one AllowRelative tag is considered.
            foreach (ITaskItem folderRoot in relativePathRoots)
            {
                string path = folderRoot.GetMetadata("Path").EnsureTrailingSlash().FixPath();
                string relativePath = fullPath.ToRelativePath(path).Replace(@"\", "/").Trim('/');
                return relativePath;
            }

            return fullPath;
        }

        /// <summary>
        /// Returns the paths in the specified folder roots which are treated as relative.
        /// </summary>
        /// <param name="folderRoots">A array of <see cref="ITaskItem" /> items containing folder roots.</param>
        /// <returns>An <see cref="IEnumerable{T}" /> of folders which are treated as relative.</returns>
        public static IEnumerable<ITaskItem> RelativePathRoots(this ITaskItem[] folderRoots)
        {
            foreach (ITaskItem folderRoot in folderRoots)
            {
                if (string.Equals(bool.TrueString, folderRoot.GetMetadata("AllowRelative"), StringComparison.OrdinalIgnoreCase))
                {
                    yield return folderRoot;
                }
            }
        }

        /// <summary>
        /// Gets the specified path as relative to another.
        /// </summary>
        /// <param name="path">The path to make relative.</param>
        /// <param name="relativeTo">The path to make the specified path relative to.</param>
        /// <returns>The specified path relative to another.</returns>
        public static string ToRelativePath(this string path, string relativeTo)
        {
            FileInfo fullPath = new FileInfo(Path.GetFullPath(path));
            FileInfo relativeFullPath = new FileInfo(Path.GetFullPath(relativeTo));

            if (fullPath.Directory == null || relativeFullPath.Directory == null || !string.Equals(fullPath.Directory.Root.FullName, relativeFullPath.Directory.Root.FullName))
            {
                return fullPath.FullName;
            }

            Uri fullPathUri = new Uri(fullPath.FullName, UriKind.Absolute);
            Uri relativePathUri = new Uri(relativeFullPath.FullName, UriKind.Absolute);

            return Uri.UnescapeDataString(relativePathUri.MakeRelativeUri(fullPathUri).ToString()).Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
        }
    }
}