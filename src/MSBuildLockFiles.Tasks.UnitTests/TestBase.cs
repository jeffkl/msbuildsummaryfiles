using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Build.Utilities.ProjectCreation;

namespace MSBuildLockFiles.Tasks.UnitTests
{
    public abstract class TestBase : MSBuildTestBase
    {
        private readonly string _testRootPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

        private static readonly Lazy<string> TaskAssemblyFullPathLazy = new Lazy<string>(() => typeof(WriteBuildLockFile).Assembly.Location);

        public string TestRootPath
        {
            get
            {
                Directory.CreateDirectory(_testRootPath);
                return _testRootPath;
            }
        }

        public string TaskAssemblyFullPath => TaskAssemblyFullPathLazy.Value;

        public static bool IsWindows { get; } = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Directory.Exists(TestRootPath))
                {
                    Directory.Delete(TestRootPath, recursive: true);
                }
            }
        }

        protected string GetTempFileName(string extension = null)
        {
            Directory.CreateDirectory(TestRootPath);

            return Path.Combine(TestRootPath, $"{Path.GetRandomFileName()}{extension ?? string.Empty}");
        }

        protected string GetTempProjectFile(string name, params string[] files)
        {
            DirectoryInfo projectDirectory = Directory.CreateDirectory(Path.Combine(TestRootPath, name));

            foreach (string file in files)
            {
                File.WriteAllText(Path.Combine(projectDirectory.FullName, file), string.Empty);
            }

            return Path.Combine(projectDirectory.FullName, $"{name}.csproj");
        }
    }
}
