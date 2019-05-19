using System;

namespace JitsuGen.Core
{
    public interface IGenDomain
    {
        void RegisterGeneratedType(Type template, Type generatedType);
        Type GetGeneratedType(Type template);
    }
}