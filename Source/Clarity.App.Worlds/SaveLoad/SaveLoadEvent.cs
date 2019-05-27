using System.Collections.Generic;
using Clarity.App.Worlds.Assets;
using Clarity.Engine.EventRouting;
using Clarity.Engine.Objects.WorldTree;

namespace Clarity.App.Worlds.SaveLoad 
{
    public class SaveLoadEvent : RoutedEventBase, ISaveLoadEvent 
    {
        public SaveLoadEventType Type { get; }
        public IWorld World { get; }
        public IReadOnlyDictionary<string, IAsset> Assets { get; }

        public SaveLoadEvent(SaveLoadEventType type, IWorld world, IReadOnlyDictionary<string, IAsset> assets)
        {
            Type = type;
            World = world;
            Assets = assets;
        }
    }
}