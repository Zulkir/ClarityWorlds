using System.Collections.Generic;

namespace Clarity.Engine.Interaction.RayHittables
{
    public interface IRayHitIndex
    {
        IEnumerable<RayHitResult> CastRay(RayCastInfo clickInfo);
    }
}