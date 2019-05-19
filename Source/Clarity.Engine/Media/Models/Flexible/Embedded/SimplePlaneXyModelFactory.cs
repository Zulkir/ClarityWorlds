using Clarity.Common.Infra.TreeReadWrite.Serialization;
using Clarity.Engine.Resources;
using Clarity.Engine.Visualization.Graphics;

namespace Clarity.Engine.Media.Models.Flexible.Embedded
{
    public class SimplePlaneXyModelFactory : ResourceFactoryBase
    {
        public override string UniqueName => "Core_SimplePlaneXyModelFactory";
        
        [TrwSerialize]
        private class Source : FactoryResourceSourceBase<Source, SimplePlaneXyModelFactory>
        {
            protected override int GetFieldsHashCode() => 0;
            protected override bool FieldsAreEual(Source other) => true;

            [TrwSerialize]
            public Source(IFactoryResourceCache factoryResourceCache, SimplePlaneXyModelFactory factory) : base(factoryResourceCache, factory)
            {
            }
        }

        public SimplePlaneXyModelFactory(IFactoryResourceCache factoryResourceCache) : base(factoryResourceCache)
        {
        }

        public override IResource GetNew(IResourceSource resourceSource)
        {
            var cSource = (Source)resourceSource;
            var vertices = new[]
            {
                new CgVertexPosNormTex(-1f, -1f, 0f, 0f, 0f, 1f, 0f, 1f),
                new CgVertexPosNormTex(-1f, 1f, 0f, 0f, 0f, 1f, 0f, 0f),
                new CgVertexPosNormTex(1f, 1f, 0f, 0f, 0f, 1f, 1f, 0f),
                new CgVertexPosNormTex(1f, -1f, 0f, 0f, 0f, 1f, 1f, 1f),
            };
            var indices = new int[]
            {
                0, 1, 2, 0, 2, 3
            };
            return FlexibleModelHelpers.CreateSimple(cSource, vertices, indices, FlexibleModelPrimitiveTopology.TriangleList);
        }

        public IResourceSource GetModelSource()
        {
            return new Source(FactoryResourceCache, this);
        }
    }
}