using System.Collections.Generic;
using Clarity.App.Worlds.Assets;
using Clarity.Engine.EventRouting;
using Clarity.Engine.Objects.WorldTree;

namespace Clarity.App.Worlds.SaveLoad
{
    public interface ISaveLoadEvent : IRoutedEvent
    {
        SaveLoadEventType Type { get; }
        IWorld World { get; }
        IReadOnlyDictionary<string, IAsset> Assets { get; }
    }
}