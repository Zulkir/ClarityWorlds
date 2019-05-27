using System;

namespace Clarity.Common.Infra.TreeReadWrite.Serialization.TypeRedirects
{
    public class RuntimeTypeTrwTypeRedirect : ITrwSerializationTypeRedirect
    {
        public bool TryRedirect(Type realType, out Type typeToSaveLoad)
        {
            // ReSharper disable once PossibleMistakenCallToGetType.2
            if (realType == typeof(Type).GetType())
            {
                typeToSaveLoad = typeof(Type);
                return true;
            }
            typeToSaveLoad = null;
            return false;
        }
    }
}