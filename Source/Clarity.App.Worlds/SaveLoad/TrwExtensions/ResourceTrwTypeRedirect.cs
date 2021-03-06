﻿using System;
using Clarity.Common.Infra.TreeReadWrite.Serialization;
using Clarity.Engine.Resources;

namespace Clarity.App.Worlds.SaveLoad.TrwExtensions
{
    public class ResourceTrwTypeRedirect : ITrwSerializationTypeRedirect
    {
        public bool TryRedirect(Type realType, out Type typeToSaveLoad)
        {
            if (!typeof(IResource).IsAssignableFrom(realType))
            {
                typeToSaveLoad = null;
                return false;
            }
            typeToSaveLoad = typeof(IResource);
            return true;
        }
    }
}