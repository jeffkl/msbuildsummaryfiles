using System;
using System.IO;
using Microsoft.Build.Utilities.ProjectCreation;
using Shouldly;
using Xunit;

namespace MSBuildLockFiles.Tasks.UnitTests
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
                .TryGetPropertyValue("BuildLockFilePath", out string buildLockFilePath);

            result.ShouldBeTrue(buildOutput.GetConsoleLog());

            File.ReadAllText(buildLockFilePath).ShouldBe(
#if NETFRAMEWORK
                @"net46:
  constants:
  - DEBUG
  - NET20_OR_GREATER
  - NET30_OR_GREATER
  - NET35_OR_GREATER
  - NET40_OR_GREATER
  - NET45_OR_GREATER
  - NET451_OR_GREATER
  - NET452_OR_GREATER
  - NET46
  - NET46_OR_GREATER
  - NETFRAMEWORK
  - TRACE
  outputs:
  - ProjectA.dll
  - ProjectA.pdb
  references:
  - {FrameworkAssemblies}/mscorlib.dll
  - {FrameworkAssemblies}/System.Core.dll
  - {FrameworkAssemblies}/System.Data.dll
  - {FrameworkAssemblies}/System.dll
  - {FrameworkAssemblies}/System.Drawing.dll
  - {FrameworkAssemblies}/System.IO.Compression.FileSystem.dll
  - {FrameworkAssemblies}/System.Numerics.dll
  - {FrameworkAssemblies}/System.Runtime.Serialization.dll
  - {FrameworkAssemblies}/System.Xml.dll
  - {FrameworkAssemblies}/System.Xml.Linq.dll
  sources:
  - AAA.cs
  - BBB.cs
  - strings.resx
net472:
  constants:
  - DEBUG
  - NET20_OR_GREATER
  - NET30_OR_GREATER
  - NET35_OR_GREATER
  - NET40_OR_GREATER
  - NET45_OR_GREATER
  - NET451_OR_GREATER
  - NET452_OR_GREATER
  - NET46_OR_GREATER
  - NET461_OR_GREATER
  - NET462_OR_GREATER
  - NET47_OR_GREATER
  - NET471_OR_GREATER
  - NET472
  - NET472_OR_GREATER
  - NETFRAMEWORK
  - TRACE
  outputs:
  - ProjectA.dll
  - ProjectA.pdb
  references:
  - {FrameworkAssemblies}/mscorlib.dll
  - {FrameworkAssemblies}/System.Core.dll
  - {FrameworkAssemblies}/System.Data.dll
  - {FrameworkAssemblies}/System.dll
  - {FrameworkAssemblies}/System.Drawing.dll
  - {FrameworkAssemblies}/System.IO.Compression.FileSystem.dll
  - {FrameworkAssemblies}/System.Numerics.dll
  - {FrameworkAssemblies}/System.Runtime.Serialization.dll
  - {FrameworkAssemblies}/System.Xml.dll
  - {FrameworkAssemblies}/System.Xml.Linq.dll
  sources:
  - AAA.cs
  - BBB.cs
  - strings.resx
",
#elif NET5_0
                @"netstandard2.0:
  constants:
  - DEBUG
  - NETSTANDARD
  - NETSTANDARD1_0_OR_GREATER
  - NETSTANDARD1_1_OR_GREATER
  - NETSTANDARD1_2_OR_GREATER
  - NETSTANDARD1_3_OR_GREATER
  - NETSTANDARD1_4_OR_GREATER
  - NETSTANDARD1_5_OR_GREATER
  - NETSTANDARD1_6_OR_GREATER
  - NETSTANDARD2_0
  - NETSTANDARD2_0_OR_GREATER
  - TRACE
  outputs:
  - ProjectA.deps.json
  - ProjectA.dll
  - ProjectA.pdb
  references:
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/Microsoft.Win32.Primitives.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/mscorlib.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/netstandard.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.AppContext.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Collections.Concurrent.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Collections.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Collections.NonGeneric.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Collections.Specialized.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.ComponentModel.Composition.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.ComponentModel.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.ComponentModel.EventBasedAsync.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.ComponentModel.Primitives.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.ComponentModel.TypeConverter.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Console.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Core.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Data.Common.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Data.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Diagnostics.Contracts.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Diagnostics.Debug.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Diagnostics.FileVersionInfo.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Diagnostics.Process.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Diagnostics.StackTrace.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Diagnostics.TextWriterTraceListener.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Diagnostics.Tools.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Diagnostics.TraceSource.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Diagnostics.Tracing.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Drawing.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Drawing.Primitives.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Dynamic.Runtime.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Globalization.Calendars.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Globalization.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Globalization.Extensions.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.IO.Compression.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.IO.Compression.FileSystem.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.IO.Compression.ZipFile.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.IO.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.IO.FileSystem.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.IO.FileSystem.DriveInfo.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.IO.FileSystem.Primitives.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.IO.FileSystem.Watcher.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.IO.IsolatedStorage.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.IO.MemoryMappedFiles.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.IO.Pipes.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.IO.UnmanagedMemoryStream.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Linq.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Linq.Expressions.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Linq.Parallel.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Linq.Queryable.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Net.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Net.Http.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Net.NameResolution.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Net.NetworkInformation.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Net.Ping.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Net.Primitives.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Net.Requests.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Net.Security.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Net.Sockets.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Net.WebHeaderCollection.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Net.WebSockets.Client.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Net.WebSockets.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Numerics.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.ObjectModel.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Reflection.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Reflection.Extensions.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Reflection.Primitives.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Resources.Reader.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Resources.ResourceManager.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Resources.Writer.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Runtime.CompilerServices.VisualC.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Runtime.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Runtime.Extensions.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Runtime.Handles.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Runtime.InteropServices.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Runtime.InteropServices.RuntimeInformation.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Runtime.Numerics.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Runtime.Serialization.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Runtime.Serialization.Formatters.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Runtime.Serialization.Json.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Runtime.Serialization.Primitives.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Runtime.Serialization.Xml.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Security.Claims.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Security.Cryptography.Algorithms.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Security.Cryptography.Csp.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Security.Cryptography.Encoding.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Security.Cryptography.Primitives.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Security.Cryptography.X509Certificates.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Security.Principal.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Security.SecureString.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.ServiceModel.Web.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Text.Encoding.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Text.Encoding.Extensions.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Text.RegularExpressions.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Threading.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Threading.Overlapped.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Threading.Tasks.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Threading.Tasks.Parallel.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Threading.Thread.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Threading.ThreadPool.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Threading.Timer.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Transactions.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.ValueTuple.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Web.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Windows.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Xml.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Xml.Linq.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Xml.ReaderWriter.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Xml.Serialization.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Xml.XDocument.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Xml.XmlDocument.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Xml.XmlSerializer.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Xml.XPath.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Xml.XPath.XDocument.dll
  sources:
  - AAA.cs
  - BBB.cs
  - strings.resx
netstandard2.1:
  constants:
  - DEBUG
  - NETSTANDARD
  - NETSTANDARD1_0_OR_GREATER
  - NETSTANDARD1_1_OR_GREATER
  - NETSTANDARD1_2_OR_GREATER
  - NETSTANDARD1_3_OR_GREATER
  - NETSTANDARD1_4_OR_GREATER
  - NETSTANDARD1_5_OR_GREATER
  - NETSTANDARD1_6_OR_GREATER
  - NETSTANDARD2_0_OR_GREATER
  - NETSTANDARD2_1
  - NETSTANDARD2_1_OR_GREATER
  - TRACE
  outputs:
  - ProjectA.deps.json
  - ProjectA.dll
  - ProjectA.pdb
  references:
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/Microsoft.Win32.Primitives.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/mscorlib.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/netstandard.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.AppContext.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Buffers.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Collections.Concurrent.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Collections.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Collections.NonGeneric.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Collections.Specialized.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.ComponentModel.Composition.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.ComponentModel.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.ComponentModel.EventBasedAsync.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.ComponentModel.Primitives.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.ComponentModel.TypeConverter.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Console.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Core.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Data.Common.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Data.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Diagnostics.Contracts.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Diagnostics.Debug.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Diagnostics.FileVersionInfo.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Diagnostics.Process.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Diagnostics.StackTrace.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Diagnostics.TextWriterTraceListener.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Diagnostics.Tools.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Diagnostics.TraceSource.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Diagnostics.Tracing.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Drawing.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Drawing.Primitives.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Dynamic.Runtime.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Globalization.Calendars.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Globalization.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Globalization.Extensions.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.IO.Compression.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.IO.Compression.FileSystem.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.IO.Compression.ZipFile.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.IO.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.IO.FileSystem.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.IO.FileSystem.DriveInfo.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.IO.FileSystem.Primitives.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.IO.FileSystem.Watcher.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.IO.IsolatedStorage.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.IO.MemoryMappedFiles.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.IO.Pipes.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.IO.UnmanagedMemoryStream.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Linq.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Linq.Expressions.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Linq.Parallel.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Linq.Queryable.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Memory.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Net.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Net.Http.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Net.NameResolution.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Net.NetworkInformation.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Net.Ping.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Net.Primitives.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Net.Requests.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Net.Security.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Net.Sockets.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Net.WebHeaderCollection.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Net.WebSockets.Client.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Net.WebSockets.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Numerics.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Numerics.Vectors.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.ObjectModel.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Reflection.DispatchProxy.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Reflection.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Reflection.Emit.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Reflection.Emit.ILGeneration.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Reflection.Emit.Lightweight.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Reflection.Extensions.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Reflection.Primitives.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Resources.Reader.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Resources.ResourceManager.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Resources.Writer.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Runtime.CompilerServices.VisualC.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Runtime.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Runtime.Extensions.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Runtime.Handles.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Runtime.InteropServices.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Runtime.InteropServices.RuntimeInformation.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Runtime.Numerics.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Runtime.Serialization.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Runtime.Serialization.Formatters.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Runtime.Serialization.Json.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Runtime.Serialization.Primitives.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Runtime.Serialization.Xml.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Security.Claims.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Security.Cryptography.Algorithms.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Security.Cryptography.Csp.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Security.Cryptography.Encoding.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Security.Cryptography.Primitives.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Security.Cryptography.X509Certificates.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Security.Principal.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Security.SecureString.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.ServiceModel.Web.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Text.Encoding.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Text.Encoding.Extensions.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Text.RegularExpressions.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Threading.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Threading.Overlapped.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Threading.Tasks.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Threading.Tasks.Extensions.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Threading.Tasks.Parallel.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Threading.Thread.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Threading.ThreadPool.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Threading.Timer.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Transactions.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.ValueTuple.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Web.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Windows.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Xml.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Xml.Linq.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Xml.ReaderWriter.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Xml.Serialization.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Xml.XDocument.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Xml.XmlDocument.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Xml.XmlSerializer.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Xml.XPath.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Xml.XPath.XDocument.dll
  sources:
  - AAA.cs
  - BBB.cs
  - strings.resx
",
#elif NET6_0
                @"netstandard2.0:
  constants:
  - DEBUG
  - NETSTANDARD
  - NETSTANDARD1_0_OR_GREATER
  - NETSTANDARD1_1_OR_GREATER
  - NETSTANDARD1_2_OR_GREATER
  - NETSTANDARD1_3_OR_GREATER
  - NETSTANDARD1_4_OR_GREATER
  - NETSTANDARD1_5_OR_GREATER
  - NETSTANDARD1_6_OR_GREATER
  - NETSTANDARD2_0
  - NETSTANDARD2_0_OR_GREATER
  - TRACE
  outputs:
  - ProjectA.deps.json
  - ProjectA.dll
  - ProjectA.pdb
  references:
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/Microsoft.Win32.Primitives.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/mscorlib.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/netstandard.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.AppContext.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Collections.Concurrent.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Collections.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Collections.NonGeneric.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Collections.Specialized.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.ComponentModel.Composition.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.ComponentModel.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.ComponentModel.EventBasedAsync.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.ComponentModel.Primitives.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.ComponentModel.TypeConverter.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Console.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Core.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Data.Common.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Data.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Diagnostics.Contracts.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Diagnostics.Debug.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Diagnostics.FileVersionInfo.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Diagnostics.Process.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Diagnostics.StackTrace.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Diagnostics.TextWriterTraceListener.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Diagnostics.Tools.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Diagnostics.TraceSource.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Diagnostics.Tracing.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Drawing.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Drawing.Primitives.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Dynamic.Runtime.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Globalization.Calendars.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Globalization.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Globalization.Extensions.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.IO.Compression.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.IO.Compression.FileSystem.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.IO.Compression.ZipFile.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.IO.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.IO.FileSystem.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.IO.FileSystem.DriveInfo.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.IO.FileSystem.Primitives.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.IO.FileSystem.Watcher.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.IO.IsolatedStorage.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.IO.MemoryMappedFiles.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.IO.Pipes.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.IO.UnmanagedMemoryStream.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Linq.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Linq.Expressions.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Linq.Parallel.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Linq.Queryable.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Net.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Net.Http.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Net.NameResolution.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Net.NetworkInformation.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Net.Ping.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Net.Primitives.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Net.Requests.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Net.Security.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Net.Sockets.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Net.WebHeaderCollection.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Net.WebSockets.Client.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Net.WebSockets.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Numerics.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.ObjectModel.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Reflection.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Reflection.Extensions.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Reflection.Primitives.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Resources.Reader.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Resources.ResourceManager.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Resources.Writer.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Runtime.CompilerServices.VisualC.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Runtime.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Runtime.Extensions.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Runtime.Handles.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Runtime.InteropServices.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Runtime.InteropServices.RuntimeInformation.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Runtime.Numerics.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Runtime.Serialization.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Runtime.Serialization.Formatters.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Runtime.Serialization.Json.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Runtime.Serialization.Primitives.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Runtime.Serialization.Xml.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Security.Claims.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Security.Cryptography.Algorithms.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Security.Cryptography.Csp.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Security.Cryptography.Encoding.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Security.Cryptography.Primitives.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Security.Cryptography.X509Certificates.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Security.Principal.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Security.SecureString.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.ServiceModel.Web.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Text.Encoding.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Text.Encoding.Extensions.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Text.RegularExpressions.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Threading.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Threading.Overlapped.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Threading.Tasks.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Threading.Tasks.Parallel.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Threading.Thread.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Threading.ThreadPool.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Threading.Timer.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Transactions.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.ValueTuple.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Web.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Windows.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Xml.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Xml.Linq.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Xml.ReaderWriter.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Xml.Serialization.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Xml.XDocument.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Xml.XmlDocument.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Xml.XmlSerializer.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Xml.XPath.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Xml.XPath.XDocument.dll
  sources:
  - AAA.cs
  - BBB.cs
  - strings.resx
netstandard2.1:
  constants:
  - DEBUG
  - NETSTANDARD
  - NETSTANDARD1_0_OR_GREATER
  - NETSTANDARD1_1_OR_GREATER
  - NETSTANDARD1_2_OR_GREATER
  - NETSTANDARD1_3_OR_GREATER
  - NETSTANDARD1_4_OR_GREATER
  - NETSTANDARD1_5_OR_GREATER
  - NETSTANDARD1_6_OR_GREATER
  - NETSTANDARD2_0_OR_GREATER
  - NETSTANDARD2_1
  - NETSTANDARD2_1_OR_GREATER
  - TRACE
  outputs:
  - ProjectA.deps.json
  - ProjectA.dll
  - ProjectA.pdb
  references:
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/Microsoft.Win32.Primitives.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/mscorlib.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/netstandard.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.AppContext.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Buffers.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Collections.Concurrent.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Collections.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Collections.NonGeneric.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Collections.Specialized.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.ComponentModel.Composition.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.ComponentModel.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.ComponentModel.EventBasedAsync.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.ComponentModel.Primitives.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.ComponentModel.TypeConverter.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Console.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Core.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Data.Common.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Data.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Diagnostics.Contracts.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Diagnostics.Debug.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Diagnostics.FileVersionInfo.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Diagnostics.Process.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Diagnostics.StackTrace.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Diagnostics.TextWriterTraceListener.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Diagnostics.Tools.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Diagnostics.TraceSource.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Diagnostics.Tracing.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Drawing.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Drawing.Primitives.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Dynamic.Runtime.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Globalization.Calendars.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Globalization.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Globalization.Extensions.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.IO.Compression.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.IO.Compression.FileSystem.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.IO.Compression.ZipFile.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.IO.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.IO.FileSystem.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.IO.FileSystem.DriveInfo.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.IO.FileSystem.Primitives.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.IO.FileSystem.Watcher.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.IO.IsolatedStorage.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.IO.MemoryMappedFiles.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.IO.Pipes.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.IO.UnmanagedMemoryStream.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Linq.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Linq.Expressions.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Linq.Parallel.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Linq.Queryable.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Memory.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Net.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Net.Http.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Net.NameResolution.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Net.NetworkInformation.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Net.Ping.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Net.Primitives.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Net.Requests.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Net.Security.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Net.Sockets.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Net.WebHeaderCollection.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Net.WebSockets.Client.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Net.WebSockets.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Numerics.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Numerics.Vectors.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.ObjectModel.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Reflection.DispatchProxy.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Reflection.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Reflection.Emit.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Reflection.Emit.ILGeneration.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Reflection.Emit.Lightweight.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Reflection.Extensions.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Reflection.Primitives.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Resources.Reader.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Resources.ResourceManager.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Resources.Writer.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Runtime.CompilerServices.VisualC.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Runtime.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Runtime.Extensions.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Runtime.Handles.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Runtime.InteropServices.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Runtime.InteropServices.RuntimeInformation.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Runtime.Numerics.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Runtime.Serialization.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Runtime.Serialization.Formatters.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Runtime.Serialization.Json.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Runtime.Serialization.Primitives.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Runtime.Serialization.Xml.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Security.Claims.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Security.Cryptography.Algorithms.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Security.Cryptography.Csp.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Security.Cryptography.Encoding.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Security.Cryptography.Primitives.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Security.Cryptography.X509Certificates.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Security.Principal.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Security.SecureString.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.ServiceModel.Web.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Text.Encoding.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Text.Encoding.Extensions.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Text.RegularExpressions.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Threading.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Threading.Overlapped.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Threading.Tasks.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Threading.Tasks.Extensions.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Threading.Tasks.Parallel.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Threading.Thread.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Threading.ThreadPool.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Threading.Timer.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Transactions.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.ValueTuple.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Web.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Windows.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Xml.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Xml.Linq.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Xml.ReaderWriter.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Xml.Serialization.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Xml.XDocument.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Xml.XmlDocument.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Xml.XmlSerializer.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Xml.XPath.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Xml.XPath.XDocument.dll
  sources:
  - AAA.cs
  - BBB.cs
  - strings.resx
",
#else
                @"netstandard2.0:
  constants:
  - DEBUG
  - NETSTANDARD
  - NETSTANDARD2_0
  - TRACE
  outputs:
  - ProjectA.deps.json
  - ProjectA.dll
  - ProjectA.pdb
  references:
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/Microsoft.Win32.Primitives.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/mscorlib.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/netstandard.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.AppContext.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Collections.Concurrent.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Collections.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Collections.NonGeneric.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Collections.Specialized.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.ComponentModel.Composition.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.ComponentModel.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.ComponentModel.EventBasedAsync.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.ComponentModel.Primitives.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.ComponentModel.TypeConverter.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Console.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Core.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Data.Common.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Data.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Diagnostics.Contracts.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Diagnostics.Debug.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Diagnostics.FileVersionInfo.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Diagnostics.Process.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Diagnostics.StackTrace.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Diagnostics.TextWriterTraceListener.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Diagnostics.Tools.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Diagnostics.TraceSource.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Diagnostics.Tracing.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Drawing.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Drawing.Primitives.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Dynamic.Runtime.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Globalization.Calendars.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Globalization.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Globalization.Extensions.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.IO.Compression.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.IO.Compression.FileSystem.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.IO.Compression.ZipFile.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.IO.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.IO.FileSystem.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.IO.FileSystem.DriveInfo.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.IO.FileSystem.Primitives.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.IO.FileSystem.Watcher.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.IO.IsolatedStorage.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.IO.MemoryMappedFiles.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.IO.Pipes.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.IO.UnmanagedMemoryStream.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Linq.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Linq.Expressions.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Linq.Parallel.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Linq.Queryable.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Net.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Net.Http.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Net.NameResolution.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Net.NetworkInformation.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Net.Ping.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Net.Primitives.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Net.Requests.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Net.Security.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Net.Sockets.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Net.WebHeaderCollection.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Net.WebSockets.Client.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Net.WebSockets.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Numerics.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.ObjectModel.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Reflection.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Reflection.Extensions.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Reflection.Primitives.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Resources.Reader.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Resources.ResourceManager.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Resources.Writer.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Runtime.CompilerServices.VisualC.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Runtime.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Runtime.Extensions.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Runtime.Handles.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Runtime.InteropServices.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Runtime.InteropServices.RuntimeInformation.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Runtime.Numerics.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Runtime.Serialization.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Runtime.Serialization.Formatters.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Runtime.Serialization.Json.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Runtime.Serialization.Primitives.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Runtime.Serialization.Xml.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Security.Claims.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Security.Cryptography.Algorithms.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Security.Cryptography.Csp.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Security.Cryptography.Encoding.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Security.Cryptography.Primitives.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Security.Cryptography.X509Certificates.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Security.Principal.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Security.SecureString.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.ServiceModel.Web.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Text.Encoding.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Text.Encoding.Extensions.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Text.RegularExpressions.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Threading.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Threading.Overlapped.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Threading.Tasks.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Threading.Tasks.Parallel.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Threading.Thread.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Threading.ThreadPool.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Threading.Timer.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Transactions.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.ValueTuple.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Web.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Windows.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Xml.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Xml.Linq.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Xml.ReaderWriter.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Xml.Serialization.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Xml.XDocument.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Xml.XmlDocument.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Xml.XmlSerializer.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Xml.XPath.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Xml.XPath.XDocument.dll
  sources:
  - AAA.cs
  - BBB.cs
  - strings.resx
netstandard2.1:
  constants:
  - DEBUG
  - NETSTANDARD
  - NETSTANDARD2_1
  - TRACE
  outputs:
  - ProjectA.deps.json
  - ProjectA.dll
  - ProjectA.pdb
  references:
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/Microsoft.Win32.Primitives.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/mscorlib.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/netstandard.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.AppContext.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Buffers.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Collections.Concurrent.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Collections.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Collections.NonGeneric.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Collections.Specialized.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.ComponentModel.Composition.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.ComponentModel.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.ComponentModel.EventBasedAsync.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.ComponentModel.Primitives.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.ComponentModel.TypeConverter.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Console.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Core.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Data.Common.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Data.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Diagnostics.Contracts.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Diagnostics.Debug.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Diagnostics.FileVersionInfo.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Diagnostics.Process.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Diagnostics.StackTrace.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Diagnostics.TextWriterTraceListener.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Diagnostics.Tools.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Diagnostics.TraceSource.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Diagnostics.Tracing.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Drawing.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Drawing.Primitives.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Dynamic.Runtime.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Globalization.Calendars.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Globalization.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Globalization.Extensions.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.IO.Compression.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.IO.Compression.FileSystem.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.IO.Compression.ZipFile.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.IO.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.IO.FileSystem.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.IO.FileSystem.DriveInfo.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.IO.FileSystem.Primitives.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.IO.FileSystem.Watcher.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.IO.IsolatedStorage.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.IO.MemoryMappedFiles.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.IO.Pipes.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.IO.UnmanagedMemoryStream.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Linq.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Linq.Expressions.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Linq.Parallel.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Linq.Queryable.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Memory.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Net.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Net.Http.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Net.NameResolution.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Net.NetworkInformation.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Net.Ping.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Net.Primitives.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Net.Requests.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Net.Security.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Net.Sockets.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Net.WebHeaderCollection.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Net.WebSockets.Client.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Net.WebSockets.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Numerics.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Numerics.Vectors.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.ObjectModel.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Reflection.DispatchProxy.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Reflection.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Reflection.Emit.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Reflection.Emit.ILGeneration.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Reflection.Emit.Lightweight.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Reflection.Extensions.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Reflection.Primitives.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Resources.Reader.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Resources.ResourceManager.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Resources.Writer.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Runtime.CompilerServices.VisualC.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Runtime.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Runtime.Extensions.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Runtime.Handles.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Runtime.InteropServices.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Runtime.InteropServices.RuntimeInformation.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Runtime.Numerics.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Runtime.Serialization.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Runtime.Serialization.Formatters.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Runtime.Serialization.Json.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Runtime.Serialization.Primitives.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Runtime.Serialization.Xml.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Security.Claims.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Security.Cryptography.Algorithms.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Security.Cryptography.Csp.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Security.Cryptography.Encoding.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Security.Cryptography.Primitives.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Security.Cryptography.X509Certificates.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Security.Principal.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Security.SecureString.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.ServiceModel.Web.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Text.Encoding.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Text.Encoding.Extensions.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Text.RegularExpressions.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Threading.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Threading.Overlapped.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Threading.Tasks.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Threading.Tasks.Extensions.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Threading.Tasks.Parallel.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Threading.Thread.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Threading.ThreadPool.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Threading.Timer.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Transactions.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.ValueTuple.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Web.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Windows.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Xml.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Xml.Linq.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Xml.ReaderWriter.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Xml.Serialization.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Xml.XDocument.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Xml.XmlDocument.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Xml.XmlSerializer.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Xml.XPath.dll
  - {NetCoreTargetingPackRoot}/NETStandard.Library.Ref/2.1.0/ref/netstandard2.1/System.Xml.XPath.XDocument.dll
  sources:
  - AAA.cs
  - BBB.cs
  - strings.resx
",
#endif
                StringCompareShould.IgnoreLineEndings);
        }

        [Fact]
        public void SingleTargetingBuild()
        {
            CreateSdkStyleProject("netstandard2.0")
                .Save(GetTempProjectFile("ProjectA", "Strings.resx"))
                .TryBuild(restore: true, out bool result, out BuildOutput buildOutput)
                .TryGetPropertyValue("BuildLockFilePath", out string buildLockFilePath);

            result.ShouldBeTrue(buildOutput.GetConsoleLog());

            buildLockFilePath.ShouldNotBeNullOrEmpty();

            File.ReadAllText(buildLockFilePath).ShouldBe(
#if NETFRAMEWORK || NET5_0_OR_GREATER
                @"netstandard2.0:
  constants:
  - DEBUG
  - NETSTANDARD
  - NETSTANDARD1_0_OR_GREATER
  - NETSTANDARD1_1_OR_GREATER
  - NETSTANDARD1_2_OR_GREATER
  - NETSTANDARD1_3_OR_GREATER
  - NETSTANDARD1_4_OR_GREATER
  - NETSTANDARD1_5_OR_GREATER
  - NETSTANDARD1_6_OR_GREATER
  - NETSTANDARD2_0
  - NETSTANDARD2_0_OR_GREATER
  - TRACE
  outputs:
  - ProjectA.deps.json
  - ProjectA.dll
  - ProjectA.pdb
  references:
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/Microsoft.Win32.Primitives.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/mscorlib.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/netstandard.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.AppContext.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Collections.Concurrent.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Collections.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Collections.NonGeneric.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Collections.Specialized.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.ComponentModel.Composition.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.ComponentModel.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.ComponentModel.EventBasedAsync.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.ComponentModel.Primitives.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.ComponentModel.TypeConverter.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Console.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Core.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Data.Common.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Data.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Diagnostics.Contracts.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Diagnostics.Debug.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Diagnostics.FileVersionInfo.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Diagnostics.Process.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Diagnostics.StackTrace.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Diagnostics.TextWriterTraceListener.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Diagnostics.Tools.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Diagnostics.TraceSource.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Diagnostics.Tracing.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Drawing.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Drawing.Primitives.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Dynamic.Runtime.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Globalization.Calendars.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Globalization.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Globalization.Extensions.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.IO.Compression.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.IO.Compression.FileSystem.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.IO.Compression.ZipFile.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.IO.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.IO.FileSystem.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.IO.FileSystem.DriveInfo.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.IO.FileSystem.Primitives.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.IO.FileSystem.Watcher.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.IO.IsolatedStorage.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.IO.MemoryMappedFiles.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.IO.Pipes.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.IO.UnmanagedMemoryStream.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Linq.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Linq.Expressions.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Linq.Parallel.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Linq.Queryable.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Net.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Net.Http.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Net.NameResolution.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Net.NetworkInformation.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Net.Ping.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Net.Primitives.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Net.Requests.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Net.Security.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Net.Sockets.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Net.WebHeaderCollection.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Net.WebSockets.Client.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Net.WebSockets.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Numerics.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.ObjectModel.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Reflection.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Reflection.Extensions.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Reflection.Primitives.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Resources.Reader.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Resources.ResourceManager.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Resources.Writer.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Runtime.CompilerServices.VisualC.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Runtime.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Runtime.Extensions.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Runtime.Handles.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Runtime.InteropServices.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Runtime.InteropServices.RuntimeInformation.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Runtime.Numerics.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Runtime.Serialization.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Runtime.Serialization.Formatters.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Runtime.Serialization.Json.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Runtime.Serialization.Primitives.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Runtime.Serialization.Xml.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Security.Claims.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Security.Cryptography.Algorithms.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Security.Cryptography.Csp.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Security.Cryptography.Encoding.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Security.Cryptography.Primitives.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Security.Cryptography.X509Certificates.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Security.Principal.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Security.SecureString.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.ServiceModel.Web.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Text.Encoding.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Text.Encoding.Extensions.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Text.RegularExpressions.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Threading.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Threading.Overlapped.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Threading.Tasks.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Threading.Tasks.Parallel.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Threading.Thread.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Threading.ThreadPool.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Threading.Timer.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Transactions.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.ValueTuple.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Web.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Windows.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Xml.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Xml.Linq.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Xml.ReaderWriter.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Xml.Serialization.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Xml.XDocument.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Xml.XmlDocument.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Xml.XmlSerializer.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Xml.XPath.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Xml.XPath.XDocument.dll
  sources:
  - Strings.resx
",
#else
                @"netstandard2.0:
  constants:
  - DEBUG
  - NETSTANDARD
  - NETSTANDARD2_0
  - TRACE
  outputs:
  - ProjectA.deps.json
  - ProjectA.dll
  - ProjectA.pdb
  references:
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/Microsoft.Win32.Primitives.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/mscorlib.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/netstandard.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.AppContext.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Collections.Concurrent.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Collections.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Collections.NonGeneric.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Collections.Specialized.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.ComponentModel.Composition.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.ComponentModel.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.ComponentModel.EventBasedAsync.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.ComponentModel.Primitives.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.ComponentModel.TypeConverter.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Console.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Core.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Data.Common.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Data.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Diagnostics.Contracts.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Diagnostics.Debug.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Diagnostics.FileVersionInfo.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Diagnostics.Process.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Diagnostics.StackTrace.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Diagnostics.TextWriterTraceListener.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Diagnostics.Tools.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Diagnostics.TraceSource.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Diagnostics.Tracing.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Drawing.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Drawing.Primitives.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Dynamic.Runtime.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Globalization.Calendars.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Globalization.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Globalization.Extensions.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.IO.Compression.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.IO.Compression.FileSystem.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.IO.Compression.ZipFile.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.IO.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.IO.FileSystem.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.IO.FileSystem.DriveInfo.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.IO.FileSystem.Primitives.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.IO.FileSystem.Watcher.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.IO.IsolatedStorage.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.IO.MemoryMappedFiles.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.IO.Pipes.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.IO.UnmanagedMemoryStream.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Linq.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Linq.Expressions.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Linq.Parallel.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Linq.Queryable.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Net.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Net.Http.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Net.NameResolution.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Net.NetworkInformation.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Net.Ping.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Net.Primitives.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Net.Requests.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Net.Security.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Net.Sockets.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Net.WebHeaderCollection.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Net.WebSockets.Client.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Net.WebSockets.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Numerics.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.ObjectModel.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Reflection.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Reflection.Extensions.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Reflection.Primitives.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Resources.Reader.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Resources.ResourceManager.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Resources.Writer.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Runtime.CompilerServices.VisualC.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Runtime.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Runtime.Extensions.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Runtime.Handles.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Runtime.InteropServices.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Runtime.InteropServices.RuntimeInformation.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Runtime.Numerics.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Runtime.Serialization.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Runtime.Serialization.Formatters.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Runtime.Serialization.Json.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Runtime.Serialization.Primitives.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Runtime.Serialization.Xml.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Security.Claims.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Security.Cryptography.Algorithms.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Security.Cryptography.Csp.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Security.Cryptography.Encoding.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Security.Cryptography.Primitives.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Security.Cryptography.X509Certificates.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Security.Principal.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Security.SecureString.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.ServiceModel.Web.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Text.Encoding.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Text.Encoding.Extensions.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Text.RegularExpressions.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Threading.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Threading.Overlapped.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Threading.Tasks.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Threading.Tasks.Parallel.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Threading.Thread.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Threading.ThreadPool.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Threading.Timer.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Transactions.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.ValueTuple.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Web.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Windows.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Xml.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Xml.Linq.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Xml.ReaderWriter.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Xml.Serialization.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Xml.XDocument.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Xml.XmlDocument.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Xml.XmlSerializer.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Xml.XPath.dll
  - {NuGetPackageRoot}/netstandard.library/2.0.3/build/netstandard2.0/ref/System.Xml.XPath.XDocument.dll
  sources:
  - Strings.resx
",
#endif
                StringCompareShould.IgnoreLineEndings);
        }

#if NETFRAMEWORK
        [Fact]
        public void LegacyProject()
        {
            CreateLegacyProject("net472", "v4.7.2")
                .ItemCompile("Class1.cs")
                .Save(GetTempProjectFile("ProjectA", "Class1.cs"))
                .TryBuild(restore: true, out bool result, out BuildOutput buildOutput)
                .TryGetPropertyValue("BuildLockFilePath", out string buildLockFilePath);

            result.ShouldBeTrue(buildOutput.GetConsoleLog());

            buildLockFilePath.ShouldNotBeNullOrEmpty();

            File.ReadAllText(buildLockFilePath).ShouldBe(
                @"net472:
  constants:
  - DEBUG
  - TRACE
  outputs:
  - ClassLibrary.dll
  - ClassLibrary.pdb
  references:
  - {FrameworkAssemblies}/Microsoft.CSharp.dll
  - {FrameworkAssemblies}/mscorlib.dll
  - {FrameworkAssemblies}/System.Core.dll
  - {FrameworkAssemblies}/System.Data.DataSetExtensions.dll
  - {FrameworkAssemblies}/System.Data.dll
  - {FrameworkAssemblies}/System.dll
  - {FrameworkAssemblies}/System.Net.Http.dll
  - {FrameworkAssemblies}/System.Xml.dll
  - {FrameworkAssemblies}/System.Xml.Linq.dll
  sources:
  - Class1.cs
",
                StringCompareShould.IgnoreLineEndings);
        }
#endif

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
                .Property("MSBuildLockFilesTaskAssembly", TaskAssemblyFullPath)
                .Import(Path.Combine(Environment.CurrentDirectory, "build", "MSBuildLockFiles.Tasks.props"), condition: targetFrameworks.Length > 1 ? "'$(TargetFramework)' != ''" : null)
                .Import(Path.Combine(Environment.CurrentDirectory, "buildMultiTargeting", "MSBuildLockFiles.Tasks.props"), condition: targetFrameworks.Length > 1 ? "'$(TargetFramework)' == ''" : bool.FalseString)
                .Save();

            ProjectCreator
                .Create(Path.Combine(TestRootPath, "Directory.Build.targets"))
                .Import(Path.Combine(Environment.CurrentDirectory, "build", "MSBuildLockFiles.Tasks.targets"), condition: targetFrameworks.Length > 1 ? "'$(TargetFramework)' != ''" : null)
                .Import(Path.Combine(Environment.CurrentDirectory, "buildMultiTargeting", "MSBuildLockFiles.Tasks.targets"), condition: targetFrameworks.Length > 1 ? "'$(TargetFramework)' == ''" : bool.FalseString)
                .Save();
        }
    }
}
