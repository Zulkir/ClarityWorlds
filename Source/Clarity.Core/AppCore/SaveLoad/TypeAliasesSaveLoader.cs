using System;
using System.Collections.Generic;
using Clarity.Common.Infra.TreeReadWrite;

namespace Clarity.Core.AppCore.SaveLoad
{
    public class TypeAliasesSaveLoader : ITypeAliasesSaveLoader
    {
        public void Save(IReadOnlyDictionary<Type, string> aliases, ITrwWriter writer)
        {
            writer.StartObject();
            foreach (var aliasEntry in aliases)
            {
                writer.AddProperty(aliasEntry.Value);
                writer.WriteValue().String(aliasEntry.Key.AssemblyQualifiedName);
            }
            writer.EndObject();
        }

        public IReadOnlyDictionary<string, Type> Load(ITrwReader reader)
        {
            var dictionary = new Dictionary<string, Type>();
            reader.MoveNext();

            while (reader.MoveNext())
            {
                if (reader.TokenType == TrwTokenType.EndObject)
                    break;
                var name = reader.ValueAsString;
                reader.MoveNext();
                var assemblyQualfiedName = reader.ValueAsString;

                var type = Type.GetType(assemblyQualfiedName);
                dictionary.Add(name, type);
            }
            return dictionary;
        }
    }
}