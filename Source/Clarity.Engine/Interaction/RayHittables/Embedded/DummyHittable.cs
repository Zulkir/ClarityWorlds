namespace Clarity.Engine.Interaction.RayHittables.Embedded
{
    public class DummyHittable : IRayHittable
    {
        public RayHitResult HitWithClick(RayHitInfo clickInfo) =>
            RayHitResult.Failure();
    }
}