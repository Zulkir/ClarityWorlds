﻿using Clarity.App.Worlds.SaveLoad.Converters.Data;
using Clarity.App.Worlds.SaveLoad.Converters.Tasks;

namespace Clarity.App.Worlds.SaveLoad.Converters
{
    public class SaveLoadConverter2 : SaveLoadVersionConverterBase
    {
        public SaveLoadConverter2() 
        {
            AddTask(new TypeRenameConverter(TypeRenames2.Renames));
        }
    }
}