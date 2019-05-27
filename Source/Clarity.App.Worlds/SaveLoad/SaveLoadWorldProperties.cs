﻿using Clarity.Common.Infra.TreeReadWrite.Serialization;
using Clarity.Engine.Objects.WorldTree;

namespace Clarity.App.Worlds.SaveLoad
{
    [TrwSerialize]
    public class SaveLoadWorldProperties
    {
        public static string Key { get; } = "SaveLoad";

        [TrwSerialize]
        public bool IsReadOnly { get; set; }

        public static SaveLoadWorldProperties Get(IWorld world) => 
            world.Properties.GetOrAdd<SaveLoadWorldProperties>(Key);
    }
}