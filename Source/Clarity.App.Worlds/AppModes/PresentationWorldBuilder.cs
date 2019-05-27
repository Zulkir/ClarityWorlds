using Clarity.App.Worlds.WorldTree;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Common;
using Clarity.Engine.Objects.WorldTree;

namespace Clarity.App.Worlds.AppModes
{
    public class PresentationWorldBuilder : IPresentationWorldBuilder
    {
        public IScene BuildPreesentationWorld(IScene editingWorld)
        {
            return editingWorld.CloneTyped();
        }

        public IWorld BuildPreesentationWorld(IWorld editingWorld)
        {
            var clone = editingWorld.CloneTyped();
            clone.Tags.Remove(WorldConstants.EditingWorldTag);
            clone.Tags.Add(WorldConstants.PresentationWorldTag);
            return clone;
        }
    }
}