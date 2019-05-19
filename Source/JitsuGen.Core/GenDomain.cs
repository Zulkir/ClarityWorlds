using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace JitsuGen.Core
{
    public class GenDomain : IGenDomain
    {
        private readonly ConcurrentDictionary<Type, Type> generatedTypes = new ConcurrentDictionary<Type, Type>();

        public void RegisterGeneratedType(Type template, Type generatedType)
        {
            generatedTypes.AddOrUpdate(template, generatedType, (t, o) => generatedType);
        }

        public Type GetGeneratedType(Type template)
        {
            if (!generatedTypes.TryGetValue(template, out var generatedType))
                throw new KeyNotFoundException($"No generated class registered for a template '{template.FullName}'.");
            return generatedType;
        }

        public static GenDomain Static { get; } = new GenDomain();
    }
}