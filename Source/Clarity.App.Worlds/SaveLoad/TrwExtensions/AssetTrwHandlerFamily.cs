using System;
using Clarity.App.Worlds.Assets;
using Clarity.Common.Infra.TreeReadWrite.Serialization;

namespace Clarity.App.Worlds.SaveLoad.TrwExtensions
{
    public class AssetTrwHandlerFamily : ITrwSerializationHandlerFamily
    {
        public bool TryCreateHandlerFor(Type type, ITrwSerializationHandlerContainer container, out ITrwSerializationHandler handler)
        {
            if (typeof(IAsset).IsAssignableFrom(type))
            {
                handler = new AssetTrwHandler();
                return true;
            }
            handler = null;
            return false;
        }
    }
}