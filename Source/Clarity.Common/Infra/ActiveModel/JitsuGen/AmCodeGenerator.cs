using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Clarity.Common.Infra.ActiveModel.ClassEmitting;
using JitsuGen.Core;
using JitsuGen.Core.Writers;

namespace Clarity.Common.Infra.ActiveModel.JitsuGen
{
    public class AmCodeGenerator : IFileGenerator
    {
        public string FileType => ".cs";

        private readonly IReadOnlyList<IAmBindingTypeDescriptor> bindingTypeDescriptors;

        public AmCodeGenerator()
        {
            // todo: actually load it from somewhere
            bindingTypeDescriptors = new IAmBindingTypeDescriptor[]
            {
                new AmSingularBindingTypeDescriptor(),
                new AmListBindingTypeDescriptor()
            };
        }

        public void GenerateFor(IFileGeneratingContext context, Type type)
        {
            var writer = new CSharpWriter();
            var amClass = type;
            var amClassName = writer.UseType(type);
            var desc = new AmObjectTypeDescription(amClass, bindingTypeDescriptors);

            var className = "_am_" + desc.Name;
            
            writer.WriteLine();
            writer.WriteLine($"namespace JitsuGen.Output.{type.Namespace}");
            using (writer.Curly())
            {
                
                writer.WriteLine($"public class {className} : {amClassName}");
                using (writer.Curly())
                {
                    foreach (var bindingDesc in desc.Bindings)
                        writer.WriteLine( $"private {writer.UseType(bindingDesc.MakeBindingType(amClass))} _{bindingDesc.Property.Name};");
                    writer.WriteLine();

                    foreach (var bindingDesc in desc.Bindings)
                    {
                        var getterString = bindingDesc.BuildGetterString($"_{bindingDesc.Property.Name}");
                        var setterString = bindingDesc.BuildSetterString($"_{bindingDesc.Property.Name}");
                        writer.WriteProperty("public override", writer.UseType(bindingDesc.Property.PropertyType), bindingDesc.Property.Name,
                            getterString != null ? (Action<CSharpWriter>)(w => w.WriteLine(getterString)) : null,
                            setterString != null ? (Action<CSharpWriter>)(w => w.WriteLine(setterString)) : null);
                        writer.WriteLine();
                    }

                    var baseCtor = amClass.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance | BindingFlags.Instance).Single();
                    var ctorParametersStr = string.Join(", ", baseCtor.GetParameters().Select(x => $"{writer.UseType(x.ParameterType)} {x.Name}"));
                    var ctorArgsStr = string.Join(", ", baseCtor.GetParameters().Select(x => x.Name));
                    writer.WriteLine($"public {className}({ctorParametersStr})");
                    writer.WriteLine($": base({ctorArgsStr})");
                    using (writer.Curly())
                    { }
                    writer.WriteLine();

                    writer.WriteLine($"protected override {writer.UseType(typeof(List<IAmBinding>))} AmInitBindings()");
                    using (writer.Curly())
                    {
                        writer.WriteLine($"var list = new {writer.UseType(typeof(List<IAmBinding>))}();");
                        foreach (var bindingDesc in desc.Bindings)
                        {
                            var flagTypeStr = writer.UseType(typeof(AmBindingFlags));
                            var flagsStr = string.Join(" | ", EnumerateFlags(bindingDesc.Flags).Select(x => $"{flagTypeStr}.{x}"));
                            writer.WriteLine($"_{bindingDesc.Property.Name} = new {writer.UseType(bindingDesc.MakeBindingType(amClass))}(this, \"{bindingDesc.Property.Name}\", {flagsStr});");
                            writer.WriteLine($"list.Add(_{bindingDesc.Property.Name});");
                        }
                        writer.WriteLine("return list;");
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

        private static IEnumerable<string> EnumerateFlags(AmBindingFlags flags)
        {
            if (flags == AmBindingFlags.None)
            {
                yield return AmBindingFlags.None.ToString();
                yield break;
            }
            if (flags.HasFlag(AmBindingFlags.Reference))
                yield return AmBindingFlags.Reference.ToString();
            if (flags.HasFlag(AmBindingFlags.Derived))
                yield return AmBindingFlags.Derived.ToString();
        }
    }
}