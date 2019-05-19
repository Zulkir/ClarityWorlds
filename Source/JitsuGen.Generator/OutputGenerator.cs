using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using JitsuGen.Core;
using JitsuGen.Core.Contexts;

namespace JitsuGen.Generator
{
    public class OutputGenerator
    {
        private readonly Dictionary<Type, IFileGenerator> generators;
        private readonly Dictionary<string, IFileGeneratingContext> generatingContexts;

        public OutputGenerator()
        {
            generators = new Dictionary<Type, IFileGenerator>();
            generatingContexts = new Dictionary<string, IFileGeneratingContext>();
            var fileWriter = new DirectFileWriter();
            var csharpContext = new CSharpFileGeneratingContext(fileWriter);
            generatingContexts.Add(csharpContext.FileType, csharpContext);
        }

        private IFileGenerator GetGenerator(Type generatorType)
        {
            if (!generators.TryGetValue(generatorType, out var generator))
            {
                var ctor = generatorType.GetConstructor(Type.EmptyTypes);
                if (ctor == null)
                    throw new Exception($"Necessary parameter-less constructor was not found for the generator type '{generatorType.Name}'.");
                generator = (IFileGenerator)ctor.Invoke(new object[0]);
                generators.Add(generatorType, generator);
            }
            return generator;
        }

        public void GenerateAll(GeneratorConfig config)
        {
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var directories = new[] {baseDir}
                .Concat(config.AdditionalFoldersToSearch.Select(x => Path.Combine(baseDir, x)))
                .ToArray();

            foreach (var dir in directories)
            {
                foreach (var file in Directory.EnumerateFiles(dir))
                {
                    var fileName = Path.GetFileName(file);
                    if (config.AssembliesToIgnore.Contains(fileName))
                        continue;

                    var extension = Path.GetExtension(file);
                    if (extension != ".dll" && extension != ".exe")
                        continue;

                    try
                    {
                        var assembly = Assembly.LoadFile(file);
                        foreach (var type in assembly.ExportedTypes)
                        {
                            var attr = type.GetCustomAttribute<JitsuGenAttribute>();
                            if (attr == null)
                                continue;
                            var ignoreAttr = type.GetCustomAttribute<JitsuGenIgnoreAttribute>(false);
                            if (ignoreAttr != null)
                            {
                                Console.WriteLine($"Ignoring type '{type.Name}' due to the attribute.");
                                continue;
                            }
                            try
                            {
                                Console.WriteLine($"Generating a file for the type '{type.Name}'...");
                                var generator = GetGenerator(attr.GeneratorType);
                                var context = generatingContexts[generator.FileType];
                                generator.GenerateFor(context, type);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine($"Failed to generate a file for type '{type.Name}':\n{e.Message}");
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Failed to load an assebly '{file}': {e.Message}");
                    }
                }
                foreach (var context in generatingContexts.Values)
                {
                    try
                    {
                        context.Dispose();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Failed to finalize a context '{context.FileType}':\n{e.Message}");
                    }
                }
            }
        }
    }
}