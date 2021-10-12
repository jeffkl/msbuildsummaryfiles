using System;
using Microsoft.Build.Utilities.ProjectCreation;
using Shouldly;
using Xunit;

namespace MSBuildLockFiles.Tasks.UnitTests
{
    public class UnitTest1 : TestBase
    {
        [Fact]
        public void SampleUnitTest()
        {
            BuildEngine buildEngine = BuildEngine.Create();

            WriteBuildLockFile task = new WriteBuildLockFile
            {
                BuildEngine = buildEngine,
                FilePath = null,
            };

            Should.Throw<ArgumentNullException>(() => task.Execute());
        }
    }
}
