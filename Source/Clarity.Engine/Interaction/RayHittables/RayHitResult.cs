using Clarity.Common.Numericals.Algebra;
using Clarity.Engine.Objects.WorldTree;

namespace Clarity.Engine.Interaction.RayHittables
{
    // todo: local hit point
    public struct RayHitResult
    {
        public ISceneNode Node;
        public Vector3 HitPoint;
        public float Distance;
        public bool Successful;

        public static RayHitResult Success(ISceneNode node, Vector3 hitPoint, float distnace) => 
            new RayHitResult { Node = node, HitPoint = hitPoint, Distance = distnace, Successful = true };

        public static RayHitResult Failure() => 
            new RayHitResult { Node = null, HitPoint = default(Vector3), Distance = default(float), Successful = false };
    }
}