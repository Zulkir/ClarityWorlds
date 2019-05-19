using System;
using Clarity.Common.Infra.ActiveModel;
using Clarity.Common.Infra.TreeReadWrite.Serialization;

namespace Clarity.Core.AppCore.SaveLoad
{
    public class AmTrwTypeRedirect : ITrwSerializationTypeRedirect
    {
        public bool TryRedirect(Type realType, out Type typeToSaveLoad)
        {
            if (typeof(IAmObject).IsAssignableFrom(realType) && !realType.IsAbstract && !realType.IsInterface)
            {
                typeToSaveLoad = realType.BaseType;
                return true;
            }
            typeToSaveLoad = null;
            return false;
        }
    }
}