using System.IO;
using System.Xml.Serialization;

namespace JitsuGen.Generator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Run();
        }

        private static void Run()
        {
            var xmlSerializer = new XmlSerializer(typeof(GeneratorConfig));
            GeneratorConfig config;
            try
            {
                using (var stream = File.OpenRead("JitsuGen.Generator.cfg"))
                    config = (GeneratorConfig)xmlSerializer.Deserialize(stream);
            }
            catch
            {
                config = new GeneratorConfig();
                using (var stream = File.Open("JitsuGen.Generator.cfg", FileMode.Create))
                    xmlSerializer.Serialize(stream, config);
            }
            var generator = new OutputGenerator();
            generator.GenerateAll(config);
        }
    }
}
