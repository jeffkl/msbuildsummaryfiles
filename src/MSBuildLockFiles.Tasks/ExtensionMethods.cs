using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Build.Framework;

namespace MSBuildLockFiles.Tasks
{
    internal static class ExtensionMethods
    {
        public static IEnumerable<string> GetNormalizedPaths(this IEnumerable<ITaskItem> items, ITaskItem[] folderRoots)
        {
            foreach (ITaskItem item in items)
            {
                string normalizedPath = item.NormalizePath(folderRoots);

                if(normalizedPath != null)
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
                string path = folderRoot.GetMetadata("Path");

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
    }
}
