using System.Collections.Generic;
using Clarity.Common.Numericals.Geometry;
using Clarity.Engine.Resources;

namespace Clarity.Engine.Media.Models.Flexible
{
    public class FlexibleModel : ResourceBase, IFlexibleModel
    {
        public IReadOnlyList<IFlexibleModelVertexSet> VertexSets { get; }
        public IReadOnlyList<IFlexibleModelPart> Parts { get; }
        public Sphere BoundingSphere { get; }
        public int PartCount => Parts.Count;

        public FlexibleModel(ResourceVolatility volatility, IReadOnlyList<IFlexibleModelVertexSet> vertexSets, IReadOnlyList<IFlexibleModelPart> parts, Sphere boundingSphere) 
            : base(volatility)
        {
            VertexSets = vertexSets;
            Parts = parts;
            BoundingSphere = boundingSphere;
        }

        public override void Dispose()
        {
            base.Dispose();
            foreach (var vertexSet in VertexSets)
                vertexSet.Dispose();
        }
    }
}