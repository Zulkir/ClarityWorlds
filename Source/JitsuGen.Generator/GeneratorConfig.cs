using System.Xml.Serialization;

namespace JitsuGen.Generator
{
    [XmlRoot]
    public class GeneratorConfig
    {
        public string[] AdditionalFoldersToSearch { get; set; }
        public string[] AssembliesToIgnore { get; set; }

        public GeneratorConfig()
        {
            AdditionalFoldersToSearch = new string[0];
            AssembliesToIgnore = new[]{ "JitsuGen.Generator.exe","JitsuGen.Output.dll" };
        }
    }
}