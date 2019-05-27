using Clarity.Common.Numericals.Algebra;
using Clarity.Engine.Objects.WorldTree;

namespace Clarity.Engine.Interaction.RayHittables
{
    public struct RayHitResult
    {
        public ISceneNode Node;
        public Vector3 GlobalHitPoint;
        public Vector3 LocalHitPoint;
        public int IntTag;
        public string StrTag;
        public float Distance;
        public bool Successful;

        public static RayHitResult Failure() => 
            new RayHitResult { Successful = false };
    }
}