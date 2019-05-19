using System.Collections.Generic;
using Clarity.Engine.Resources;

namespace Clarity.Engine.Media.Models.Flexible
{
    public class FlexibleModel : ResourceBase, IFlexibleModel
    {
        public IReadOnlyList<IFlexibleModelVertexSet> VertexSets { get; }
        public IReadOnlyList<IFlexibleModelPart> Parts { get; }
        public float Radius { get; }

        public FlexibleModel(ResourceVolatility volatility, IReadOnlyList<IFlexibleModelVertexSet> vertexSets, IReadOnlyList<IFlexibleModelPart> parts, float radius) 
            : base(volatility)
        {
            VertexSets = vertexSets;
            Parts = parts;
            Radius = radius;
        }

        public override void Dispose()
        {
            base.Dispose();
            foreach (var vertexSet in VertexSets)
                vertexSet.Dispose();
        }
    }
}