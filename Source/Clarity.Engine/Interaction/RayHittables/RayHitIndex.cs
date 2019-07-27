using System.Collections.Generic;
using System.Linq;
using Clarity.Engine.Objects.WorldTree;

namespace Clarity.Engine.Interaction.RayHittables
{
    public class RayHitIndex : IRayHitIndex
    {
        public IEnumerable<RayHitResult> CastRay(RayCastInfo clickInfo)
        {
            return clickInfo.Scene == null
                ? Enumerable.Empty<RayHitResult>()
                : clickInfo.Scene.EnumerateAllNodes()
                    .SelectMany(x => x.SearchComponents<IRayHittableComponent>())
                    .Select(x => x.HitWithClick(clickInfo))
                    .Where(x => x.Successful)
                    .OrderBy(x => x.Distance);
        }
    }
}