using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.Build.Utilities.ProjectCreation;
using Shouldly;
using Xunit;

namespace MSBuildSummaryFiles.Tasks.UnitTests
{
    public class WriteTargetFrameworkSummaryFileTests : TestBase
    {
        public string ProjectsRoot { get; } = IsWindows ? @"C:\projects" : @"/home/projects";

        public string PackageRoot { get; } = IsWindows ? @"C:\packages" : @"/home/packages";

        public string FrameworkAssembliesRoot { get; } = IsWindows ? @"C:\programfiles\frameworkAssembliesRoot\" : @"/home/programfiles/frameworkAssembliesRoot/";

        public string NetCoreTargetingPackRoot { get; } = IsWindows ? @"C:\dotnet\packs\" : @"/home/dotnet/packs/";

        [Fact]
        public void SampleUnitTest()
        {
            FileInfo filePath = new FileInfo(GetTempFileName(".yml"));

            BuildEngine buildEngine = BuildEngine.Create();

            var IntermediateOutputPath = Path.Combine(ProjectsRoot, "ProjectA", "obj", "Debug", "net472");

            WriteTargetFrameworkSummaryFile task = new WriteTargetFrameworkSummaryFile
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
                    new TaskItem(Path.Combine(ProjectsRoot, "ProjectA", "obj", "Debug", "net472", "ProjectA.dll")),
                    new TaskItem(Path.Combine(ProjectsRoot, "ProjectA", "obj", "Debug", "net472", "ProjectA.pdb")),
                },
                FolderRoots = new ITaskItem[]
                {
                    new TaskItem("#OutputPath", new Dictionary<string, string>
                    {
                        ["Path"] = Path.Combine(ProjectsRoot, "ProjectA", "bin", "Debug", "net472")
                    }),
                    new TaskItem("!IntermediateOutputPath", new Dictionary<string, string>
                    {
                        ["Path"] = IntermediateOutputPath
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
                    new TaskItem(Path.Combine(IntermediateOutputPath, "ProjectA.Version.cs")),
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

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void AllowRelative(bool withDirectorySeparatorChar)
        {
            FileInfo filePath = new FileInfo(GetTempFileName(".yml"));

            BuildEngine buildEngine = BuildEngine.Create();

            WriteTargetFrameworkSummaryFile task = new WriteTargetFrameworkSummaryFile
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
                        ["Path"] = Path.Combine(ProjectsRoot, "ProjectA") + (withDirectorySeparatorChar ? Path.DirectorySeparatorChar : string.Empty),
                        ["AllowRelative"] = bool.TrueString
                    }),
                    new TaskItem("NuGetPackageRoot", new Dictionary<string, string>
                    {
                        ["Path"] = PackageRoot + (withDirectorySeparatorChar ? Path.DirectorySeparatorChar : string.Empty),
                    }),
                    new TaskItem("FrameworkAssembliesRoot", new Dictionary<string, string>
                    {
                        ["Path"] = FrameworkAssembliesRoot,
                    }),
                    new TaskItem("NetCoreTargetingPackRoot", new Dictionary<string, string>
                    {
                        ["Path"] = NetCoreTargetingPackRoot
                    }),
                },
                References = new ITaskItem[]
                {
                    new TaskItem(Path.Combine(FrameworkAssembliesRoot, "Microsoft.CSharp.dll")),
                    new TaskItem(Path.Combine(NetCoreTargetingPackRoot, "Microsoft.NETCore.App.Ref", "net472", "Microsoft.CSharp.dll")),
                },
                Sources = new ITaskItem[]
                {
                    new TaskItem(Path.Combine(ProjectsRoot, "ProjectA", "Class1.cs")),
                    new TaskItem(Path.GetFullPath(Path.Combine(ProjectsRoot, "ProjectA", "..", "Shared", "SharedClass.cs"))),
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
  - {FrameworkAssembliesRoot}/Microsoft.CSharp.dll
  - {NetCoreTargetingPackRoot}/Microsoft.NETCore.App.Ref/net472/Microsoft.CSharp.dll
  sources:
  - ../Shared/SharedClass.cs
  - Class1.cs
",
                StringCompareShould.IgnoreLineEndings);
        }

        [Fact]
        public void MoreThanOneAllowRelativeTag_Throws()
        {
            FileInfo filePath = new FileInfo(GetTempFileName(".yml"));

            BuildEngine buildEngine = BuildEngine.Create();

            WriteTargetFrameworkSummaryFile task = new WriteTargetFrameworkSummaryFile
            {
                BuildEngine = buildEngine,
                FilePath = filePath.FullName,

                FolderRoots = new ITaskItem[]
                {

                    new TaskItem("#MSBuildProjectDirectory", new Dictionary<string, string>
                    {
                        ["Path"] = Path.Combine(ProjectsRoot, "ProjectA"),
                        ["AllowRelative"] = bool.TrueString
                    }),
                    new TaskItem("NuGetPackageRoot", new Dictionary<string, string>
                    {
                        ["Path"] = PackageRoot,
                        ["AllowRelative"] = bool.TrueString
                    })
                },
                Sources = new ITaskItem[]
                {
                    new TaskItem(Path.Combine(ProjectsRoot, "ProjectA", "Class1.cs")),
                    new TaskItem(Path.GetFullPath(Path.Combine(ProjectsRoot, "ProjectA", "..", "Shared", "SharedClass.cs"))),
                },
                TargetFramework = "net472",
            };

            var exception = Assert.Throws<InvalidOperationException>(() => task.Execute().ShouldBeTrue(buildEngine.GetConsoleLog()));
            Assert.Equal("No more than 1 AllowRelative tag is permitted for roots.", exception.Message);
        }

        [Fact]
        public void Deduplicate_Compiler_Constants()
        {
            FileInfo filePath = new FileInfo(GetTempFileName(".yml"));

            BuildEngine buildEngine = BuildEngine.Create();

            WriteTargetFrameworkSummaryFile task = new WriteTargetFrameworkSummaryFile
            {
                BuildEngine = buildEngine,
                FilePath = filePath.FullName,
                DefineConstants = new ITaskItem[]
                {
                    new TaskItem("ONE"),
                    new TaskItem("ONE"),
                    new TaskItem("TWO"),
                },
                FolderRoots = new ITaskItem[]
                {
                    new TaskItem("#MSBuildProjectDirectory", new Dictionary<string, string>
                    {
                        ["Path"] = Path.Combine(ProjectsRoot, "ProjectA"),
                        ["AllowRelative"] = bool.TrueString
                    }),
                    new TaskItem("NuGetPackageRoot", new Dictionary<string, string>
                    {
                        ["Path"] = PackageRoot,
                    })
                },
                Sources = new ITaskItem[]
                {
                    new TaskItem(Path.Combine(ProjectsRoot, "ProjectA", "Class1.cs")),
                    new TaskItem(Path.GetFullPath(Path.Combine(ProjectsRoot, "ProjectA", "..", "Shared", "SharedClass.cs"))),
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
  references:
  sources:
  - ../Shared/SharedClass.cs
  - Class1.cs
",
                StringCompareShould.IgnoreLineEndings);
        }
    }
}
