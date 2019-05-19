using System;
using System.IO;
using System.Linq;
using JitsuGen.Core;
using JitsuGen.Core.Writers;

namespace JitsuGen.InfrastructureTest
{
    public class VerboseWrapperGenerator : IFileGenerator
    {
        public string FileType => ".cs";

        public void GenerateFor(IFileGeneratingContext context, Type type)
        {
            if (!type.IsInterface)
                throw new ArgumentException("VerboseWrapperGenerator only works with interfaces.");
            var className = "_verbose_" + type.Name;
            

            var writer = new CSharpWriter();
            writer.WriteLine();
            writer.WriteLine($"namespace {type.Namespace}");
            using (writer.Curly())
            {
                var interfaceName = writer.UseType(type);
                writer.WriteLine($"public class {className} : {interfaceName}");
                using (writer.Curly())
                {
                    writer.WriteLine($"private {interfaceName} real;");
                    writer.WriteLine();
                    writer.WriteLine($"public {className}({interfaceName} real)");
                    using (writer.Curly())
                        writer.WriteLine("this.real = real;");
                    writer.WriteLine();

                    foreach (var propertyInfo in type.GetProperties())
                    {
                        var writeGetter = propertyInfo.GetMethod != null
                            ? (Action<CSharpWriter>)(w =>
                            {
                                w.WriteLine($"{writer.UseType(typeof(Console))}.WriteLine($\"Getting {interfaceName}.{propertyInfo.Name}\");");
                                w.WriteLine($"return real.{propertyInfo.Name}; ");
                            })
                            : null;
                        var writeSetter = propertyInfo.SetMethod != null
                            ? (Action<CSharpWriter>)(w =>
                            {
                                w.WriteLine($"{writer.UseType(typeof(Console))}.WriteLine($\"Setting {interfaceName}.{propertyInfo.Name}\");");
                                w.WriteLine($"real.{propertyInfo.Name} = value;");
                            })
                            : null;
                        writer.WriteProperty("public", writer.UseType(propertyInfo.PropertyType), propertyInfo.Name, writeGetter, writeSetter);
                        writer.WriteLine();
                    }

                    foreach (var methodInfo in type.GetMethods())
                    {
                        if (methodInfo.IsSpecialName)
                            continue;

                        writer.WriteMethodDeclaration("public", methodInfo);
                        using (writer.Curly())
                        {
                            writer.WriteLine($"{writer.UseType(typeof(Console))}.WriteLine($\"Invoking {interfaceName}.{methodInfo.Name}()\");");
                            var paramString = string.Join(", ", methodInfo.GetParameters().Select(x => x.Name));
                            var invocationString = $"real.{methodInfo.Name}({paramString});";
                            if (methodInfo.ReturnType == typeof(void))
                                writer.WriteLine(invocationString);
                            else
                                writer.WriteLine($"return {invocationString}");
                        }
                        writer.WriteLine();
                    }
                }
            }
            writer.Builder.Insert(0, writer.GetUsingsString());
            var path = Path.Combine(type.Namespace.Replace(".", "\\"), className + ".cs");
            var content = writer.Builder.ToString();
            context.AddFile(new GeneratedFileInfo
            {
                Path = path,
                Content = content,
                TypesUsed = writer.GetUsedTypes(),
                Template = type,
                ImplementationFullName = $"{type.Namespace}.{className}"
            });
        }
    }
}
