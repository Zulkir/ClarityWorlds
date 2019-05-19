using System;
using Clarity.Common.Infra.TreeReadWrite.Serialization;
using Clarity.Core.AppCore.ResourceTree.Assets;

namespace Clarity.Core.AppCore.SaveLoad
{
    public class AssetForWorldTrwTypeRedirect : ITrwSerializationTypeRedirect
    {
        public bool TryRedirect(Type realType, out Type typeToSaveLoad)
        {
            if (typeof(IAsset).IsAssignableFrom(realType))
            {
                typeToSaveLoad = typeof(IAsset);
                return true;
            }
            typeToSaveLoad = realType;
            return false;
        }
    }
}