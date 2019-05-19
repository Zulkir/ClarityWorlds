using System.Linq;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Collections;
using Clarity.Common.Numericals;
using Clarity.Engine.Objects.WorldTree;

namespace Clarity.Engine.Interaction.RayHittables
{
    public class RayHitIndex : IRayHitIndex
    {
        public RayHitResult FindEntity(RayHitInfo clickInfo)
        {
            if (clickInfo.Scene == null)
                return RayHitResult.Failure();
            
            var minimalOrDefault = clickInfo.Scene.Root.EnumerateSceneNodesDeep()
                .SelectMany(x => x.SearchComponents<IRayHittableComponent>())
                .Select(x => x.HitWithClick(clickInfo))
                .Where(x => x.Successful)
                .MinimalOrDefault(x => x.Distance - (x.Node.ParentNode?.ChildNodes.IndexOf(x.Node) ?? 0) * GraphicsHelper.MinZOffset);
            return minimalOrDefault.Successful 
                ? minimalOrDefault 
                : RayHitResult.Failure();
        }
    }
}