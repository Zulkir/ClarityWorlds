using System.Linq;
using Clarity.App.Worlds.Logging;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Common;
using Clarity.Engine.Objects.WorldTree;

namespace Clarity.App.Worlds.SaveLoad.ReadOnly 
{
    public class ReadOnlyWorldBuilder : IReadOnlyWorldBuilder 
    {
        public IWorld BuildReadOnly(IWorld originalWorld)
        {
            var readOnlyWorld = originalWorld.CloneTyped();
            SaveLoadWorldProperties.Get(readOnlyWorld).IsReadOnly = true;
            foreach (var node in readOnlyWorld.Scenes.SelectMany(x => x.EnumerateAllNodes(false)))
                AdjustNode(node);
            return readOnlyWorld;
        }

        private void AdjustNode(ISceneNode node)
        {
            for (var i = node.Components.Count - 1; i >= 0; i--)
            {
                var component = node.Components[i];
                if (component is IReadOnlyOverrideComponent cReadOnlyOverride)
                {
                    node.Components.RemoveAt(i);
                    var j = 0;
                    foreach (var overridenComponent in cReadOnlyOverride.ToReadOnlyComponents())
                    {
                        node.Components.Insert(i + j, overridenComponent);
                        j++;
                    }
                }
                else if (component.AmInterface.Assembly != GetType().Assembly)
                    Log.Write(LogMessageType.Warning, $"Saving a type '{component.AmInterface.FullName}' from a non-core assembly '{component.AmInterface.Assembly.FullName}' may cause incompatibility.");
            }
        }
    }
}