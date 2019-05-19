using System;

namespace Clarity.Common.Infra.TreeReadWrite.Serialization
{
    public interface ITrwSerializationTypeRedirect
    {
        bool TryRedirect(Type realType, out Type typeToSaveLoad);
    }
}