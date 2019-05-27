using Clarity.Common.Infra.TreeReadWrite.Serialization;
using Clarity.Common.Numericals.Algebra;
using Clarity.Engine.Resources;
using Clarity.Engine.Visualization.Graphics;

namespace Clarity.Engine.Media.Models.Explicit.Embedded
{
    public class SimplePlaneXzModelFactory : ResourceFactoryBase
    {
        public override string UniqueName => "Core_SimplePlaneXzModelFactory";

        [TrwSerialize]
        private class Source : FactoryResourceSourceBase<Source, SimplePlaneXzModelFactory>
        {
            protected override int GetFieldsHashCode() => 0;
            protected override bool FieldsAreEual(Source other) => true;

            [TrwSerialize]
            public Source(IFactoryResourceCache factoryResourceCache, SimplePlaneXzModelFactory factory) : base(factoryResourceCache, factory)
            {
            }
        }

        public SimplePlaneXzModelFactory(IFactoryResourceCache factoryResourceCache) : base(factoryResourceCache)
        {
        }

        public override IResource GetNew(IResourceSource resourceSource)
        {
            var cSource = (Source)resourceSource;
            var vertices = new[]
            {
                new VertexPosTanNormTex(new Vector3(-1, 0,  1), new Vector3(1, 0, 0), new Vector3(0, 0, 1), new Vector2(0, 1)),
                new VertexPosTanNormTex(new Vector3(-1, 0, -1), new Vector3(1, 0, 0), new Vector3(0, 0, 1), new Vector2(0, 0)),
                new VertexPosTanNormTex(new Vector3( 1, 0, -1), new Vector3(1, 0, 0), new Vector3(0, 0, 1), new Vector2(1, 0)),
                new VertexPosTanNormTex(new Vector3( 1, 0,  1), new Vector3(1, 0, 0), new Vector3(0, 0, 1), new Vector2(1, 1)),
            };
            var indices = new int[]
            {
                0, 1, 2, 0, 2, 3
            };
            return ExplicitModel.FromVertices(vertices, indices, ExplicitModelPrimitiveTopology.TriangleList).WithSource(cSource);
        }

        public IResourceSource GetModelSource()
        {
            return new Source(FactoryResourceCache, this);
        }
    }
}