using System;
using System.IO;

namespace MSBuildLockFiles.Tasks
{
    internal static class PathTool
    {
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
