using System;
using Clarity.Common.Infra.TreeReadWrite.Serialization;
using Clarity.Core.AppCore.ResourceTree.Assets;

namespace Clarity.Core.AppCore.SaveLoad
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