using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using JitsuGen.Core.Writers;

namespace JitsuGen.Core.Contexts
{
    public class CSharpFileGeneratingContext : IFileGeneratingContext
    {
        public string FileType => ".cs";

        private readonly string csprojDirPath;
        private readonly IFileWriter fileWriter;
        private readonly List<string> files;
        private readonly List<KeyValuePair<Type, string>> implementations;
        private readonly HashSet<Type> typesUsed;

        public CSharpFileGeneratingContext(IFileWriter fileWriter)
        {
            this.fileWriter = fileWriter;
            files = new List<string>();
            typesUsed = new HashSet<Type>();
            implementations = new List<KeyValuePair<Type, string>>();
            csprojDirPath = Path.GetFullPath("JitsuGenOutput/CS/");
        }

        public void AddFile(GeneratedFileInfo fileInfo)
        {
            if (fileInfo.TypesUsed != null)
                foreach (var type in fileInfo.TypesUsed)
                    typesUsed.Add(type);
            if (fileInfo.Template != null && fileInfo.ImplementationFullName != null)
                implementations.Add(new KeyValuePair<Type, string>(fileInfo.Template, fileInfo.ImplementationFullName));
            files.Add(fileInfo.Path);
            fileWriter.WriteFile(Path.Combine(csprojDirPath, fileInfo.Path), fileInfo.Content, Encoding.UTF8);
        }
        
        public void Dispose()
        {
            AddGenOutputFile();
            var csproj = GenerateCsprojFile();
            fileWriter.WriteFile(Path.Combine(csprojDirPath, "JitsuGen.Output.csproj"), csproj, Encoding.UTF8);
        }

        private void AddGenOutputFile()
        {
            var writer = new CSharpWriter();
            writer.WriteLine();
            writer.WriteLine("namespace JitsuGen.Output");
            using (writer.Curly())
            {
                writer.WriteLine("public static class GenOutput");
                using (writer.Curly())
                {
                    writer.WriteLine($"public static void FillDomain({writer.UseType(typeof(IGenDomain))} genDomain)");
                    using (writer.Curly())
                    {
                        foreach (var impl in implementations)
                            writer.WriteLine($"genDomain.RegisterGeneratedType(typeof({writer.UseType(impl.Key)}), typeof({impl.Value}));");
                    }
                }
            }
            writer.Builder.Insert(0, writer.GetUsingsString());
            AddFile(new GeneratedFileInfo
            {
                Path = "GenOutput.cs",
                TypesUsed = writer.GetUsedTypes(),
                Content = writer.Builder.ToString()
            });
        }

        private string GenerateCsprojFile()
        {
            var builder = new StringBuilder();
            builder.AppendLine(@"<?xml version=""1.0"" encoding=""utf-8""?>
<Project ToolsVersion=""15.0"" xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">
  <Import Project=""$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props"" Condition=""Exists('$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props')"" />
  <PropertyGroup>
    <Configuration Condition="" '$(Configuration)' == '' "">Debug</Configuration>
    <Platform Condition="" '$(Platform)' == '' "">AnyCPU</Platform>
    <ProjectGuid>{604E2A8A-A609-464F-9530-456AD9C8F02A}</ProjectGuid>
    <OutputType>Library</OutputType>
    <AppDesignerFolder>Properties</AppDesignerFolder>
    <RootNamespace>JitsuGen.Output</RootNamespace>
    <AssemblyName>JitsuGen.Output</AssemblyName>
    <TargetFrameworkVersion>v4.5</TargetFrameworkVersion>
    <FileAlignment>512</FileAlignment>
  </PropertyGroup>
  <PropertyGroup Condition="" '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' "">
    <DebugSymbols>true</DebugSymbols>
    <DebugType>full</DebugType>
    <Optimize>false</Optimize>
    <OutputPath>../../</OutputPath>
    <DefineConstants>DEBUG;TRACE</DefineConstants>
    <ErrorReport>prompt</ErrorReport>
    <WarningLevel>4</WarningLevel>
    <AllowUnsafeBlocks>true</AllowUnsafeBlocks>
  </PropertyGroup>
  <PropertyGroup Condition="" '$(Configuration)|$(Platform)' == 'Release|AnyCPU' "">
    <DebugType>pdbonly</DebugType>
    <Optimize>true</Optimize>
    <OutputPath>../../</OutputPath>
    <DefineConstants>TRACE</DefineConstants>
    <ErrorReport>prompt</ErrorReport>
    <WarningLevel>4</WarningLevel>
    <AllowUnsafeBlocks>true</AllowUnsafeBlocks>
  </PropertyGroup>");
            builder.AppendLine(@"  <ItemGroup>");
            var references = typesUsed.Select(x => x.Assembly).Distinct().ToArray();
            foreach (var referenceAssembly in references)
            {
                builder.AppendLine($@"    <Reference Include=""{referenceAssembly.FullName}"">");
                builder.AppendLine(@"      <SpecificVersion>False</SpecificVersion>");
                builder.AppendLine($@"      <HintPath>{referenceAssembly.Location}</HintPath>");
                builder.AppendLine(@"      <Private>False</Private>");
                builder.AppendLine(@"    </Reference>");
            }
            builder.AppendLine(@"  </ItemGroup>");
            builder.AppendLine(@"  <ItemGroup>");
            foreach (var file in files)
                builder.AppendLine($@"    <Compile Include=""{file}"" />");
            builder.AppendLine(@"  </ItemGroup>");
            builder.AppendLine(@"  <Import Project=""$(MSBuildToolsPath)\Microsoft.CSharp.targets"" />");
            builder.AppendLine(@"</Project>");
            return builder.ToString();
        }
    }
}