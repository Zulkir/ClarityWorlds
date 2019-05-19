using System;

namespace JitsuGen.Core
{
    public class JitsuGenAttribute : Attribute
    {
        public Type GeneratorType { get; }

        public JitsuGenAttribute(Type generatorType)
        {
            GeneratorType = generatorType;
        }
    }
}