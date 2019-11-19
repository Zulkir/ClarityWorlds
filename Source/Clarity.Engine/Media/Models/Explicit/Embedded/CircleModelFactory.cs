using System.Linq;
using Clarity.Common.CodingUtilities.Sugar.Extensions.Common;
using Clarity.Common.Infra.TreeReadWrite.Serialization;
using Clarity.Common.Numericals;
using Clarity.Engine.Resources;
using Clarity.Engine.Visualization.Graphics;

namespace Clarity.Engine.Media.Models.Explicit.Embedded
{
    public class CircleModelFactory : ResourceFactoryBase
    {
        public override string UniqueName => "Core_CircleModelFactory";
        
        [TrwSerialize]
        private class Source : FactoryResourceSourceBase<Source, CircleModelFactory>
        {
            [TrwSerialize]
            public int NumSegments { get; set; }

            [TrwSerialize]
            public Source(IFactoryResourceCache factoryResourceCache, CircleModelFactory factory) 
                : base(factoryResourceCache, factory)
            {
            }

            protected override int GetFieldsHashCode() => NumSegments;
            protected override bool FieldsAreEqual(Source other) => NumSegments == other.NumSegments;
        }

        public CircleModelFactory(IFactoryResourceCache factoryResourceCache) 
            : base(factoryResourceCache)
        {
        }

        public override IResource GetNew(IResourceSource resourceSource)
        {
            var cSource = (Source)resourceSource;
            var vertices = Enumerable.Range(0, cSource.NumSegments)
                .Select(i => (float)i / cSource.NumSegments * MathHelper.TwoPi)
                .Select(a => new VertexPos(MathHelper.Cos(a), MathHelper.Sin(a), 0f))
                .ToArray();
            var indices = Enumerable.Range(0, cSource.NumSegments).Concat(0.EnumSelf()).ToArray();
            return ExplicitModel.FromVertices(vertices, indices, ExplicitModelPrimitiveTopology.LineStrip).WithSource(cSource);
        }

        public IResourceSource GetModelSource(int numSegments)
        {
            return new Source(FactoryResourceCache, this) {NumSegments = numSegments};
        }
    }
}