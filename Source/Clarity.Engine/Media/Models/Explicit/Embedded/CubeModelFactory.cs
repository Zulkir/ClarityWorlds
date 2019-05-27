using Clarity.Common.Infra.TreeReadWrite.Serialization;
using Clarity.Common.Numericals.Algebra;
using Clarity.Engine.Resources;
using Clarity.Engine.Visualization.Graphics;

namespace Clarity.Engine.Media.Models.Explicit.Embedded
{
    public class CubeModelFactory : ResourceFactoryBase
    {
        public override string UniqueName => "Core_CubeModelFactory";
        
        [TrwSerialize]
        private class Source : FactoryResourceSourceBase<Source, CubeModelFactory>
        {
            protected override int GetFieldsHashCode() => 0;
            protected override bool FieldsAreEual(Source other) => true;

            [TrwSerialize]
            public Source(IFactoryResourceCache factoryResourceCache, CubeModelFactory factory) : base(factoryResourceCache, factory)
            {
            }
        }

        public CubeModelFactory(IFactoryResourceCache factoryResourceCache) : base(factoryResourceCache)
        {
        }

        public static IModel3D GenerateNew()
        {
            var vertices = new[]
            {
                new VertexPosTanNormTex(new Vector3( 1, -1,  1), new Vector3(0, 0, -1), new Vector3(1, 0, 0), new Vector2(0, 1)),
                new VertexPosTanNormTex(new Vector3( 1,  1,  1), new Vector3(0, 0, -1), new Vector3(1, 0, 0), new Vector2(0, 0)),
                new VertexPosTanNormTex(new Vector3( 1,  1, -1), new Vector3(0, 0, -1), new Vector3(1, 0, 0), new Vector2(1, 0)),
                new VertexPosTanNormTex(new Vector3( 1, -1, -1), new Vector3(0, 0, -1), new Vector3(1, 0, 0), new Vector2(1, 1)),

                new VertexPosTanNormTex(new Vector3(-1,  1,  1), new Vector3(1, 0, 0), new Vector3(0, 1, 0),  new Vector2(0, 1)),
                new VertexPosTanNormTex(new Vector3(-1,  1, -1), new Vector3(1, 0, 0), new Vector3(0, 1, 0),  new Vector2(0, 0)),
                new VertexPosTanNormTex(new Vector3( 1,  1, -1), new Vector3(1, 0, 0), new Vector3(0, 1, 0),  new Vector2(1, 0)),
                new VertexPosTanNormTex(new Vector3( 1,  1,  1), new Vector3(1, 0, 0), new Vector3(0, 1, 0),  new Vector2(1, 1)),

                new VertexPosTanNormTex(new Vector3(-1, -1, -1), new Vector3(0, 0, 1), new Vector3(-1, 0, 0), new Vector2(0, 1)),
                new VertexPosTanNormTex(new Vector3(-1,  1, -1), new Vector3(0, 0, 1), new Vector3(-1, 0, 0), new Vector2(0, 0)),
                new VertexPosTanNormTex(new Vector3(-1,  1,  1), new Vector3(0, 0, 1), new Vector3(-1, 0, 0), new Vector2(1, 0)),
                new VertexPosTanNormTex(new Vector3(-1, -1,  1), new Vector3(0, 0, 1), new Vector3(-1, 0, 0), new Vector2(1, 1)),

                new VertexPosTanNormTex(new Vector3( 1, -1,  1), new Vector3(-1, 0, 0), new Vector3(0, -1, 0), new Vector2(0, 1)),
                new VertexPosTanNormTex(new Vector3( 1, -1, -1), new Vector3(-1, 0, 0), new Vector3(0, -1, 0), new Vector2(0, 0)),
                new VertexPosTanNormTex(new Vector3(-1, -1, -1), new Vector3(-1, 0, 0), new Vector3(0, -1, 0), new Vector2(1, 0)),
                new VertexPosTanNormTex(new Vector3(-1, -1,  1), new Vector3(-1, 0, 0), new Vector3(0, -1, 0), new Vector2(1, 1)),

                new VertexPosTanNormTex(new Vector3(-1, -1,  1), new Vector3(1, 0, 0), new Vector3(0, 0, 1),  new Vector2(0, 1)),
                new VertexPosTanNormTex(new Vector3(-1,  1,  1), new Vector3(1, 0, 0), new Vector3(0, 0, 1),  new Vector2(0, 0)),
                new VertexPosTanNormTex(new Vector3( 1,  1,  1), new Vector3(1, 0, 0), new Vector3(0, 0, 1),  new Vector2(1, 0)),
                new VertexPosTanNormTex(new Vector3( 1, -1,  1), new Vector3(1, 0, 0), new Vector3(0, 0, 1),  new Vector2(1, 1)),

                new VertexPosTanNormTex(new Vector3( 1, -1, -1), new Vector3(-1, 0, 0), new Vector3(0, 0, -1), new Vector2(0, 1)),
                new VertexPosTanNormTex(new Vector3( 1,  1, -1), new Vector3(-1, 0, 0), new Vector3(0, 0, -1), new Vector2(0, 0)),
                new VertexPosTanNormTex(new Vector3(-1,  1, -1), new Vector3(-1, 0, 0), new Vector3(0, 0, -1), new Vector2(1, 0)),
                new VertexPosTanNormTex(new Vector3(-1, -1, -1), new Vector3(-1, 0, 0), new Vector3(0, 0, -1), new Vector2(1, 1))
            };

            var indices = new int[]
            {
                0, 1, 2, 0, 2, 3,
                4, 5, 6, 4, 6, 7,
                8, 9, 10, 8, 10, 11,
                12, 13, 14, 12, 14, 15,
                16, 17, 18, 16, 18, 19,
                20, 21, 22, 20, 22, 23
            };
            return ExplicitModel.FromVertices(vertices, indices, ExplicitModelPrimitiveTopology.TriangleList);
        }

        public override IResource GetNew(IResourceSource resourceSource)
        {
            return GenerateNew().WithSource(resourceSource);
        }

        public IResourceSource GetModelSource()
        {
            return new Source(FactoryResourceCache, this);
        }
    }
}