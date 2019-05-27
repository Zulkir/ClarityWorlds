using System.Collections.Generic;
using Clarity.Common.Numericals.Algebra;
using Clarity.Common.Numericals.Geometry;

namespace Clarity.App.Worlds.StoryGraph.FreeNavigation
{
    public interface ICollisionMesh
    {
        Vector3 RestrictMovement(float radius, Vector3 startingPoint, Vector3 offset);
        IReadOnlyList<AaBox> GetWalkableAreas();
        IStoryLayoutZoning Zoning { get; }
        // todo: refactor this monstrosity
        float ZeroGravityTeleportHeight { get; }
    }
}