// Copyright (c) Microsoft Corporation.
//
// Licensed under the MIT license.

using Microsoft.Build.Utilities.ProjectCreation;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace MSBuildSummaryFiles.Tasks.UnitTests
{
    public abstract class TestBase : MSBuildTestBase
    {
#if NETFRAMEWORK
        protected const string CurrentTargetFramework = "net472";
#elif NET5_0
        protected const string CurrentTargetFramework = "net5.0";
#elif NET6_0
        protected const string CurrentTargetFramework = "net6.0";
#else
        protected const string CurrentTargetFramework = "netcoreapp3.1";
#endif

        private static readonly Lazy<string> TaskAssemblyFullPathLazy = new Lazy<string>(() => typeof(WriteBuildSummaryFile).Assembly.Location);

        private readonly string _testRootPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

        public static bool IsWindows { get; } = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        public string TaskAssemblyFullPath => TaskAssemblyFullPathLazy.Value;

        public string TestRootPath
        {
            get
            {
                Directory.CreateDirectory(_testRootPath);
                return _testRootPath;
            }
        }

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
                File.WriteAllText(Path.Combine(projectDirectory.FullName, file), GetFileContent(file));
            }

            return Path.Combine(projectDirectory.FullName, $"{name}.csproj");
        }

        private string GetFileContent(string file)
        {
            string content = string.Empty;

            if (string.Equals(file, "strings.resx", StringComparison.OrdinalIgnoreCase))
            {
                content = GetTestStringResxContent();
            }

            return content;
        }

        private string GetTestStringResxContent()
        {
            return @"<?xml version=""1.0"" encoding=""utf-8""?>
<root>
  <xsd:schema id=""root"" xmlns="""" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:msdata=""urn:schemas-microsoft-com:xml-msdata"">
    <xsd:import namespace=""http://www.w3.org/XML/1998/namespace"" />
    <xsd:element name=""root"" msdata:IsDataSet=""true"">
    </xsd:element>
  </xsd:schema>
  <resheader name=""resmimetype"">
    <value>text/microsoft-resx</value>
  </resheader>
  <resheader name=""version"">
    <value>2.0</value>
  </resheader>
  <resheader name=""reader"">
    <value>System.Resources.ResXResourceReader, System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</value>
  </resheader>
  <resheader name=""writer"">
    <value>System.Resources.ResXResourceWriter, System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</value>
  </resheader>
</root>";
        }
    }
}