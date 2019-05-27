using System;
using Clarity.App.Worlds.Assets;
using Clarity.Common.Infra.TreeReadWrite.Serialization;

namespace Clarity.App.Worlds.SaveLoad.TrwExtensions
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