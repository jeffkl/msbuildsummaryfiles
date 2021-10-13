using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Build.Framework;

namespace MSBuildLockFiles.Tasks
{
    internal static class ExtensionMethods
    {
        private static Lazy<bool> _isWindows = new Lazy<bool>(() => RuntimeInformation.IsOSPlatform(OSPlatform.Windows));

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
            string fullPath = taskItem.GetMetadata("FullPath");
            string relativePath = string.Empty;

            foreach (ITaskItem folderRoot in folderRoots)
            {
                string name = folderRoot.ItemSpec;
                // TODO: EnsureTrailingSlash for ToRelativePath() to work
                string path = folderRoot.GetMetadata("Path").EnsureTrailingSlash();

                bool allowRelative = string.Equals(bool.TrueString, folderRoot.GetMetadata("AllowRelative"), StringComparison.OrdinalIgnoreCase);

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

                    return fullPath.Replace(path, name.StartsWith("#") ? string.Empty : $"{{{name}}}").Replace(@"\", "/").Trim('/');
                }

                if (allowRelative)
                {
                    relativePath = fullPath.ToRelativePath(path).Replace(@"\", "/").Trim('/');
                }
            }

            return string.IsNullOrWhiteSpace(relativePath) ? fullPath : relativePath;
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
            else if (HasTrailingDirectorySeparator(path))
            {
                return path.Substring(0, path.Length - 1) + trailingCharacter;
            }

            return path + trailingCharacter;
        }

        private static bool HasTrailingDirectorySeparator(string path)
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
            if (_isWindows.Value)
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
