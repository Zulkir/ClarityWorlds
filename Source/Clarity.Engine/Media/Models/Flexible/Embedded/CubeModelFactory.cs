using Clarity.Common.Infra.TreeReadWrite.Serialization;
using Clarity.Engine.Resources;
using Clarity.Engine.Visualization.Graphics;

namespace Clarity.Engine.Media.Models.Flexible.Embedded
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

        public override IResource GetNew(IResourceSource resourceSource)
        {
            var vertices = new[]
            {
                new CgVertexPosNormTex(1f, -1f, 1f, 1f, 0f, 0f, 0f, 0f),
                new CgVertexPosNormTex(1f, 1f, 1f, 1f, 0f, 0f, 1f, 0f),
                new CgVertexPosNormTex(1f, 1f, -1f, 1f, 0f, 0f, 1f, 1f),
                new CgVertexPosNormTex(1f, -1f, -1f, 1f, 0f, 0f, 0f, 1f),

                new CgVertexPosNormTex(1f, 1f, 1f, 0f, 1f, 0f, 0f, 0f),
                new CgVertexPosNormTex(-1f, 1f, 1f, 0f, 1f, 0f, 1f, 0f),
                new CgVertexPosNormTex(-1f, 1f, -1f, 0f, 1f, 0f, 1f, 1f),
                new CgVertexPosNormTex(1f, 1f, -1f, 0f, 1f, 0f, 0f, 1f),

                new CgVertexPosNormTex(-1f, 1f, 1f, -1f, 0f, 0f, 0f, 0f),
                new CgVertexPosNormTex(-1f, -1f, 1f, -1f, 0f, 0f, 1f, 0f),
                new CgVertexPosNormTex(-1f, -1f, -1f, -1f, 0f, 0f, 1f, 1f),
                new CgVertexPosNormTex(-1f, 1f, -1f, -1f, 0f, 0f, 0f, 1f),

                new CgVertexPosNormTex(-1f, -1f, 1f, 0f, -1f, 0f, 0f, 0f),
                new CgVertexPosNormTex(1f, -1f, 1f, 0f, -1f, 0f, 1f, 0f),
                new CgVertexPosNormTex(1f, -1f, -1f, 0f, -1f, 0f, 1f, 1f),
                new CgVertexPosNormTex(-1f, -1f, -1f, 0f, -1f, 0f, 0f, 1f),

                new CgVertexPosNormTex(-1f, -1f, 1f, 0f, 0f, 1f, 0f, 0f),
                new CgVertexPosNormTex(-1f, 1f, 1f, 0f, 0f, 1f, 1f, 0f),
                new CgVertexPosNormTex(1f, 1f, 1f, 0f, 0f, 1f, 1f, 1f),
                new CgVertexPosNormTex(1f, -1f, 1f, 0f, 0f, 1f, 0f, 1f),

                new CgVertexPosNormTex(-1f, 1f, -1f, 0f, 0f, -1f, 0f, 0f),
                new CgVertexPosNormTex(-1f, -1f, -1f, 0f, 0f, -1f, 1f, 0f),
                new CgVertexPosNormTex(1f, -1f, -1f, 0f, 0f, -1f, 1f, 1f),
                new CgVertexPosNormTex(1f, 1f, -1f, 0f, 0f, -1f, 0f, 1f)
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

            return FlexibleModelHelpers.CreateSimple(resourceSource, vertices, indices, FlexibleModelPrimitiveTopology.TriangleList);
        }

        public IResourceSource GetModelSource()
        {
            return new Source(FactoryResourceCache, this);
        }
    }
}