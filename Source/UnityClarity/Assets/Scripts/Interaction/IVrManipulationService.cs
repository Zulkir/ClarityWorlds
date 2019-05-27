using System.Collections.Generic;
using Clarity.Engine.Objects.WorldTree;

namespace Assets.Scripts.Interaction
{
    public interface IVrManipulationService
    {
        IEnumerable<ISceneNode> GetGrabbedObjects();
    }
}