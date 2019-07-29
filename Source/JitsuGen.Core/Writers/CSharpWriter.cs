using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace JitsuGen.Core.Writers
{
    public class CSharpWriter : CStyleWriter
    {
        private readonly Dictionary<string, string> usingStrings;
        private readonly HashSet<Type> usedTypes;

        public IEnumerable<Type> GetUsedTypes() => usedTypes;

        public CSharpWriter()
        {
            usingStrings = new Dictionary<string, string>();
            usedTypes= new HashSet<Type>();
        }

        public string UseType(Type type)
        {
            var name = type.Name;
            if (type.IsGenericType)
            {
                var backtick = name.IndexOf('`');
                if (backtick > 0)
                    name = name.Remove(backtick);
            }
            usedTypes.Add(type);
            var normalizedFullName = NormalizeFullName(type);
            return UseGeneratedType(normalizedFullName, name);
        }

        private string NormalizeFullName(Type type)
        {
            var fullName = type.FullName;
            if (type.IsGenericType)
            {
                var backtick = fullName.IndexOf('`');
                if (backtick > 0)
                    fullName = fullName.Remove(backtick);
                fullName += "<";
                if (type.IsGenericTypeDefinition)
                    fullName += new string(',', type.GetGenericArguments().Length - 1);
                else
                    fullName += string.Join(", ", type.GetGenericArguments().Select(NormalizeFullName));
                fullName += ">";
            }
            fullName = fullName.Replace('+', '.');
            return fullName;
        }

        public string UseGeneratedType(string fullName, string typeName)
        {
            if (fullName == "System.Void")
                return "void";
            var alreadyExists = usingStrings.TryGetValue(typeName, out var otherFullName);
            if (alreadyExists && otherFullName == fullName)
                return typeName;
            if (!alreadyExists)
            {
                usingStrings.Add(typeName, fullName);
                return typeName;
            }
            var counter = 2;
            while (usingStrings.ContainsKey(typeName + counter))
                counter++;
            var typeNameAlias = typeName + counter;
            usingStrings.Add(typeNameAlias, fullName);
            return typeNameAlias;
        }

        public string GetUsingsString()
        {
            var builder = new StringBuilder();
            foreach (var kvp in usingStrings)
            {
                var alias = kvp.Key;
                var fullName = kvp.Value;
                // todo: more robust array removal
                while (alias.EndsWith("[]") && fullName.EndsWith("[]"))
                {
                    alias = alias.Remove(alias.Length - 2);
                    fullName = fullName.Remove(fullName.Length - 2);
                }
                builder.AppendLine($"using {alias} = {fullName};");
            }
            return builder.ToString();
        }

        public void WriteMethodDeclaration(string modifiers, MethodInfo methodInfo)
        {
            WriteIndent();
            Write(modifiers);
            Write(" ");
            Write(UseType(methodInfo.ReturnType));
            Write(" ");
            Write(methodInfo.Name);
            if (methodInfo.ContainsGenericParameters)
            {
                Write("<");
                Write(string.Join(", ", methodInfo.GetGenericArguments().Select(x => x.Name)));
                Write(">");
            }
            Write("(");
            Write(string.Join(", ", methodInfo.GetParameters().Select(x => $"{UseType(x.ParameterType)} {x.Name}")));
            Write(")");
            WriteLine();
            // todo: write generic constrains
        }

        public void WriteProperty(string modifiers, string resolvedType, string name, 
            Action<CSharpWriter> writeGetter,
            Action<CSharpWriter> writeSetter)
        {
            WriteLine($"{modifiers} {resolvedType} {name}");
            using (Curly())
            {
                if (writeGetter != null)
                {
                    WriteLine("get");
                    using (Curly())
                        writeGetter(this);
                }
                if (writeSetter != null)
                {
                    WriteLine("set");
                    using (Curly())
                        writeSetter(this);
                }
            }
        }
    }
}