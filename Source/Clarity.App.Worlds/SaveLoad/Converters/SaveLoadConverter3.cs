﻿using Clarity.App.Worlds.SaveLoad.Converters.Data;
using Clarity.App.Worlds.SaveLoad.Converters.Tasks;

namespace Clarity.App.Worlds.SaveLoad.Converters
{
    public class SaveLoadConverter3 : SaveLoadVersionConverterBase
    {
        public SaveLoadConverter3()
        {
            AddTask(new TypeRenameConverter(TypeRenames3.Renames));
            AddTask(new PropertyRenameConverter(PropertyRenames3.Renames));
        }
    }
}