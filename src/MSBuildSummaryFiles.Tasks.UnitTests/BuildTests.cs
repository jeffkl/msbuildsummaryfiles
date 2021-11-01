// Copyright (c) Microsoft Corporation.
//
// Licensed under the MIT license.

using Microsoft.Build.Utilities.ProjectCreation;
using Shouldly;
using System;
using System.IO;
using Xunit;

namespace MSBuildSummaryFiles.Tasks.UnitTests
{
    public class BuildTests : TestBase
    {
        [Fact]
        public void MultiTargetingBuild()
        {
#if NETFRAMEWORK
            string[] targetFrameworks = { "net46", "net472" };
#else
            string[] targetFrameworks = { "netstandard2.0", "netstandard2.1" };
#endif
            CreateSdkStyleProject(targetFrameworks)
                .Save(GetTempProjectFile("ProjectA", "AAA.cs", "BBB.cs", "strings.resx"))
                .TryBuild(restore: true, out bool result, out BuildOutput buildOutput)
                .TryGetPropertyValue("BuildSummaryFilePath", out string buildSummaryFilePath);

            result.ShouldBeTrue(buildOutput.GetConsoleLog());

            CompareSummaryFiles(nameof(MultiTargetingBuild), buildSummaryFilePath);
        }

        [Fact]
        public void SingleTargetingBuild()
        {
            CreateSdkStyleProject("netstandard2.0")
                .Save(GetTempProjectFile("ProjectA", "Strings.resx"))
                .TryBuild(restore: true, out bool result, out BuildOutput buildOutput)
                .TryGetPropertyValue("BuildSummaryFilePath", out string buildSummaryFilePath);

            result.ShouldBeTrue(buildOutput.GetConsoleLog());

            buildSummaryFilePath.ShouldNotBeNullOrEmpty();

            CompareSummaryFiles(nameof(SingleTargetingBuild), buildSummaryFilePath);
        }

        [Fact]
        public void NuGetPackageInAllOutputs()
        {
            CreateSdkStyleProject("netstandard1.0")
                .Property("GenerateTargetPlatformDefineConstants", bool.FalseString)
                .Property("GeneratePackageOnBuild", bool.TrueString)
                .Save(GetTempProjectFile("ProjectA"))
                .TryBuild(restore: true, out bool result, out BuildOutput buildOutput)
                .TryGetPropertyValue("BuildSummaryFilePath", out string buildSummaryFilePath);

            result.ShouldBeTrue(buildOutput.GetConsoleLog());

            buildSummaryFilePath.ShouldNotBeNullOrEmpty();

            CompareSummaryFiles(nameof(NuGetPackageInAllOutputs), buildSummaryFilePath);
        }

#if NETFRAMEWORK

        [Fact]
        public void LegacyProject()
        {
            CreateLegacyProject("net472", "v4.7.2")
                .ItemCompile("Class1.cs")
                .Save(GetTempProjectFile("ProjectA", "Class1.cs"))
                .TryBuild(restore: true, out bool result, out BuildOutput buildOutput)
                .TryGetPropertyValue("BuildSummaryFilePath", out string buildSummaryFilePath);

            result.ShouldBeTrue(buildOutput.GetConsoleLog());

            buildSummaryFilePath.ShouldNotBeNullOrEmpty();

            CompareSummaryFiles(nameof(LegacyProject), buildSummaryFilePath);
        }

#endif
        private void CompareSummaryFiles(string testName, string actualFilePath)
        {
            FileInfo fileInfo = new FileInfo(Path.Combine("Expected", $"{testName}.{CurrentTargetFramework}.yml"));

            fileInfo.Exists.ShouldBeTrue($"{fileInfo.FullName} does not exist but should.");

            using StreamReader expectedReader = File.OpenText(fileInfo.FullName);

            using StreamReader actualReader = File.OpenText(actualFilePath);

            int line = 1;
            while (!expectedReader.EndOfStream && !actualReader.EndOfStream)
            {
                actualReader.ReadLine().ShouldBe(
                    expectedReader.ReadLine(),
                    $"Line {line} is not the same",
                    StringCompareShould.IgnoreLineEndings);
                line++;
            }
        }

        private ProjectCreator CreateSdkStyleProject(params string[] targetFrameworks)
        {
            CreateDirectoryBuildPropsAndTargets(targetFrameworks);

            return ProjectCreator.Templates.SdkCsproj(
                targetFrameworks: targetFrameworks);
        }

        private ProjectCreator CreateLegacyProject(string targetFramework, string targetFrameworkVersion)
        {
            CreateDirectoryBuildPropsAndTargets(targetFramework);

            return ProjectCreator.Templates.LegacyCsproj(
                targetFrameworkVersion: targetFrameworkVersion);
        }

        private void CreateDirectoryBuildPropsAndTargets(params string[] targetFrameworks)
        {
            ProjectCreator
                .Create(Path.Combine(TestRootPath, "Directory.Build.props"))
                .Property("MSBuildSummaryFilesTaskAssemblyPath", TaskAssemblyFullPath)
                .Import(Path.Combine(Environment.CurrentDirectory, "build", "MSBuildSummaryFiles.props"), condition: targetFrameworks.Length > 1 ? "'$(TargetFramework)' != ''" : null)
                .Import(Path.Combine(Environment.CurrentDirectory, "buildMultiTargeting", "MSBuildSummaryFiles.props"), condition: targetFrameworks.Length > 1 ? "'$(TargetFramework)' == ''" : bool.FalseString)
                .Save();

            ProjectCreator
                .Create(Path.Combine(TestRootPath, "Directory.Build.targets"))
                .Import(Path.Combine(Environment.CurrentDirectory, "build", "MSBuildSummaryFiles.targets"), condition: targetFrameworks.Length > 1 ? "'$(TargetFramework)' != ''" : null)
                .Import(Path.Combine(Environment.CurrentDirectory, "buildMultiTargeting", "MSBuildSummaryFiles.targets"), condition: targetFrameworks.Length > 1 ? "'$(TargetFramework)' == ''" : bool.FalseString)
                .Save();
        }
    }
}