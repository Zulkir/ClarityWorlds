using Clarity.Common.Infra.TreeReadWrite.Serialization;
using Clarity.Common.Numericals;
using Clarity.Engine.Resources;
using Clarity.Engine.Visualization.Graphics;

namespace Clarity.Engine.Media.Models.Explicit.Embedded
{
    public class SimpleFrustumModelFactory : ResourceFactoryBase
    {
        public const string FactoryName = "Core_SimpleFrustumModelFactory";
        public override string UniqueName => FactoryName;
        
        [TrwSerialize]
        private class Source : FactoryResourceSourceBase<Source, SimpleFrustumModelFactory>
        {
            protected override int GetFieldsHashCode() => 0;
            protected override bool FieldsAreEqual(Source other) => true;

            [TrwSerialize]
            public Source(IFactoryResourceCache factoryResourceCache, SimpleFrustumModelFactory factory) : base(factoryResourceCache, factory)
            {
            }
        }

        public SimpleFrustumModelFactory(IFactoryResourceCache factoryResourceCache) : base(factoryResourceCache)
        {
        }

        public override IResource GetNew(IResourceSource resourceSource)
        {
            var cSource = (Source)resourceSource;
            var distance = 1f / MathHelper.Tan(MathHelper.Pi / 8);
            var vertices = new[]
            {
                new VertexPos(-1, -1, 0),
                new VertexPos(-1, 1, 0),
                new VertexPos(1, 1, 0),
                new VertexPos(1, -1, 0),
                new VertexPos(-2, -2, -distance),
                new VertexPos(-2, 2, -distance),
                new VertexPos(2, 2, -distance),
                new VertexPos(2, -2, -distance)
            };
            var indices = new[]
            {
                0, 1, 1, 2, 2, 3, 3, 0,
                4, 5, 5, 6, 6, 7, 7, 4,
                0, 4, 1, 5, 2, 6, 3, 7
            };
            return ExplicitModel.FromVertices(vertices, indices, ExplicitModelPrimitiveTopology.LineList).WithSource(cSource);
        }

        public IResourceSource GetModelSource()
        {
            return new Source(FactoryResourceCache, this);
        }
    }
}