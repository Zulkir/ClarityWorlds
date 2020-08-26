using System.Collections.Generic;
using Clarity.Common.Numericals.Algebra;
using Clarity.Engine.Resources;

namespace Clarity.App.Worlds.Hacks.SpherePackingLoad
{
    public class SpherePackingResult : ResourceBase
    {
        public IReadOnlyList<Vector3> Points { get; }

        public SpherePackingResult(IReadOnlyList<Vector3> points) : base(ResourceVolatility.Immutable)
        {
            Points = points;
        }
    }
}