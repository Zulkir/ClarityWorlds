namespace Clarity.Engine.Interaction.RayHittables
{
    public interface IRayHitIndex
    {
        RayHitResult FindEntity(RayHitInfo clickInfo);
    }
}