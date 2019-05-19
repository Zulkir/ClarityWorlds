using System;
using Clarity.Common.Infra.TreeReadWrite.Serialization;

namespace Clarity.Core.AppCore.SaveLoad
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