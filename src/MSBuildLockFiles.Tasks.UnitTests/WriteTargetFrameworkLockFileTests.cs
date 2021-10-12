using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.Build.Utilities.ProjectCreation;
using Shouldly;
using Xunit;

namespace MSBuildLockFiles.Tasks.UnitTests
{
    public class WriteTargetFrameworkLockFileTests : TestBase
    {
        public string ProjectsRoot { get; } = IsWindows ? @"C:\projects" : @"/home/projects";

        public string PackageRoot { get; } = IsWindows ? @"C:\packages" : @"/home/packages";

        [Fact]
        public void SampleUnitTest()
        {
            FileInfo filePath = new FileInfo(GetTempFileName(".yml"));

            BuildEngine buildEngine = BuildEngine.Create();

            WriteTargetFrameworkLockFile task = new WriteTargetFrameworkLockFile
            {
                BuildEngine = buildEngine,
                FilePath = filePath.FullName,
                DefineConstants = new ITaskItem[]
                {
                    new TaskItem("TWO"),
                    new TaskItem("ONE"),
                },
                FileWrites = new ITaskItem[]
                {
                    new TaskItem(Path.Combine(ProjectsRoot, "ProjectA", "bin", "Debug", "net472", "ProjectA.dll")),
                    new TaskItem(Path.Combine(ProjectsRoot, "ProjectA", "bin", "Debug", "net472", "ProjectA.pdb")),
                },
                FolderRoots = new ITaskItem[]
                {
                    new TaskItem("#OutputPath", new Dictionary<string, string>
                    {
                        ["Path"] = Path.Combine(ProjectsRoot, "ProjectA", "bin", "Debug", "net472")
                    }),
                    new TaskItem("#MSBuildProjectDirectory", new Dictionary<string, string>
                    {
                        ["Path"] = Path.Combine(ProjectsRoot, "ProjectA")
                    }),
                    new TaskItem("NuGetPackageRoot", new Dictionary<string, string>
                    {
                        ["Path"] = PackageRoot
                    }),
                },
                References = new ITaskItem[]
                {
                    new TaskItem(Path.Combine(PackageRoot, "package.a", "1.0.0", "lib", "net472", "package.a.dll")),
                },
                Sources = new ITaskItem[]
                {
                    new TaskItem(Path.Combine(ProjectsRoot, "ProjectA", "Class1.cs")),
                },
                TargetFramework = "net472",
            };

            task.Execute().ShouldBeTrue(buildEngine.GetConsoleLog());

            filePath.Exists.ShouldBeTrue();

            File.ReadAllText(filePath.FullName).ShouldBe(
@"net472:
  constants:
  - ONE
  - TWO
  outputs:
  - ProjectA.dll
  - ProjectA.pdb
  references:
  - {NuGetPackageRoot}/package.a/1.0.0/lib/net472/package.a.dll
  sources:
  - Class1.cs
",
                StringCompareShould.IgnoreLineEndings);
        }
    }
}
