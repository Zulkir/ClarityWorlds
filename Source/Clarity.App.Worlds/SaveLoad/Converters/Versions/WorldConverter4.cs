﻿using System.Dynamic;

namespace Clarity.App.Worlds.SaveLoad.Converters.Versions
{
    public class WorldConverter4 : SaveLoadConverterExpandoBase, ISaveLoadVersionConverter
    {
        protected override ExpandoObject Run(ExpandoObject input)
        {
            dynamic world = input;
            return world;
        }
    }
}