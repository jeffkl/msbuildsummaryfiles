using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace MSBuildLockFiles.Tasks
{
    public class GatherCompileTimeDependencies : Task
    {
        [Required]
        public ITaskItem[] CscCommandLineArgs { get; set; }

        [Required]
        public ITaskItem[] FolderRoots { get; set; }

        public override bool Execute()
        {
            SortedSet<(string Name, string Path)> folderRoots = new SortedSet<(string Name, string Path)>(Thing.Instance);

            foreach (ITaskItem item in FolderRoots)
            {
                string name = item.ItemSpec;
                string path = item.GetMetadata("Path");

                if (string.IsNullOrWhiteSpace(path))
                {
                    continue;
                }

                folderRoots.Add((name, path));
            }

            Debugger.Launch();

            Log.LogMessageFromText("Hello", MessageImportance.High);

            CSharpCommandLineParser parser = CSharpCommandLineParser.Default;

            CSharpCommandLineArguments arguments = parser.Parse(CscCommandLineArgs.Select(i => i.ItemSpec), Environment.CurrentDirectory, null);


            Everything everything = Everything.GetEverything(this);

            everything.CompileTimeReferences = new List<string>(arguments.MetadataReferences.Length);

            foreach (CommandLineReference metadataReference in arguments.MetadataReferences)
            {
                foreach ((string Name, string Path) folderRoot in folderRoots)
                {
                    if (metadataReference.Reference.StartsWith(folderRoot.Path))
                    {
                        everything.CompileTimeReferences.Add(metadataReference.Reference.Replace(folderRoot.Path, $"$({folderRoot.Name})"));
                    }
                }
            }

            return !Log.HasLoggedErrors;
        }
    }

    public class Thing : IComparer<(string Name, string Path)>
    {
        public static readonly Thing Instance = new Thing();

        private Thing()
        {
        }

        public int Compare((string Name, string Path) x, (string Name, string Path) y) => y.Path.Length.CompareTo(x.Path.Length);
    }
}