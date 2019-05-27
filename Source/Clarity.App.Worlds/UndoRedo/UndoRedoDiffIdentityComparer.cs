using System.Collections.Generic;
using Clarity.Common.Infra.TreeReadWrite.DiffBuilding;
using Clarity.Common.Infra.TreeReadWrite.Serialization;
using Clarity.Engine.Objects.WorldTree;

namespace Clarity.App.Worlds.UndoRedo
{
    public class UndoRedoDiffIdentityComparer : ITrwDiffIdentityComparer
    {
        public bool AreSameObject(IDictionary<string, object> o1, IDictionary<string, object> o2)
        {
            const string typePropName = TrwSerializationDiffApplier.TypePropertyName;
            if (!o1.TryGetValue(typePropName, out var t1) || !o2.TryGetValue(typePropName, out var t2) || 
                !(t1 is string ts1) || !(t2 is string ts2) || ts1 != ts2)
                return false;
            if (ts1.Contains(typeof(SceneNode).Name))
                return (int)o1[nameof(ISceneNode.Id)] == (int)o2[nameof(ISceneNode.Id)];
            if (ts1.Contains(typeof(World).FullName))
                return true;
            if (ts1.Contains(typeof(Scene).FullName))
                return true;
            // todo: more
            return false;
        }
    }
}