using Clarity.Common.Infra.TreeReadWrite.Serialization;
using Clarity.Engine.Resources;
using Clarity.Engine.Visualization.Graphics;

namespace Clarity.Engine.Media.Models.Flexible.Embedded
{
    public class Rect3DModelFactory : ResourceFactoryBase
    {
        public override string UniqueName => "Core_Rect3DModelFactory";
        
        [TrwSerialize]
        private class Source : FactoryResourceSourceBase<Source, Rect3DModelFactory>
        {
            protected override int GetFieldsHashCode() => 0;
            protected override bool FieldsAreEual(Source other) => true;

            [TrwSerialize]
            public Source(IFactoryResourceCache factoryResourceCache, Rect3DModelFactory factory) : base(factoryResourceCache, factory)
            {
            }
        }

        public Rect3DModelFactory(IFactoryResourceCache factoryResourceCache) : base(factoryResourceCache)
        {
        }

        public override IResource GetNew(IResourceSource resourceSource)
        {
            var cSource = (Source)resourceSource;
            var vertices = new[]
            {
                new CgVertexPosNormTex(-3, -1, 0, 0, 1, 0, 0, 1),
                new CgVertexPosNormTex(-3, 1, 0, 0, 1, 0, 0, 0),
                new CgVertexPosNormTex(3, 1, 0, 0, 1, 0, 1, 0),
                new CgVertexPosNormTex(3, -1, 0, 0, 1, 0, 1, 1),
            };
            var indices = new[]
            {
                0, 1, 2, 0, 2, 3,
            };
            return FlexibleModelHelpers.CreateSimple(cSource, vertices, indices, FlexibleModelPrimitiveTopology.TriangleList);
        }

        public IResourceSource GetModelSource()
        {
            return new Source(FactoryResourceCache, this);
        }
    }
}