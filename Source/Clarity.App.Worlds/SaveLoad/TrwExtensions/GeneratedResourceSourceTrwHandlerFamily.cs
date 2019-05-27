using System;
using Clarity.Common.Infra.TreeReadWrite.Serialization;
using Clarity.Engine.Resources;

namespace Clarity.App.Worlds.SaveLoad.TrwExtensions 
{
    public class GeneratedResourceSourceTrwHandlerFamily : ITrwSerializationHandlerFamily
    {
        public bool TryCreateHandlerFor(Type type, ITrwSerializationHandlerContainer container, out ITrwSerializationHandler handler)
        {
            if (typeof(GeneratedResourceSource).IsAssignableFrom(type))
            {
                handler = new GeneratedResourceSourceTrwHandler();
                return true;
            }

            handler = null;
            return false;
        }
    }
}