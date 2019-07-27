using Clarity.Engine.Objects.WorldTree;

namespace Clarity.Engine.Interaction.RayHittables
{
    public interface IRayHittableComponent : ISceneNodeComponent
    {
        RayHitResult HitWithClick(RayCastInfo clickInfo);
    }
}