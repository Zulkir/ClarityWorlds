namespace Clarity.Engine.Interaction.RayHittables
{
    public interface IRayHittable
    {
        RayHitResult HitWithClick(RayCastInfo clickInfo);
    }
}