using System;
using System.Collections.Generic;
using Clarity.Common.Infra.TreeReadWrite;

namespace Clarity.Core.AppCore.SaveLoad
{
    public interface ITypeAliasesSaveLoader
    {
        void Save(IReadOnlyDictionary<Type, string> aliases, ITrwWriter writer);
        IReadOnlyDictionary<string, Type> Load(ITrwReader reader);
    }
}