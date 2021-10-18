using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Build.Framework;

namespace MSBuildSummaryFiles.Tasks
{
    internal static class ExtensionMethods
    {
        private static readonly bool IsWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

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

        public static string NormalizePath(this ITaskItem taskItem, ITaskItem[] folderRoots)
        {
            string fullPath = taskItem.GetMetadata("FullPath").FixPath();

            // Need to exhaust absolute path options before try relative path.
            foreach (ITaskItem folderRoot in folderRoots.AbsolutePathRoots())
            {
                string name = folderRoot.ItemSpec;
                string path = folderRoot.GetMetadata("Path").FixPath();

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
                        : path.HasTrailingDirectorySeparator() ? $"{{{name}}}{Path.DirectorySeparatorChar}" : $"{{{name}}}";

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

        public static string FixPath(this string path)
        {
            if (path.StartsWith("/private/var"))
            {
                return path.Substring(8);
            }

            return path;
        }

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

        public static string EnsureTrailingSlash(this string path)
        {
            return EnsureTrailingCharacter(path, Path.DirectorySeparatorChar);
        }

        private static string EnsureTrailingCharacter(string path, char trailingCharacter)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            // if the path is empty, we want to return the original string instead of a single trailing character.
            if (path.Length == 0
                || path[path.Length - 1] == trailingCharacter)
            {
                return path;
            }
            // This condition checks if there is a different valid path separator than the one requested for.
            // In that case we replace this path separator.
            else if (path.HasTrailingDirectorySeparator())
            {
                return path.Substring(0, path.Length - 1) + trailingCharacter;
            }

            return path + trailingCharacter;
        }

        private static bool HasTrailingDirectorySeparator(this string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return false;
            }
            else
            {
                return IsDirectorySeparatorChar(path[path.Length - 1]);
            }
        }

        private static bool IsDirectorySeparatorChar(char ch)
        {
            if (IsWindows)
            {
                // Windows has both '/' and '\' as valid directory separators.
                return (ch == Path.DirectorySeparatorChar ||
                        ch == Path.AltDirectorySeparatorChar);
            }
            else
            {
                return ch == Path.DirectorySeparatorChar;
            }
        }
    }
}
